using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerIdleState : PlayerState
    {
        public override int id {
            get {
                return (int)PlayerStateID.Idle;
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
            controller.Flip(inputController.aimDirection.x);

            if (inputController.horizontalInput != 0) {
                controller.ChangeState(new PlayerRunningState());
            }

            if(inputController.jumpInput) {
                controller.ChangeState(new PlayerJumpingState());
            }

            if(inputController.dashInput && controller.canDash && controller.DashUnlocked) {
                controller.ChangeState(new PlayerDashingState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
        }
    }
}
