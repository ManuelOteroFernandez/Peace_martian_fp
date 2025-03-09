using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemyController : EnemyController {
    [SerializeField] GameObject projectilePrefab;

    protected override void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void EnemyAttack() {
        rigidbody2D.linearVelocity = Vector2.zero;

        Vector2 direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, angle));

        EnemyBulletController bulletController = projectile.GetComponent<EnemyBulletController>();
        if (bulletController != null) {
            bulletController.Initialize(direction);
        }

        currentAttackCooldown = attackCooldown;
    }
}
