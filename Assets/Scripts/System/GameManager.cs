using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public WeaponDatabase weaponDatabase;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        RespawnPlayer();
        LoadUnlockedWeapons();
        PlayMusic();
    }

    public void RespawnPlayer() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            SaveData saveData = SaveSystem.GetSaveData();
            //Calculo de la mitad de la altura del sprite del jugador para que no se quede bajo tierra
            Vector2 yOffset = player.GetComponent<SpriteRenderer>().bounds.extents.y / 2 * Vector2.up;

            if (saveData.lastCheckpointID != null) {
                List<Checkpoint> checkpointList = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();

                Checkpoint lastActiveCheckpoint = checkpointList.Find(checkpoint => checkpoint.CheckpointID == saveData.lastCheckpointID);
                if (lastActiveCheckpoint != null) {
                    player.transform.position = (Vector2) lastActiveCheckpoint.transform.position + yOffset;
                } else {
                    player.transform.position = Vector2.zero + yOffset;
                }
            } else {
                player.transform.position = Vector2.zero + yOffset;
            }

            if (saveData.hasArmor) {
                player.GetComponent<PlayerController>().AddArmor();
            }

            if (saveData.hasDoubleJump) {
                player.GetComponent<PlayerController>().UnlockDoubleJump();
            }

            if (saveData.hasDash) {
                player.GetComponent<PlayerController>().UnlockDash();
            }

            RemovePreviousEnemies(player.transform);
        }
    }

    public void LoadUnlockedWeapons() {
        SaveData saveData = SaveSystem.GetSaveData();
        foreach (int weaponID in saveData.unlockedWeaponsId) {
            weaponDatabase.LoadWeapon(weaponID);
        }
    }

    public void RemovePreviousEnemies(Transform playerTransform) {
        List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        foreach (GameObject enemy in enemies) {
            if (enemy.transform.position.x < playerTransform.position.x) {
                Destroy(enemy);
            }
        }
    }

    void PlayMusic() {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case -1:
                AudioManager.Instance.PlayMusicMainMenu();
                break;
            case 0:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.level1Theme);
                break;
            case 2:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.level2Theme);
                break;
            case 3:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.level3Theme);
                break;
        }
    }
}
