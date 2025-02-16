using UnityEngine;

public class PlayerState
{
    public virtual void Enter(PlayerController controller){}
    public virtual void Update(PlayerController controller, PlayerInputController inputController){}
    public virtual void Exit(PlayerController controller){}

    protected void UpdateWeaponState(PlayerController controller, PlayerInputController inputController) {
        if(inputController.nextWeaponInput) {
            controller.weaponManager.SwitchWeapon(true);
            inputController.ResetInput();
        } else if (inputController.previousWeaponInput) {
            controller.weaponManager.SwitchWeapon(false);
            inputController.ResetInput();
        }

        controller.weaponManager.ManageShooting(inputController);
    }
}
