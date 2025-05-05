using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Transform weaponHolder;
    [SerializeField] WeaponDatabase weaponDatabase;
    [SerializeField] AudioClip changeWeaponSFX;
    Weapon currentWeapon;

    List<Weapon> availableWeapons;
    int currentWeaponIndex = 0;

    Animator animator;

    private void Start(){
        animator = GetComponent<Animator>();

        availableWeapons = new List<Weapon>(weaponDatabase.unlockedWeapons);

        if (availableWeapons.Count > 0){
            EquipWeapon(0);
        }
    }

    public void ChangeWeapon(bool next){
        if (weaponDatabase.unlockedWeapons.Count <= 1){
            return;
        }

        currentWeaponIndex = next ? 
            (currentWeaponIndex + 1) % weaponDatabase.unlockedWeapons.Count : 
            (currentWeaponIndex - 1 + weaponDatabase.unlockedWeapons.Count) % weaponDatabase.unlockedWeapons.Count;

        EquipWeapon(currentWeaponIndex);
    }

    private void EquipWeapon(int index){
        if (currentWeapon != null) {
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = Instantiate(weaponDatabase.unlockedWeapons[index], weaponHolder.position, Quaternion.identity, weaponHolder);

        animator.SetFloat("currentWeaponIndex",currentWeaponIndex);
    }

    public void UnlockWeapon(Weapon newWeapon){
        if (!weaponDatabase.unlockedWeapons.Contains(newWeapon)){
            weaponDatabase.unlockedWeapons.Add(newWeapon);
        }
    }
    public void StartShooting(){
        if (currentWeapon != null){
            currentWeapon.StartShooting();
        }
    }

    public void StopShooting(){
        if (currentWeapon != null){
            currentWeapon.StopShooting();
        }
    }

    public void SwitchWeapon(bool next){
        if (weaponDatabase.unlockedWeapons.Count <= 1){
            return;
        }

        if (next){
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponDatabase.unlockedWeapons.Count;
        } else {
            currentWeaponIndex = (currentWeaponIndex - 1 + weaponDatabase.unlockedWeapons.Count) % weaponDatabase.unlockedWeapons.Count;
        }
        AudioManager.Instance.PlaySFX(changeWeaponSFX, true);

        EquipWeapon(currentWeaponIndex);
    }

    public void ManageShooting(PlayerInputController inputController){
        if (currentWeapon is ProjectileWeapon && inputController.shootInput){
            StartShooting();
            inputController.ResetInput();
        }
        if (currentWeapon is StreamWeapon && inputController.shootHoldInput){
            StartShooting();
        }
        if (currentWeapon is StreamWeapon && !inputController.shootHoldInput){
            StopShooting();
        }
    }

    public Weapon GetCurrentWeapon(){
        return currentWeapon;
    }
}
