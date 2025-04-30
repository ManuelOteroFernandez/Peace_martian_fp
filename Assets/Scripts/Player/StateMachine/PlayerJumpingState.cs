using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerJumpingState : PlayerState
    {
        public override int id {
            get {
                return (int)PlayerStateID.Jumping;
            }
        }

        public override void Enter(PlayerController controller) {
            controller.animator.SetInteger("currentStateId", id);
            controller.animator.SetTrigger("isJumping");
            Jump(controller);
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            controller.UpdateWeaponState();
            controller.Move(inputController.horizontalInput);
            controller.Flip(inputController.aimDirection.x);

            if (inputController.jumpInput && controller.CanDoubleJump() && controller.DoubleJumpUnlocked){
                Jump(controller);
                controller.CastJumpEffect();
            }

            if(!controller.isGrounded && controller.rigidbody2D.linearVelocityY < 0) {
                controller.ChangeState(new PlayerFallingState());
            }

            if(inputController.dashInput && controller.canDash && controller.DashUnlocked) {
                controller.ChangeState(new PlayerDashingState());
            }

            if (controller.isGrounded){
                controller.ChangeState(new PlayerIdleState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            controller.animator.ResetTrigger("isJumping");
            controller.animator.ResetTrigger("isDoubleJumping");
        }

        void Jump(PlayerController controller) {
            PlayJumpSFX(controller);
            controller.Jump();

            if (!controller.isGrounded) {
                controller.animator.ResetTrigger("isJumping");
                controller.animator.SetTrigger("isDoubleJumping");
            }
        }

        void PlayJumpSFX(PlayerController controller) {
            if (controller.isGrounded) {
                controller.PlayJumpSFX();
            } else {
                controller.PlayDoubleJumpSFX();
            }
        }
    }
}
