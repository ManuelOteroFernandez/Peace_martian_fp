using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerDashingState : PlayerState
    {
        float dashTime;
        public override int id {
            get {
                return (int)PlayerStateID.Dashing;
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
            controller.PlayDashSFX();
            controller.Dash();
        }

        public override void Update(PlayerController controller, PlayerInputController inputController) {
            dashTime += Time.deltaTime;

            if (dashTime >= controller.DashDuration) {
                if (inputController.horizontalInput != 0) {
                    controller.ChangeState(new PlayerRunningState());
                } else {
                    controller.ChangeState(new PlayerIdleState());
                }
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller) {
            controller.EndDash();
        }
    }
}