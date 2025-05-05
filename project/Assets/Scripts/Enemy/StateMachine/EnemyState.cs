using UnityEngine;

public class EnemyState
{
    public virtual void Enter(EnemyController controller){}
    public virtual void Update(EnemyController controller){}
    public virtual void Exit(EnemyController controller){}
}
