using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    SceneFader sceneFader;
    LogicaEntreEscenas entreEscenas;

    public WeaponDatabase weaponDatabase;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else {
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        PlayMusic();
    }

    void Start() {
        sceneFader = FindFirstObjectByType<SceneFader>();
        entreEscenas = FindFirstObjectByType<LogicaEntreEscenas>();
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
                }
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

    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadScene(int sceneIndex) {
        sceneFader.FadeOutAndLoad(sceneIndex);
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
            case 0:
                AudioManager.Instance.PlayMusic(AudioManager.Instance.menuTheme2);
                break;
            case 1:
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

    public void Win()
    {
        SaveSystem.SetSceneIndex(LevelManager.getNextLevelIndex());
        if (entreEscenas == null) {
            LevelManager.NextLevel();
        }else{
            entreEscenas.SetActiveVictoryMenu(true);
        }
    }

    public void Defeat()
    {
        if (entreEscenas == null)
        {            
            ReloadScene();
            RespawnPlayer();
        }else
        {
            entreEscenas.SetActiveDefeatMenu(true);
        }
    }
}
