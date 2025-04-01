using UnityEngine;

public class EnemyFallingState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isTrapped");
    }

    public override void Update(EnemyController controller)
    {
        if (controller.IsGrounded()){
            //controller.ResetVelocity();
            controller.ChangeState(new EnemyChaseState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isTrapped");
    }
}