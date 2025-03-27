using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerRunningState : PlayerState
    {
        public override int id {
            get {
                return 1;
            }
        }

        public override void Enter(PlayerController controller) {
            controller.animator.SetInteger("currentStateId", id);
            controller.StartWalkSFX();
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            controller.UpdateWeaponState();
            controller.Move(inputController.horizontalInput);
            controller.Flip(inputController.aimDirection.x);

            if(!controller.isGrounded && controller.rigidbody2D.linearVelocityY < 0) {
                controller.ChangeState(new PlayerFallingState());
            }

            if (inputController.horizontalInput == 0) {
                controller.ChangeState(new PlayerIdleState());
            }

            if (inputController.jumpInput) {
                controller.ChangeState(new PlayerJumpingState());
            }

            if(inputController.dashInput && controller.canDash && controller.DashUnlocked) {
                controller.ChangeState(new PlayerDashingState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            controller.StopWalkSFX();
        }
    }
}
