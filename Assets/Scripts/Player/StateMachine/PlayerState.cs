using UnityEngine;

public class PlayerState
{
    public virtual void Enter(PlayerController controller){}
    public virtual void Update(PlayerController controller, PlayerInputController inputController){}
    public virtual void Exit(PlayerController controller){}
}
