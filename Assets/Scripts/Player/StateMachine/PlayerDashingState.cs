using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerDashingState : PlayerState
    {
        public override void Enter(PlayerController controller)
        {
            controller.animator.SetTrigger("isDashing");
        }

        public override void Update(PlayerController controller, PlayerInputController inputController)
        {
            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller)
        {
            controller.animator.SetBool("isDashing", false);
        }
    }
}