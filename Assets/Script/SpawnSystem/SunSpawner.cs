using UnityEngine;

public class SunSpawner : MonoBehaviour, IWaveBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int sunPerWave = 20;

    [Header("Spawn Area Settings")]
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;

    private int spawnedCount = 0;
    private int spawnLimit = 0;

    private void SpawnObject()
    {
        if (AllObjectsSpawned())
        {
            CancelInvoke(nameof(SpawnObject));
            return;
        }

        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        spawnedCount++;
    }

    public void StartWave(int waveNumber)
    {
        spawnedCount = 0;
        spawnLimit = Mathf.RoundToInt(sunPerWave - 0.4f * waveNumber);
        InvokeRepeating(nameof(SpawnObject), spawnInterval, spawnInterval);
    }

    public void StopWave()
    {
        CancelInvoke(nameof(SpawnObject));
    }

    public bool IsWaveComplete()
    {
        return AllObjectsSpawned();
    }

    private bool AllObjectsSpawned()
    {
        return spawnedCount >= spawnLimit;
    }
}