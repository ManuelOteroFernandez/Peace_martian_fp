using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isIdle");
    }

    public override void Update(EnemyController controller)
    {
        if (controller.enemyHealth.IsInBubble()) {
            controller.ChangeState(new EnemyBubbleTrappedState());
        }

        if (controller.IsTargerInDetectionRange() && !controller.IsTargetInAttackRange()) {
            controller.ChangeState(new EnemyChaseState());
        }

        if (controller.IsTargetInAttackRange() && controller.IsGrounded()) {
            controller.ChangeState(new EnemyAttackState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isIdle");
    }
}