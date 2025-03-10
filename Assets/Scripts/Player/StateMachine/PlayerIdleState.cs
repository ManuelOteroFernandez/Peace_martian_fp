using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerIdleState : PlayerState
    {
        public override void Enter(PlayerController controller) {
            controller.animator.SetTrigger("isIdle");
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            controller.UpdateWeaponState();
            controller.Flip(inputController.aimDirection.x);

            if (inputController.horizontalInput != 0) {
                controller.ChangeState(new PlayerRunningState());
            }

            if(inputController.jumpInput) {
                controller.ChangeState(new PlayerJumpingState());
            }

            if(inputController.dashInput && controller.canDash) {
                controller.ChangeState(new PlayerDashingState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            controller.animator.ResetTrigger("isIdle");
        }
    }
}
