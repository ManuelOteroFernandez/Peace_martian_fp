using UnityEngine;

public class BubblePopEffectScript : MonoBehaviour
{
    [SerializeField] float lifeTime;

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
