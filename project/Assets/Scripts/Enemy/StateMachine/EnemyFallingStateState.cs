using UnityEngine;

public class EnemyFallingState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isTrapped");
        // Debug.Log("start FAlling");
    }

    public override void Update(EnemyController controller) {
        if (!controller.IsGrounded()) return;

        if (controller.IsTargetInChaseRange())
        {
            controller.ChangeState(new EnemyChaseState());
        }
        else
        {
            controller.ChangeState(new EnemyRetreatState());

        }
    }

    public override void Exit(EnemyController controller) {
        controller.animator.ResetTrigger("isTrapped");
        // Debug.Log("end FAlling");
    }
}