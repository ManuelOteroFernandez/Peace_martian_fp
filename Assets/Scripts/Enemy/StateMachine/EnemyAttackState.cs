using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isDead");
    }

    public override void Update(EnemyController controller)
    {
        
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isIdle");
    }
}