using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZombieSpawner : MonoBehaviour, IWaveBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnInterval = 3.2f;
    [SerializeField] private int totalZombiesPerWave = 20;

    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap spawnTilemap;

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

    private void StopSpawning()
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

    // Implementasi IWaveBehaviour
    public void StartWave(int waveNumber)
    {
        zombiesSpawnedThisWave = 0;
        zombiesAliveThisWave = 0;
        zombieLimit = Mathf.FloorToInt(totalZombiesPerWave * 1.2f * waveNumber);

        InvokeRepeating(nameof(SpawnZombie), 0f, spawnInterval);
    }

    public void StopWave()
    {
        StopSpawning();
    }

    public bool IsWaveComplete()
    {
        return AllZombiesDead();
    }

    // Handle saat zombie mati
    private void HandleZombieDied()
    {
        zombiesAliveThisWave--;
        Debug.Log($"Zombie mati. Sisa alive: {zombiesAliveThisWave}");
    }

    public bool AllZombiesDead()
        => zombiesSpawnedThisWave >= zombieLimit && zombiesAliveThisWave <= 0;

}