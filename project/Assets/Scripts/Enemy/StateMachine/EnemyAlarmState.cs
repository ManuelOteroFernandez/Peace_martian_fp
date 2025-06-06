using UnityEngine;

public class EnemyAlarmState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isRunning");
    }

    public override void Update(EnemyController controller)
    {
        
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isRunning");
    }
}