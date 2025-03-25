using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public bool hasArmor;
    public Vector2 lastCheckpoint;
    public List<string> activatedCheckpoints = new List<string>();
    public List<int> unlockedWeaponsId = new List<int>();
    public bool hasDoubleJump;
    public bool hasDash;
}
