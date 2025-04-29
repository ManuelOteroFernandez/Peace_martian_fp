using UnityEngine;


public class EndLevelFlag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            SaveSystem.SetSceneIndex(LevelManager.getNextLevelIndex());
            LevelManager.NextLevel();
        }
    }
}
