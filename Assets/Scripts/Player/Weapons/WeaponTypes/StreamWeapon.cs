using UnityEngine;

public abstract class StreamWeapon : Weapon
{
    protected bool isShooting = false;

    public override void StartShooting() {
        isShooting = true;
        OnStartShooting();
    }
    public override void StopShooting(){
        isShooting = false;
        OnStopShooting();
    }

    protected abstract void OnStartShooting();
    protected abstract void OnStopShooting();
}
