using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Game/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject {
    public List<Weapon> weapons = new List<Weapon>();
    public List<Weapon> unlockedWeapons = new List<Weapon>();

    public Weapon GetWeaponByID(int weaponID){
        return weapons.Find(w => w.WeaponId == weaponID);
    }

    public void LoadWeapon(int weaponID){
        Weapon weapon = GetWeaponByID(weaponID);
        if (weapon != null && !unlockedWeapons.Contains(weapon)){
            unlockedWeapons.Add(weapon);
        }
    }
}