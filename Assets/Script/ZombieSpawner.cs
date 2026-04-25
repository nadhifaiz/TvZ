using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap spawnTilemap;

    // Dipanggil WaveManager setiap ada zombie mati
    public event Action OnZombieDied;

    private List<Vector3> spawnPoints = new List<Vector3>();
    private int zombiesSpawnedThisWave = 0;
    private int zombiesAliveThisWave = 0;
    private int zombieLimit = 0;

    private void Start()
    {
        if (zombiePrefab == null || spawnTilemap == null)
        {
            Debug.LogError("ZombieSpawner: Ada field yang belum diassign di Inspector!");
            return;
        }

        CollectSpawnPoints();

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("ZombieSpawner: Tidak ada tile di spawnTilemap!");
        }
    }

    // ── Public API untuk WaveManager ───────────────────────
    public void StartSpawning(int limit)
    {
        zombiesSpawnedThisWave = 0;
        zombiesAliveThisWave = 0;
        zombieLimit = limit;

        InvokeRepeating(nameof(SpawnZombie), 0f, spawnInterval);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnZombie));
    }

    // ── Internal ────────────────────────────────────────────
    private void CollectSpawnPoints()
    {
        BoundsInt bounds = spawnTilemap.cellBounds;

        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            if (!spawnTilemap.HasTile(cellPos)) continue;

            Vector3 worldPos = spawnTilemap.GetCellCenterWorld(cellPos);
            worldPos.y += 0.5f;
            spawnPoints.Add(worldPos);
        }

        Debug.Log($"Total spawn points: {spawnPoints.Count}");
    }

    private void SpawnZombie()
    {
        // Sudah capai limit, stop spawn
        if (zombiesSpawnedThisWave >= zombieLimit)
        {
            StopSpawning();
            return;
        }

        Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        zombiesSpawnedThisWave++;
        zombiesAliveThisWave++;

        // Subscribe ke event mati zombie
        Zombie health = zombie.GetComponent<Zombie>();
        if (health != null)
            health.OnDied += HandleZombieDied;

        Debug.Log($"Zombie spawned ({zombiesSpawnedThisWave}/{zombieLimit})");
    }

    private void HandleZombieDied()
    {
        zombiesAliveThisWave--;
        OnZombieDied?.Invoke();

        Debug.Log($"Zombie mati. Sisa alive: {zombiesAliveThisWave}");
    }

    public bool AllZombiesDead()
        => zombiesSpawnedThisWave >= zombieLimit && zombiesAliveThisWave <= 0;
}