using UnityEngine;

public class MelonPult : PlantShooter
{
    [Header("MelonPult Settings")]
    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float targetRange = 8f;

    protected override void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError($"{name}: projectilePrefab atau firePoint belum diassign!");
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError($"{name}: Projectile prefab tidak punya Rigidbody2D!");
            return;
        }

        Vector2 startPos = firePoint.position;
        Vector2 targetPos = new Vector2(startPos.x + targetRange, startPos.y);

        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float Vy = Mathf.Sqrt(2f * gravity * arcHeight);
        float totalTime = 2f * Vy / gravity;
        float Vx = (targetPos.x - startPos.x) / totalTime;

        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(Vx, Vy);
    }
}