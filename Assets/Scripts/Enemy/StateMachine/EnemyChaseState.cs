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

        if (controller.rigidbody2D.linearVelocityY < 0) {
            controller.ChangeState(new EnemyFallingState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isRunning");
    }
}