using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Input")]
    public float horizontalInput {get; private set;} //Joystick izquierdo
    public bool jumpInput {get; private set;} //X
    public bool dashInput {get; private set;} //L2
    public bool nextWeaponInput {get; private set;} //Cruceta derecha
    public bool previousWeaponInput {get; private set;} //Cruceta izquierda
    public bool shootInput {get; private set;} //R2
    public bool shootHoldInput {get; private set;} //R2
    public Vector2 aimDirection {get; private set;} //Joystick derecho - Ratón

    private bool forceRelease = false;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;

    [Header("Joystick Deadzone")]
    [SerializeField] private float joystickDeadzone;


    void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimDirection = (mousePosition - (Vector2)transform.position).normalized;

        /*Vector2 joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (joystickInput.magnitude < joystickDeadzone) {
            aimDirection = joystickInput.normalized;
        }*/

        if (Input.GetButtonDown("Jump")) {
            jumpInput = true;
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Dash")) {
            dashInput = true;
        }

        if (Input.GetButtonDown("NextWeapon")) {
            nextWeaponInput = true;
        }

        /*if (Input.GetButtonDown("PreviousWeapon")) {
            previousWeaponInput = true;
        }*/
    
        if (Input.GetButtonDown("Fire1")) {
            shootInput = true;
        }

        //Mecanismo para forzar al jugador a soltar el botón de disparo antes de poder volver a disparar
        bool isHoldingShoot = Input.GetButton("Fire1");
        if (forceRelease) {
            if (!isHoldingShoot) {
                forceRelease = false;
            }

            shootHoldInput = false;
        } else {
            shootHoldInput = isHoldingShoot;
        }
    }

    public bool UseJump() {
        if (jumpBufferCounter > 0) {
            jumpBufferCounter = 0;
            jumpInput = false;

            return true;
        }

        return false;
    }

    public void ResetInput() {
        jumpInput = false;
        dashInput = false;
        shootInput = false;
        nextWeaponInput = false;
        //previousWeaponInput = false;
    }

    public void ForceRelease() {
        forceRelease = true;
    }
}
