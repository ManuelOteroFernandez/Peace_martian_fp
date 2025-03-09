using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public override void Enter(EnemyController controller)
    {
        controller.animator.SetTrigger("isDead");
    }

    public override void Update(EnemyController controller)
    {
        if (controller.IsAttackCooldownReady()) {
            controller.StartCoroutine(ExecuteAttack(controller));
        }
    }

    IEnumerator ExecuteAttack(EnemyController controller) {
        controller.EnemyAttack();

        yield return new WaitForSeconds(controller.AttackDuration);

        if (controller.IsTargetInAttackRange() && controller.IsAttackCooldownReady()) {
            controller.StartCoroutine(ExecuteAttack(controller));
        } else {
            controller.ChangeState(new EnemyChaseState());
        }
    }

    public override void Exit(EnemyController controller)
    {
        controller.animator.ResetTrigger("isIdle");
    }
}