using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerInputController : MonoBehaviour
{
    PlayerInputActions inputActions;

    [Header("Input")]
    public float horizontalInput {get; private set;}
    public bool jumpInput {get; private set;}
    public bool dashInput {get; private set;}
    public bool nextWeaponInput {get; private set;}
    public bool previousWeaponInput {get; private set;}
    public bool shootInput {get; private set;}
    public bool shootHoldInput {get; private set;}
    public Vector2 aimDirection {get; private set;}    

    private bool forceRelease = false;
    private bool isUsingJoystick = false;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;

    [Header("Joystick Deadzone")]
    [SerializeField] private float joystickDeadzone = 0.3f;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Awake() {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => {
            Vector2 moveInput = ctx.ReadValue<Vector2>();

            horizontalInput = moveInput.x != 0 ? Mathf.Sign(moveInput.x) : 0;
        };

        inputActions.Player.Move.canceled += ctx => horizontalInput = 0;

        inputActions.Player.AimJoystick.performed += ctx => {
            Vector2 joystickInput = ctx.ReadValue<Vector2>();

            //Se aplica un deadzone al joystick para evitar valores residuales que no filtra el propio Input System
            if (joystickInput.magnitude > joystickDeadzone) 
            {
                aimDirection = joystickInput.normalized;
                isUsingJoystick = true;
            }
        };

        inputActions.Player.AimJoystick.canceled += ctx => {
            aimDirection = Vector2.zero;
            isUsingJoystick = false;
        };

        inputActions.Player.AimMouse.performed += ctx =>
        {
            if (!isUsingJoystick) {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
                aimDirection = (mousePosition - (Vector2)transform.position).normalized;
            }
        };

        inputActions.Player.Jump.performed += ctx => jumpInput = true;

        inputActions.Player.Dash.performed += ctx => dashInput = true;

        inputActions.Player.Shoot.performed += ctx => shootInput = true;

        inputActions.Player.Shoot.performed += ctx => shootHoldInput = true;
        inputActions.Player.Shoot.canceled += ctx => shootHoldInput = false;

        inputActions.Player.NextWeapon.performed += ctx => nextWeaponInput = true;
        inputActions.Player.PreviousWeapon.performed += ctx => previousWeaponInput = true;
    }

    private void Update() {
        //Se aplica un buffer al salto para que el usuario no sienta que se ignoran sus controles.
        if (jumpInput) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }

        //Se fuerza al jugador a volver a disparar
        if (forceRelease) {
            if (!shootHoldInput) {
                forceRelease = false;
            }

            shootHoldInput = false;
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
        previousWeaponInput = false;
    }

    public void ForceRelease() {
        forceRelease = true;
    }
}
