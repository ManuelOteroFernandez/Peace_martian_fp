using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float maxBubblesResists = 4;
    [SerializeField] GameObject smallBubblePrefab;
    [SerializeField] GameObject bigBubblePrefab;

    List<GameObject> bubbles = new List<GameObject>();
    GameObject bigBubble = null;

    float currentBubbleResist;

    void Start() {
        currentBubbleResist = maxBubblesResists;
    }

    public void TakeBubbleDamage(float bubbleDamage){
        if (currentBubbleResist <= 0) {
            return;
        }

        if (bubbleDamage >= currentBubbleResist) {
            currentBubbleResist = 0;
            BigBubbleSpawn();
        } else {
            currentBubbleResist -= bubbleDamage;

            GameObject bubble = Instantiate(smallBubblePrefab, RandomBubblePosition(), Quaternion.identity);
            bubble.transform.SetParent(transform);
            bubbles.Add(bubble);
        }
    }

    void BigBubbleSpawn() {
        bigBubble = Instantiate(bigBubblePrefab, transform.position, Quaternion.identity);
        bigBubble.GetComponent<BigBubbleTrap>().Initialize(gameObject);

        bubbles.ForEach(bubble => Destroy(bubble));
        bubbles.Clear();
    }

    public void ReleaseFromBubble() {
        currentBubbleResist = maxBubblesResists;
        bigBubble = null;
    }

    public bool IsInBubble() {
        return bigBubble != null && currentBubbleResist <= 0;
    }

    Vector2 RandomBubblePosition() {
        float offsetX = Random.Range(-0.5f, 0.5f);
        float offsetY = Random.Range(-0.5f, 0.5f);

        return transform.position + new Vector3(offsetX, offsetY, 0);
    }
}
