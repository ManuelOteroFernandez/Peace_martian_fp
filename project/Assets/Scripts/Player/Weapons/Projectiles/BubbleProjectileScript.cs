using UnityEngine;

public class BubbleProjectileScript : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float maxBounces;
    [SerializeField] float lifeTime;
    [SerializeField] GameObject bubblePopEffect;
    int bounces;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * Time.deltaTime * speed;
    }

    private void Update() {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Traps")) {
            DestroyProjectile();
        } else {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                bounces++;
                if (bounces >= maxBounces) {
                    DestroyProjectile();
                }
            }

            if (collision.gameObject.CompareTag("Enemy")) {
                EnemyHealth healthComponent = collision.gameObject.GetComponent<EnemyHealth>();
                if (healthComponent == null) {
                    return;
                }

                healthComponent.TakeBubbleDamage(damage);
                Destroy(gameObject);
            }
        }
    }

    void DestroyProjectile(){
        Instantiate(bubblePopEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
