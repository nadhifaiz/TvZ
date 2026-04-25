using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Spawn Area Settings")]
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }

    private void SpawnObject()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
    }
}