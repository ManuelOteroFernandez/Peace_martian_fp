using UnityEngine;

public class EnemyBubbleTrappedState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isTrapped");
    }

    public override void Update(EnemyController controller)
    {
        if (!controller.enemyHealth.IsInBubble())
        {
            controller.ChangeState(new EnemyFallingState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isTrapped");
    }
}