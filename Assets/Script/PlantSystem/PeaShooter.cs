using UnityEngine;

public class PeaShooter : PlantShooter
{
    [Header("PeaShooter Settings")]
    [SerializeField] private float projectileSpeed = 5f;

    protected override void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError($"{name}: projectilePrefab atau firePoint belum diassign!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * projectileSpeed;
        }
        else
        {
            Debug.LogError($"{name}: Projectile prefab tidak punya Rigidbody2D!");
        }
    }
}