using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2D;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetBool("isRunning", rigidbody2D.linearVelocityX != 0);
    }
}
