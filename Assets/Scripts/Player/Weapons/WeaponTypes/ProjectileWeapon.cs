using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float fireCooldown;
    [SerializeField] AudioClip shootSFX;
    float lastFireTime;

    public override void StartShooting() {
        if (Time.time - lastFireTime > fireCooldown){
            Shoot();
            lastFireTime = Time.time;
        }
    }

    public override void StopShooting(){}

    public virtual void Shoot(){
        AudioManager.Instance.PlaySFX(shootSFX);
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
