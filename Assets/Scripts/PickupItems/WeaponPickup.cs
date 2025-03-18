using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] float rotationSpeed;

    private void Update() {
        float t = (Mathf.Sin(Time.time * rotationSpeed) + 1) / 2; // Convierte Sin de -1 a 1 en 0 a 1

        transform.localScale = new Vector3(Mathf.Lerp(-1, 1, t), transform.localScale.y, transform.localScale.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponentInChildren<WeaponManager>().AddWeapon(weapon);
            other.GetComponentInChildren<WeaponManager>().UnlockWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
