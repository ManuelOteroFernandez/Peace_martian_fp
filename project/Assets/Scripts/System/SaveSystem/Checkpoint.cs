using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] string checkpointID;

    public string CheckpointID => checkpointID;

    void Start()
    {
        if (SaveSystem.IsCheckpointActive(checkpointID)) {
            GetComponent<SpriteRenderer>().color = new Color(0.52f,1.0f,0.63f,0.5f);
        }
    }

    void OnValidate() {
        if (Application.isPlaying) {
            ValidateUniqueID();
        }
    }

    private void ValidateUniqueID(){
        Checkpoint[] allCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        HashSet<string> idSet = new HashSet<string>();

        foreach (Checkpoint checkpoint in allCheckpoints) {
            if (checkpoint == this){
                continue;
            }

            if (idSet.Contains(checkpoint.checkpointID)) {
                Debug.LogError($"Checkpoint ID duplicado detectado: {checkpoint.checkpointID} en {checkpoint.gameObject.name}", checkpoint);
            }
            else {
                idSet.Add(checkpoint.checkpointID);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (!SaveSystem.IsCheckpointActive(checkpointID)) {
                PlayerController player = collision.GetComponent<PlayerController>();
                if (player != null) {
                    SaveSystem.SetCheckpoint(checkpointID, player);
                    GetComponent<SpriteRenderer>().color = new Color(0.52f,1.0f,0.63f,0.5f);
                    //Debug.Log("Checkpoint activado: " + checkpointID);
                    DisableCheckpoint();
                }
            } else {
                DisableCheckpoint();
            }
        }
    }

    private void DisableCheckpoint() {
        GetComponent<Collider2D>().enabled = false; 
    }
}
