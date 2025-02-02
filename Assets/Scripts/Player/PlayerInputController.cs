using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [Header("Input")]
    public float horizontalInput {get; private set;}
    public bool jumpInput {get; private set;}
    public bool dashInput {get; private set;}

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferTime;
    private float jumpBufferCounter;


    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpInput = true;
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Dash"))
        {
            dashInput = true;
        }
    }

    public bool UseJump() {
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter = 0;
            jumpInput = false;
            return true;
        }
        return false;
    }

    public void ResetInput()
    {
        jumpInput = false;
        dashInput = false;
    }
}
