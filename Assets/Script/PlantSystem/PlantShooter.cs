using UnityEngine;

public abstract class PlantShooter : Plant
{
    [Header("Shooter Settings")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float shootInterval = 2f;

    protected float shootTimer;

    protected virtual void Start()
    {
        shootTimer = 0f;
    }

    protected virtual void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval && ShouldShoot())
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    protected virtual bool ShouldShoot()
    {
        return true;
    }

    // Abstract — wajib diisi subclass, tidak ada default behavior
    protected abstract void Shoot();
}