using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerRunningState : PlayerState
    {
        public override void Enter(PlayerController controller) {
            controller.animator.SetTrigger("isRunning");
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            UpdateWeaponState(controller, inputController);
            controller.Move(inputController.horizontalInput);
            controller.Flip(inputController.aimDirection.x);

            if (inputController.horizontalInput == 0) {
                controller.ChangeState(new PlayerIdleState());
            }

            if (inputController.jumpInput) {
                controller.ChangeState(new PlayerJumpingState());
            }

            if(inputController.dashInput) {
                controller.ChangeState(new PlayerDashingState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            controller.animator.ResetTrigger("isRunning");
        }
    }
}
