using System;
using System.Collections;
using PlayerStateMachine;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.SceneManagement;

//TODO: Unificar velocity o addForce. No mezclar.
//TODO: Controlar velocidad máxima de caída.
public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float horizontalMovementSpeed;
    [SerializeField] float airHorizontalMovementSpeed;

    [Header("Skills")]
    [SerializeField] bool dashUnlocked;
    [SerializeField] bool doubleJumpUnlocked;

    [Header("Effects")]
    [SerializeField] GameObject dashEffectPrefab;
    [SerializeField] GameObject doubleJumpEffectPrefab;
    [SerializeField] Vector2 dashEffectOffset = new Vector2(-2, 0);
    [SerializeField] Vector2 doubleJumpEffectOffset = new Vector2 (-1, -1.5f);

    [Header("HP")]
    [SerializeField] bool hasArmor;
    [SerializeField] float PLAYER_RESPAWN_TIME = 2f;
    bool isDying = false;

    [Header("Dash Parameters")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    public float DashDuration => dashDuration;
    [SerializeField] float dashCd;
    float dashCurrentCd;
    public bool canDash {get; private set;}

    [Header("Jump Parameters")]
    [SerializeField] float jumpForce;
    [SerializeField] float airJumpForce;
    [SerializeField] float gravityScale;
    [SerializeField] float fallGravityScale;
    int jumpCount;
    bool jumpedFromGround;

    [Header("Coyote Time")]
    [SerializeField] float coyoteTime;
    float coyoteTimeCounter;

    [Header("Components")]
    public Rigidbody2D rigidbody2D {get; private set;}
    public Animator animator {get; private set;}
    public PlayerInputController inputController {get; private set;}
    public WeaponManager weaponManager {get; private set;}
    public Collider2D collider {get; private set;}

    [Header("States")]
    PlayerState currentState;

    [Header("Ground Check Properties")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Transform groundCheckPoint;
    bool wasGroundedLastFrame;
    public bool isGrounded {get; private set;}

    [Header("Audio")]
    [SerializeField] AudioClip walkSFX;
    [SerializeField] AudioClip dashSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip doubleJumpSFX;
    [SerializeField] AudioClip landSFX;

    [Header("References")]
    public bool DoubleJumpUnlocked => doubleJumpUnlocked;
    public bool DashUnlocked => dashUnlocked;
    public bool HasArmor => hasArmor;

    [Header("PowerUps")]
    [SerializeField] GameObject mochila;
    [SerializeField] GameObject tubo;
    [SerializeField] GameObject shild;
    public Animator mochilaAnimator {get; private set;}
    public Animator tuboAnimator {get; private set;}
    public Animator armaAnimator {get; private set;}

    void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        inputController = GetComponent<PlayerInputController>();
        weaponManager = GetComponentInChildren<WeaponManager>();

        mochilaAnimator = mochila.GetComponent<Animator>();
        tuboAnimator = tubo.GetComponent<Animator>();
        armaAnimator = weaponManager.GetComponent<Animator>();
    }
    void Start() {
        ChangeState(new PlayerIdleState());

        tubo.SetActive(dashUnlocked);
        mochila.SetActive(doubleJumpUnlocked);
        shild.SetActive(hasArmor);
    }

    void FixedUpdate()
    {
        RemoveSliding();
        CheckGrounded();
        ApplyGravityScaling();
        UpdateCooldowns();
        UpdateState();
        UpdateAnimator();
    }

    void UpdateState() {
        currentState.Update(this, inputController);
    }

    void RemoveSliding() {
        if (isGrounded && inputController.horizontalInput < 0.1f && currentState is not PlayerDashingState) {
            rigidbody2D.linearVelocity = new Vector2(0, rigidbody2D.linearVelocityY);
        }
    }

    public void UpdateAnimator(){
        float aimDirection;
        float angle = weaponManager.GetCurrentWeapon().getAdjustedAngle();
        switch (angle)
        {
            case 90:
                aimDirection = 1;
                break;

            case 135:
            case 45: 
                aimDirection = 0.75f;
                break;

            case -45:
            case -135:
                aimDirection = 0.25f;
                break;
            
            case -90:
                aimDirection = 0;
                break;

            default: 
                aimDirection = 0.5f;
                break;
        }
        if(animator.GetFloat("aimDirection") != aimDirection)
            animator.SetFloat("aimDirection",aimDirection);
            mochilaAnimator.SetFloat("aimDirection",aimDirection);
            tuboAnimator.SetFloat("aimDirection",aimDirection);
            armaAnimator.SetFloat("aimDirection",aimDirection);
    }

    public void ChangeState(PlayerState newState) {
        currentState?.Exit(this);
        currentState = newState;

        //Debug.Log("current state: " + currentState);

        currentState.Enter(this);
    }

    public void Move(float direction) {

        float linearVelocityX = Time.fixedDeltaTime * direction;
        linearVelocityX *= isGrounded ? horizontalMovementSpeed : airHorizontalMovementSpeed;
        rigidbody2D.linearVelocity = new Vector2(linearVelocityX, rigidbody2D.linearVelocityY);
    }

    public void Flip(float direction) {
        if (direction != 0) {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }
    }

    public bool CanDoubleJump() {
        return (jumpedFromGround && jumpCount < 2) || (!jumpedFromGround && jumpCount < 1);
    }

    public void Jump() {
        //Guardamos un flag indicando si el saltos e hizo desde tierra o no para controlar si puede realizar doble salto o no
        jumpedFromGround = isGrounded;

        rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, 0);
        rigidbody2D.AddForce(new Vector2(0, isGrounded ? jumpForce : airJumpForce), ForceMode2D.Impulse);
        jumpCount++;
    }

    public void Dash() {
        if (canDash) {
            gameObject.layer = LayerMask.NameToLayer("Invulnerable");
            dashCurrentCd = dashCd;
            canDash = false;
            //Desactivamos la gravedad para no caer durante un dash aéreo
            rigidbody2D.gravityScale = 0;
            //El dash tiene 2 comportamientos distintos
            //Si el personaje está en movimiento horizontal, se desplaza en esa dirección
            //Si no, se desplaza en la dirección en la que mira
            if (rigidbody2D.linearVelocityX != 0) {
                rigidbody2D.linearVelocity = new Vector2(Time.fixedDeltaTime * Mathf.Sign(rigidbody2D.linearVelocityX) * dashSpeed, 0);
            } else {
                rigidbody2D.linearVelocity = new Vector2(Time.fixedDeltaTime * Mathf.Sign(transform.localScale.x) * dashSpeed, 0);
            }

            //Efecto de dash
            CastDashEffect();
        }
    }

    public void EndDash() {
        gameObject.layer = LayerMask.NameToLayer("Player");
        rigidbody2D.gravityScale = gravityScale;
        rigidbody2D.linearVelocity = new Vector2(0, 0);
    }

    public void ResetDash() {
        dashCurrentCd = 0;
        canDash = true;
    }

    public void AddArmor() {
        hasArmor = true;
        shild.SetActive(true);
    }

    public void UnlockDoubleJump() {
        doubleJumpUnlocked = true;
        mochila.SetActive(true);
    }

    public void UnlockDash() {
        dashUnlocked = true;
        tubo.SetActive(true);
    }

    void UpdateCooldowns() {
        //Dash cooldown
        if (dashCurrentCd > 0) {
            dashCurrentCd = Mathf.Clamp(dashCurrentCd - Time.fixedDeltaTime, 0, Mathf.Infinity);
        } else if (!canDash && isGrounded){
            canDash = true;
        }
    }

    public void UpdateWeaponState() {
        if(inputController.nextWeaponInput) {
            weaponManager.SwitchWeapon(true);
            inputController.ResetInput();
        } else if (inputController.previousWeaponInput) {
            weaponManager.SwitchWeapon(false);
            inputController.ResetInput();
        }

        weaponManager.ManageShooting(inputController);
    }

    void CheckGrounded() {
        isGrounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);

        //Para evitar que el contador de saltos se reinicie justo cuando saltamos porque el motor de física sigue detectando el suelo
        //Solo reiniciamos el contador si en el frame anterior no estaba en el suelo
        if (isGrounded && !wasGroundedLastFrame) {
            jumpCount = 0;
            coyoteTimeCounter = coyoteTime;
            jumpedFromGround = false;
        } else {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        wasGroundedLastFrame = isGrounded;
    }

    private void ApplyGravityScaling()
    {
        //Para evitar caer en un dash, tenemos que evitar que el sistema actualice la gravedad
        if (currentState is PlayerDashingState) {
            return;
        }
        //Para que el movimiento sea más natural y evitar la sensación de flotar, hacemos que la gravedad sea más fuerte al caer
        if (rigidbody2D.linearVelocityY > 0) {
            rigidbody2D.gravityScale = gravityScale; 
        } else {
            rigidbody2D.gravityScale = fallGravityScale;
        }
    }

    void PlayWalkSFX() {
        if (isGrounded) {
            AudioManager.Instance.PlaySFX(walkSFX, true);
        }
    }

    public void StartWalkSFX()
    {
        CancelInvoke(nameof(PlayWalkSFX));
        InvokeRepeating(nameof(PlayWalkSFX), 0f, 0.45f);
    }

    public void StopWalkSFX()
    {
        CancelInvoke(nameof(PlayWalkSFX));
    }

    public void PlayDashSFX() {
        AudioManager.Instance.PlaySFX(dashSFX);
    }

    public void PlayJumpSFX() {
        AudioManager.Instance.PlaySFX(jumpSFX);
    }

    public void PlayDoubleJumpSFX() {
        AudioManager.Instance.PlaySFX(doubleJumpSFX);
    }

    public void PlayLandSFX() {
        AudioManager.Instance.PlaySFX(landSFX);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyAttack") || collision.CompareTag("EnemyProjectile")) {
            if (hasArmor) {
                hasArmor = false;
                shild.SetActive(false);
            } else {
                Die();
            }
        }
        if (collision.CompareTag("Finish"))
        {
            Win();
        }
    }

    void Die() {
        StartCoroutine(DeathAndRespawn());
    }
    void Win() {
        StartCoroutine(WinAndRun());
    }

    IEnumerator WinAndRun() {
        inputController.enabled = false;
        // FIXME Aqui deberia moverse pero no lo hace
        rigidbody2D.linearVelocity = new Vector2(4000, 0);
        
        
        yield return new WaitForSeconds(PLAYER_RESPAWN_TIME);
        
        rigidbody2D.linearVelocity = new Vector2(0, 0);
        
    }

    IEnumerator DeathAndRespawn() {
        isDying = true;
        inputController.enabled = false;
        rigidbody2D.linearVelocity = new Vector2(0, 10);
        collider.enabled = false;

        yield return new WaitForSeconds(PLAYER_RESPAWN_TIME);

        rigidbody2D.gravityScale = 0;
        GameManager.Instance.Defeat();
    }

    public void CastDashEffect() {
        Vector3 spawnPos = (Vector2)transform.position + dashEffectOffset * transform.localScale.x;
        spawnPos.z = -10;

        int particleScale = rigidbody2D.linearVelocityX > 0 ? -1 : 1;

        ParticleSystem ps = dashEffectPrefab.GetComponent<ParticleSystem>();
        var velocity = ps.velocityOverLifetime;
        velocity.x = new ParticleSystem.MinMaxCurve(0.5f * particleScale);

        Instantiate(dashEffectPrefab, spawnPos, Quaternion.identity, transform);
    }

    public void CastJumpEffect() {
         Vector3 spawnPos = (Vector2)transform.position + new Vector2(doubleJumpEffectOffset.x * transform.localScale.x, doubleJumpEffectOffset.y);
         spawnPos.z = -10;

         Instantiate(doubleJumpEffectPrefab, spawnPos, doubleJumpEffectPrefab.transform.rotation, transform);
    }




    /*=============TESTING=============*/
    private void OnDrawGizmos() {
        //Dibuja el área de detección del suelo para depuración
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }

}
