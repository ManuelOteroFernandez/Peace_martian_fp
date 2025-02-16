using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;
    protected float damage;
    protected float cooldown;
    protected PlayerInputController playerInputController;

    protected virtual void Awake() {
        playerInputController = GetComponentInParent<PlayerInputController>();
    }

    protected virtual void Update() {
        float angle = Mathf.Atan2(playerInputController.aimDirection.y, playerInputController.aimDirection.x) * Mathf.Rad2Deg;

        float adjustedAngle = Mathf.Round(angle / 45) * 45;

        //Si el jugador está mirando a la izquierda, se invierte el sprite
        if (transform.root.localScale.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        } else {
            transform.localScale = new Vector3(1, 1, 1);
        }

        this.transform.rotation = Quaternion.Euler(0, 0, adjustedAngle);
    }

    //public virtual void Shoot() {}
    public abstract void StartShooting();
    public abstract void StopShooting();
}
