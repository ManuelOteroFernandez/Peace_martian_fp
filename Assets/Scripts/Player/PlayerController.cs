using PlayerStateMachine;
using UnityEditor.Callbacks;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

//TODO: Unificar velocity o addForce. No mezclar.
public class PlayerController : MonoBehaviour
{
    [Header("Testing")]
    [SerializeField] int targetFps;

    [Header("Movement Parameters")]
    [SerializeField] float horizontalMovementSpeed;

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

    [Header("States")]
    PlayerState currentState;

    [Header("Ground Check Properties")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Transform groundCheckPoint;
    bool wasGroundedLastFrame;
    public bool isGrounded {get; private set;}

    void Awake() {
        //Application.targetFrameRate = targetFps;

        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inputController = GetComponent<PlayerInputController>();
    }
    void Start()
    {
        ChangeState(new PlayerIdleState());
    }

    void FixedUpdate()
    {
        CheckGrounded();
        ApplyGravityScaling();
        UpdateCooldowns();
        currentState.Update(this, inputController);
    }

    public void ChangeState(PlayerState newState) {
        currentState?.Exit(this);
        currentState = newState;

        Debug.Log("current state: " + currentState);

        currentState.Enter(this);
    }

    public void Move(float direction) {
        rigidbody2D.linearVelocity = new Vector2(Time.fixedDeltaTime * direction * horizontalMovementSpeed, rigidbody2D.linearVelocityY);

        Flip(direction);
    }

    void Flip(float direction) {
        if (direction != 0) {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }
    }

    public void Jump() {
        if (isGrounded || coyoteTimeCounter > 0) {
            DoJump(jumpForce);
            jumpedFromGround = true;
        } else {
            //Si no estamos en el suelo, podemos hacer un salto extra en el aire
            if (jumpedFromGround && jumpCount < 2) {
                DoJump(airJumpForce);
            } else if (jumpCount < 1) {
                DoJump(airJumpForce);
            }
        }
    }

    void DoJump(float jumpForce) {
        rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, 0);
        rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        jumpCount++;
    }

    public void Dash() {
        dashCurrentCd = dashCd;
        //Desactivamos la gravedad para no caer durante un dash aéreo
        rigidbody2D.gravityScale = 0;
        rigidbody2D.linearVelocity = new Vector2(Time.fixedDeltaTime * transform.localScale.x * dashSpeed, 0);
    }

    public void EndDash() {
        rigidbody2D.gravityScale = gravityScale;
        rigidbody2D.linearVelocity = new Vector2(0, 0);
    }

    void UpdateCooldowns() {
        //Dash cooldown
        if (dashCurrentCd > 0) {
            dashCurrentCd = Mathf.Clamp(dashCurrentCd - Time.fixedDeltaTime, 0, Mathf.Infinity);
        } else if (!canDash){
            canDash = true;
        }
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





    /*=============TESTING=============*/
        private void OnDrawGizmos()
    {
        //Dibuja el área de detección del suelo para depuración
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundCheckSize);
    }

}
