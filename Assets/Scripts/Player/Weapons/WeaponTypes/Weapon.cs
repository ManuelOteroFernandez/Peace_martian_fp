using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;
    protected float damage;
    protected float cooldown;
    protected float adjustedAngle;
    protected PlayerInputController playerInputController;

    [Header("Weapon ID")]
    [SerializeField ]protected int weaponId;

    [Header("References")]
    public int WeaponId => weaponId;

    protected virtual void Awake() {
        playerInputController = GetComponentInParent<PlayerInputController>();
    }

    protected virtual void Update() {
        float angle = Mathf.Atan2(playerInputController.aimDirection.y, playerInputController.aimDirection.x) * Mathf.Rad2Deg;

        adjustedAngle = Mathf.Round(angle / 45) * 45;
        Debug.Log(adjustedAngle);
        switch (adjustedAngle)
        {
            case 90:
                transform.localPosition = new Vector3(0.43f,0.5f,0);
                transform.rotation = Quaternion.Euler(0,0,90);
                // firePoint.rotation = Quaternion.Euler(0,firePoint.rotation.y,firePoint.rotation.z);
                break;

            case 135:
                transform.localPosition = new Vector3(0.8f,-0.1f,0);
                transform.rotation = Quaternion.Euler(0, 0, 135);
                // firePoint.rotation = Quaternion.Euler(0,firePoint.rotation.y,firePoint.rotation.z);
                break;
            case 45: 
                transform.localPosition = new Vector3(0.8f,-0.1f,0);
                transform.rotation = Quaternion.Euler(0, 0, 45);
                // firePoint.rotation = Quaternion.Euler(0,firePoint.rotation.y,firePoint.rotation.z);
                break;

            case -45:
                transform.localPosition = new Vector3(0.7f,-0.7f,0);
                transform.rotation = Quaternion.Euler(0, 0, -45);
                // firePoint.rotation = Quaternion.Euler(180,firePoint.rotation.y,firePoint.rotation.z);
                break;
            
            case -135:
                transform.localPosition = new Vector3(0.7f,-0.7f,0);
                transform.rotation = Quaternion.Euler(0, 0, -135);
                // firePoint.rotation = Quaternion.Euler(180,firePoint.rotation.y,firePoint.rotation.z);
                break;
            
            case -90:
                transform.localPosition = new Vector3(0.2f,-0.8f,0);
                transform.rotation = Quaternion.Euler(0, 0, -90);
                // firePoint.rotation = Quaternion.Euler(0,firePoint.rotation.y,firePoint.rotation.z);
                break;

            case -180:
            case 180:
                transform.localPosition = new Vector3(0.8f,-0.5f,0);
                transform.rotation = Quaternion.Euler(0, 0, 180);
                // firePoint.rotation = Quaternion.Euler(180,firePoint.rotation.y,firePoint.rotation.z);
                break;


            default: 
                transform.localPosition = new Vector3(0.8f,-0.5f,0);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                // firePoint.rotation = Quaternion.Euler(0,firePoint.rotation.y,firePoint.rotation.z);
                break;
        }

        // //Si el jugador está mirando a la izquierda, se invierte el sprite
        // if (transform.root.localScale.x < 0) {
        //     transform.localScale = new Vector3(-1, 1, 1);
        // } else {
        //     transform.localScale = new Vector3(1, 1, 1);
        // }
    }

    //public virtual void Shoot() {}
    public abstract void StartShooting();
    public abstract void StopShooting();

    public float getAdjustedAngle(){
        return adjustedAngle;
    }
}
