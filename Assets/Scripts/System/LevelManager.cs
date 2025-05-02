
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager {

    public static void RestoreLastPlay() {
        SaveData saveData = SaveSystem.GetSaveData();
        if (saveData.sceneIndex > 0 && saveData.sceneIndex <= 3) {
            GameObject.FindFirstObjectByType<SceneFader>().FadeOutAndLoad(saveData.sceneIndex);
        }else{
            GameObject.FindFirstObjectByType<SceneFader>().FadeOutAndLoad(1);
        }
    }

    public static void NextLevel() {
        GameObject.FindFirstObjectByType<SceneFader>().FadeOutAndLoad(getNextLevelIndex());
    }

    public static int getNextLevelIndex()
    {
        int NextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (NextSceneIndex > 3)
        {
            return 0;
        }
        return NextSceneIndex;
    }

    public static void StartPlay() {
        SaveSystem.ClearData();
        RestoreLastPlay();
    }
}