using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isRunning");
    }

    public override void Update(EnemyController controller)
    {
        controller.Patrol();

        if (controller.enemyHealth.IsInBubble()) {
            controller.ChangeState(new EnemyBubbleTrappedState());
        }

        if (controller.IsTargetInChaseRange() && !controller.IsTargetInAttackRange()) {
            controller.ChangeState(new EnemyChaseState());
        }

        if (controller.IsTargetInAttackRange() && controller.IsGrounded()) {
            controller.ChangeState(new EnemyAttackState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isRunning");
    }
}