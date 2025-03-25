using System.Collections;
using UnityEngine;

public class BigBubbleTrap : MonoBehaviour
{
    [Header("Bubble Trap Settings")]
    [SerializeField] private float maxFloatSpeed = 1.5f;
    [SerializeField] private LayerMask trapLayer;
    private Collider2D mapLimits;

    [Header("Components")]
    Rigidbody2D rigidbody2D;

    [Header("Trapped Enemy")]
    private GameObject trappedEnemy;
    private Rigidbody2D enemyRigidbody2D;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        mapLimits = GameObject.FindGameObjectWithTag("Limits").GetComponent<Collider2D>();
    }

    private void Update() {
        //TODO: Destruir burbuja y enemigo si se salen de X límites.
        /*if (!mapLimits.bounds.Contains(transform.position)) {
            Destroy(trappedEnemy);
            Destroy(gameObject);
        }*/

        if (rigidbody2D.linearVelocity.y > maxFloatSpeed) {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, maxFloatSpeed);
        }

        trappedEnemy.transform.position = transform.position;
    }

    public void Initialize(GameObject enemy){
        trappedEnemy = enemy;
        enemyRigidbody2D = enemy.GetComponent<Rigidbody2D>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), trappedEnemy.GetComponent<Collider2D>(), true);
        enemyRigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        rigidbody2D.gravityScale = -1;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (((1 << collision.gameObject.layer) & trapLayer) != 0){
            if (trappedEnemy != null && enemyRigidbody2D != null){
                ReleaseEnemy();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyProjectile")) {
            ReleaseEnemy();
        }
    }

    public void ReleaseEnemy() {
        enemyRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        enemyRigidbody2D.rotation = 0;
        enemyRigidbody2D.angularVelocity = 0;

        trappedEnemy.GetComponent<EnemyHealth>().ReleaseFromBubble();
        Destroy(gameObject);
    }
}
