using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerFallingState : PlayerState
    {
        public override int id {
            get {
                return (int)PlayerStateID.Falling;
            }
        }

        public override void Enter(PlayerController controller) {
            controller.animator.SetInteger("currentStateId", id);
            controller.armaAnimator.SetInteger("currentStateId", id);
            
            if (controller.mochilaAnimator.isActiveAndEnabled)
            { 
                controller.mochilaAnimator.SetInteger("currentStateId", id);
            }
            if (controller.tuboAnimator.isActiveAndEnabled)
            { 
                controller.tuboAnimator.SetInteger("currentStateId", id);
            }
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            controller.UpdateWeaponState();
            //TODO: Restringir el movimiento horizontal en el aire?
            controller.Move(inputController.horizontalInput);
            controller.Flip(inputController.aimDirection.x);

            if (inputController.jumpInput && controller.CanDoubleJump() && controller.DoubleJumpUnlocked){
                controller.CastJumpEffect();
                controller.ChangeState(new PlayerJumpingState());
            }

            if(inputController.dashInput && controller.canDash && controller.DashUnlocked) {
                controller.ChangeState(new PlayerDashingState());
            }

            if (controller.isGrounded){
                controller.ResetDash();
                controller.ChangeState(new PlayerIdleState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            //controller.animator.ResetTrigger("isRunning");
            if (controller.isGrounded) {
                controller.PlayLandSFX();
            }
        }
    }
}
