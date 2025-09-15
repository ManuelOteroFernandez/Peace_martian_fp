using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.StartChasing();
        controller.animator.SetTrigger("isRunning");
        // Debug.Log("Start CHASE");
    }

    public override void Update(EnemyController controller) {
        controller.MoveToTarget();

        if (controller.enemyHealth.IsInBubble()) {
            controller.ChangeState(new EnemyBubbleTrappedState());
        } else {
            if (controller.IsFalling()) {
                controller.ChangeState(new EnemyFallingState());
            }

            if (controller.CantReachTarget()) {
                controller.ChangeState(new EnemyRetreatState());
            }

            if ((controller is RangeEnemyController) && controller.IsTrappedEnemyInAttackRange() 
                || (controller.IsTargetInAttackRange() && controller.IsGrounded() && controller.IsAttackCooldownReady())) {
                controller.ChangeState(new EnemyAttackState());
            }
        }
    }


    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isRunning");
        // Debug.Log("end CHASE");
    }
}