using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerDashingState : PlayerState
    {
        float dashTime;
        public override int id {
            get {
                return 4;
            }
        }

        public override void Enter(PlayerController controller) {
            controller.animator.SetInteger("currentStateId", id);
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
            controller.animator.ResetTrigger("isDashing");
        }
    }
}