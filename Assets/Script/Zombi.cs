using UnityEngine;

public class Zombie : MonoBehaviour, IDamageAble
{
    [Header("Zombie Settings")]
    [SerializeField] private int health = 100;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackInterval = 3f; // Serangan per detik

    private float attackTimer;
    private IDamageAble currentTarget;
    private bool isAttacking;

    private void Update()
    {
        DetectPlant();

        if (isAttacking)
        {
            AttackTarget();
        }
        else
        {
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void DetectPlant()
    {

        // Offset raycast ke bawah sesuai posisi collider
        Vector3 rayOrigin = transform.position + new Vector3(0f, -0.5f, 0f); // sesuaikan nilai Y nya

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, 0.5f);
        Debug.DrawRay(rayOrigin, Vector2.left * 2f, Color.red); // debug visual

        if (hit.collider != null && hit.collider.CompareTag("Plant"))
        {
            currentTarget = hit.collider.GetComponent<IDamageAble>();
            isAttacking = currentTarget != null;
            Debug.Log($"{name} mendeteksi tanaman dan mulai menyerang!");
        }
        else
        {
            if (currentTarget == null || (currentTarget as MonoBehaviour) == null)
            {
                // Target benar-benar sudah mati/destroy
                isAttacking = false;
                attackTimer = 0f;
            }
        }
    }

    private void AttackTarget()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            currentTarget?.TakeDamage(damage);
            Debug.Log($"{name} menyerang target!");
            attackTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}