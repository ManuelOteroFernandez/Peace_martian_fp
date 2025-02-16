using UnityEngine;

public class BubbleProjectileScript : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] float speed;
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            bounces++;
            if (bounces >= maxBounces)
            {
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile(){
        Instantiate(bubblePopEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
