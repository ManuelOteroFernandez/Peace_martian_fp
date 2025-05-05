using Unity.VisualScripting;
using UnityEngine;

public class BubblePopEffectScript : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float lifeTime;

    [Header("SFX")]
    [SerializeField] AudioClip bubblePopSFX;

    private void Start() {
        AudioManager.Instance.PlaySFX(bubblePopSFX);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
