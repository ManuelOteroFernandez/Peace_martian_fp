using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Parámetros")]
    [SerializeField] float movementSpeed;
    Rigidbody2D rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        rigidbody2D.linearVelocity = new Vector2(Time.deltaTime * horizontalMovement * movementSpeed, 0);
    }
}
