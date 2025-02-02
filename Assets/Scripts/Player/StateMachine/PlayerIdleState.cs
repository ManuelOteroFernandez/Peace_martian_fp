using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerIdleState : PlayerState
    {
        public override void Enter(PlayerController controller) {
            controller.animator.SetTrigger("isIdle");
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            if (inputController.horizontalInput != 0) {
                controller.ChangeState(new PlayerRunningState());
            }

            if(inputController.jumpInput) {
                controller.ChangeState(new PlayerJumpingState());
            }

            if(inputController.dashInput) {
                controller.ChangeState(new PlayerDashingState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            controller.animator.ResetTrigger("isIdle");
        }
    }
}
