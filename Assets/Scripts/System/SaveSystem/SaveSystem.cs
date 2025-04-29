using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

//TODO: Refactorizar todo el sistema de checkpoints y guardado para abstraerlo y hacerlo más escalable
public static class SaveSystem {
    private static string path = Application.persistentDataPath + "/savefile.json";
    private static SaveData saveData;

    public static SaveData GetSaveData() {
        if (saveData == null) {
            LoadGame();
        }

        return saveData;
    }

    public static void SaveGame() {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public static void LoadGame() {
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else {
            saveData = new SaveData();
            SaveGame();
        }
    }

    // Setear nuevo checkpoint y estado de hasArmor
    public static void SetCheckpoint(string checkpointID, PlayerController player) {
        saveData.lastCheckpointID = checkpointID;
        saveData.hasArmor = player.HasArmor;
        saveData.hasDoubleJump = player.DoubleJumpUnlocked;
        saveData.hasDash = player.DashUnlocked;
        saveData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (!saveData.activatedCheckpoints.Contains(checkpointID)) {
            saveData.activatedCheckpoints.Add(checkpointID);
        }

        SaveGame();
    }

    public static void SetSceneIndex(int Index)
    {        
        if (0 < Index &&  Index < SceneManager.sceneCountInBuildSettings){
            saveData = GetSaveData();
            saveData.sceneIndex = Index;
            SaveGame();
        }
    }

    public static void SetUnlockedWeapon(int weaponID) {
        if (!saveData.unlockedWeaponsId.Contains(weaponID)) {
            saveData.unlockedWeaponsId.Add(weaponID);
        }
    }

    public static bool IsCheckpointActive(string checkpointID) {
        return saveData.activatedCheckpoints.Contains(checkpointID);
    }

    public static void ClearData() {
        saveData = new SaveData();
        SaveGame();
    }
}
