using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.StartChasing();
        controller.animator.SetTrigger("isRunning");
    }

    public override void Update(EnemyController controller)
    {
        controller.MoveEnemy();

        if (controller.enemyHealth.IsInBubble()) {
            controller.ChangeState(new EnemyBubbleTrappedState());
        }

        if (controller.rigidbody2D.linearVelocityY < 0) {
            controller.ChangeState(new EnemyFallingState());
        }

        if (controller.DistanceToOriginWaypoint() > controller.MaxDistanceFromOrigin || controller.CantReachTarget()) {
            controller.ChangeState(new EnemyRetreatState());
        }

        if (controller.IsTrappedEnemyInAttackRange() || (controller.IsTargetInAttackRange() && controller.IsGrounded() && controller.IsAttackCooldownReady())) {
            controller.ChangeState(new EnemyAttackState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isRunning");
    }
}