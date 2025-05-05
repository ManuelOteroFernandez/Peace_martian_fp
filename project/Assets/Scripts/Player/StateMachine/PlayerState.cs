public class PlayerState
{
    public enum PlayerStateID : int
    {
        Idle = 0,
        Running = 1,
        Falling = 2,
        Jumping = 3,
        Dashing = 4,
    }

    public virtual int id { get; }
    public virtual void Enter(PlayerController controller){}
    public virtual void Update(PlayerController controller, PlayerInputController inputController){}
    public virtual void Exit(PlayerController controller){}
}
