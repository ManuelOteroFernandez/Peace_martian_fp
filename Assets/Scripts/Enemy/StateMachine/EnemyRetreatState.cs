using UnityEngine;

public class EnemyRetreatState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isRunning");
    }

    public override void Update(EnemyController controller)
    {
        controller.ReturnToOrigin();

        if (controller.IsBackInOrigin()) {
            controller.ChangeState(new EnemyPatrolState());
        }

        if (controller.enemyHealth.IsInBubble()) {
            controller.ChangeState(new EnemyBubbleTrappedState());
        }

        if (controller.rigidbody2D.linearVelocityY < 0) {
            controller.ChangeState(new EnemyFallingState());
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