using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemyController : EnemyController {
    [Header("ShootParameters")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] protected float aimOffsetAngle = 5f;

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
        Transform target = this.target;

        if (ChooseToFreeEnemy() && trappedEnemyTarget != null) {
            target = trappedEnemyTarget;    
        }

        rigidbody2D.linearVelocity = Vector2.zero;

        float randomAimOffset = Random.Range(-aimOffsetAngle, aimOffsetAngle);

        Vector2 direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float finalAngle = angle + randomAimOffset;

        Vector2 offsetDirection = new Vector2(
            Mathf.Cos(finalAngle * Mathf.Deg2Rad),
            Mathf.Sin(finalAngle * Mathf.Deg2Rad)
        ).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, finalAngle));

        EnemyBulletController bulletController = projectile.GetComponent<EnemyBulletController>();
        if (bulletController != null) {
            bulletController.Initialize(offsetDirection);
        }

        currentAttackCooldown = attackCooldown;
    }

}
