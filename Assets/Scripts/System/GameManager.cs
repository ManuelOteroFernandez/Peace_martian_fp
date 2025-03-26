using UnityEngine;

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
    }

    public void RespawnPlayer() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            SaveData saveData = SaveSystem.GetSaveData();
            player.transform.position = saveData.lastCheckpoint;
            if (saveData.hasArmor) {
                player.GetComponent<PlayerController>().AddArmor();
            }

            if (saveData.hasDoubleJump) {
                player.GetComponent<PlayerController>().UnlockDoubleJump();
            }

            if (saveData.hasDash) {
                player.GetComponent<PlayerController>().UnlockDash();
            }
        }
    }

    public void LoadUnlockedWeapons() {
        SaveData saveData = SaveSystem.GetSaveData();
        foreach (int weaponID in saveData.unlockedWeaponsId) {
            weaponDatabase.LoadWeapon(weaponID);
        }
    }
}
