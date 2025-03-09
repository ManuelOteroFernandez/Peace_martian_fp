using System.Collections;
using UnityEngine;

public class MeleeEnemyController : EnemyController {
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
        currentAttackCooldown = attackCooldown;
    }
}
