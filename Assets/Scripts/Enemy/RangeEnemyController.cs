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

    public override void MoveEnemy() {
        aiAgent.UpdateFieldFlowNextWaypoint();

        if (aiAgent.nextWaypoint == null){
            return;
        }

        if (aiAgent.nextWaypoint.type == WaypointType.Cliff) {
            if (!aiAgent.CanJumpFromCliff(aiAgent.nextWaypoint)) {
                aiAgent.nextWaypoint = null;
                rigidbody2D.linearVelocity = Vector2.zero;
                return;
            }
        }

        Vector2 direction = aiAgent.nextWaypoint.position - aiAgent.currentWaypoint.position;
        Flip(-direction.x);
        enemyPosition = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);

        if (IsGrounded() || aiAgent.currentWaypoint.type != WaypointType.Cliff) {
            Vector2 horizontalVelocity = direction * chaseSpeed;
            rigidbody2D.linearVelocity = new Vector2(horizontalVelocity.x, rigidbody2D.linearVelocity.y);
        }

        if (Vector2.Distance(enemyPosition, aiAgent.nextWaypoint.position) < 0.5f){
            if (aiAgent.nextWaypoint.type == WaypointType.Ladder) {
                transform.position = new Vector3(aiAgent.nextWaypoint.position.x, transform.position.y, 0);
            }
            aiAgent.AdvanceToNextWaypoint();
        }
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
