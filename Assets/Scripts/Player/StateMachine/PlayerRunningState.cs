using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerRunningState : PlayerState
    {
        public override void Enter(PlayerController controller)
        {
            controller.animator.SetTrigger("isRunning");
        }

        public override void Update(PlayerController controller, PlayerInputController inputController)
        {
            controller.Move(inputController.horizontalInput);

            if (inputController.horizontalInput == 0)
            {
                controller.ChangeState(new PlayerIdleState());
            }

            if (inputController.jumpInput)
            {
                Debug.Log("Is jumping: " + inputController.jumpInput);
                controller.Jump();
                controller.ChangeState(new PlayerJumpingState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller)
        {
            controller.animator.ResetTrigger("isRunning");
        }
    }
}
