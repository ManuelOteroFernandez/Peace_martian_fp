using UnityEngine;

namespace PlayerStateMachine{
    public class PlayerJumpingState : PlayerState
    {
        public override void Enter(PlayerController controller)
        {
            SetAnimation(controller);
        }

        public override void Update(PlayerController controller, PlayerInputController inputController)
        {
            SetAnimation(controller);

            controller.Move(inputController.horizontalInput);

            if (inputController.jumpInput)
            {
                controller.Jump();
            }

            if (controller.isGrounded)
            {
                controller.ChangeState(new PlayerIdleState());
            }

            inputController.ResetInput();
        }

        public override void Exit(PlayerController controller)
        {
            SetAnimation(controller);
        }

        void SetAnimation(PlayerController controller) {
            // if (controller.rigidbody2D.linearVelocityY > 0)
            // {
            //     controller.animator.SetTrigger("isJumping");
            // }
            // else if (controller.rigidbody2D.linearVelocityY < 0)
            // {
            //     controller.animator.SetTrigger("isFalling");
            // }
            // else
            // {
            //     controller.animator.SetTrigger("isJumping");
            //     controller.animator.SetTrigger("isFalling");
            // }
        }
    }
}
