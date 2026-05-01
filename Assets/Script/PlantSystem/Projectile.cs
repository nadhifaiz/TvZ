using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float maxDistance = 20f;

    private Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(spawnPosition, transform.position) >= maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Plant"))
            return;

        IDamageAble target = collision.GetComponent<IDamageAble>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log($"{name} hit {collision.name} for {damage} damage!");
            Destroy(gameObject);
        }
    }
}