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
            controller.armaAnimator.SetInteger("currentStateId", id);
            controller.animator.SetTrigger("isJumping");
            controller.armaAnimator.SetTrigger("isJumping");
            
            if (controller.mochilaAnimator.isActiveAndEnabled)
            { 
                controller.mochilaAnimator.SetInteger("currentStateId", id);
                controller.mochilaAnimator.SetTrigger("isJumping");
            }
            if (controller.tuboAnimator.isActiveAndEnabled)
            { 
                controller.tuboAnimator.SetInteger("currentStateId", id);
                controller.tuboAnimator.SetTrigger("isJumping");
            }
            
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

            if(!controller.isGrounded && controller.rb2d.linearVelocityY < 0) {
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
            controller.armaAnimator.ResetTrigger("isJumping");

            if (controller.mochilaAnimator.isActiveAndEnabled)
            {
                controller.mochilaAnimator.ResetTrigger("isJumping");
            }
            if (controller.tuboAnimator.isActiveAndEnabled)
            {
                controller.tuboAnimator.ResetTrigger("isJumping");
            }
        }

        void Jump(PlayerController controller) {
            PlayJumpSFX(controller);
            controller.Jump();

            if (!controller.isGrounded) {
                controller.animator.ResetTrigger("isJumping");
                controller.armaAnimator.ResetTrigger("isJumping");
                
                if (controller.mochilaAnimator.isActiveAndEnabled) 
                {
                    controller.mochilaAnimator.ResetTrigger("isJumping");
                }
                if (controller.tuboAnimator.isActiveAndEnabled)
                {
                    controller.tuboAnimator.ResetTrigger("isJumping");
                }
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
