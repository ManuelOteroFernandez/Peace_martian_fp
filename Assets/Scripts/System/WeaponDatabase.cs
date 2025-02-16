using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Game/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject
{
    public List<Weapon> unlockedWeapons = new List<Weapon>();
}
