using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public bool hasArmor;
    public List<string> activatedCheckpoints = new List<string>();
    public string lastCheckpointID;
    public List<int> unlockedWeaponsId = new List<int>();
    public int sceneIndex;
    public bool hasDoubleJump;
    public bool hasDash;
}
