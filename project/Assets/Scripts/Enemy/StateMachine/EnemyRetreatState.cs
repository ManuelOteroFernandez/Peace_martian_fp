using UnityEngine;

public class EnemyRetreatState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isRunning");
        controller.aiAgent.RelocateCurrentWaypoint();
        controller.aiAgent.ResetRouteToOrigin();
        controller.aiAgent.ResetRouteToOriginWaypointIndex();
        controller.aiAgent.CalculateRouteToOrigin();
        controller.aiAgent.UpdateRouteToOriginNextWaypoint();
        // Debug.Log("Start RETREAT");
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

        if (controller.IsFalling()) {
            controller.ChangeState(new EnemyFallingState());
        }
        
        if (controller.IsTargerInDetectionRange() && !controller.IsTargetInAttackRange())
        {
            // Debug.Log("Lo veooo");
            controller.ChangeState(new EnemyChaseState());
        }

        if (controller.IsTrappedEnemyInAttackRange() || (controller.IsTargetInAttackRange() && controller.IsGrounded() && controller.IsAttackCooldownReady())) {
            controller.ChangeState(new EnemyAttackState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isRunning");
        controller.aiAgent.ResetRouteToOrigin();
        // Debug.Log("end RETREAT");
    }
}