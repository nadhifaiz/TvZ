using UnityEngine;

public abstract class Plant : MonoBehaviour, IDamageAble
{
    [Header("Plant Settings")]
    [SerializeField] protected int cost = 50;
    [SerializeField] protected int health = 100;

    protected GridManager gridManager;

    public int Cost => cost;

    protected virtual void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    public void Die()
    {
        gridManager.SetCellOccupied(transform.position, false);
        Destroy(gameObject);
    }
}