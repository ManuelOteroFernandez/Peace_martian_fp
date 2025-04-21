using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxSpeed;
    Material mat;
    Transform cameraTransform;
    Vector3 initialPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        cameraTransform = Camera.main.transform;
        initialPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cameraTransform.position.x, initialPos.y, initialPos.z);
        mat.mainTextureOffset = new Vector2(cameraTransform.position.x * parallaxSpeed, 0);
    }
}
