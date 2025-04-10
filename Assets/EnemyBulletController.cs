using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float lifetime = 3f;
    Vector2 direction;
    Rigidbody2D rigidbody2D;

    void Start() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rigidbody2D.linearVelocity = direction * speed;
    }

    public void Initialize(Vector2 direction) {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Limits")){
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        if (collision.CompareTag("Limits")) {
            Destroy(gameObject);
        }
    }
}
