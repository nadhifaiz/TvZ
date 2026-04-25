using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Tilemap Settings")]
    [SerializeField] private Tilemap spawnTilemap; // Tilemap khusus zombie spawn
    [SerializeField] private float zombieHeight = 1f;

    private List<Vector3> spawnPoints = new List<Vector3>();

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
            return;
        }

        InvokeRepeating(nameof(SpawnZombie), spawnInterval, spawnInterval);
    }

    private void CollectSpawnPoints()
    {
        // Scan semua cell dalam bounds tilemap
        BoundsInt bounds = spawnTilemap.cellBounds;

        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            if (!spawnTilemap.HasTile(cellPos)) continue;

            // Ambil world position center cell
            Vector3 worldPos = spawnTilemap.GetCellCenterWorld(cellPos);

            // Offset Y ke atas sebesar setengah tinggi zombie
            worldPos.y += zombieHeight / 2f;

            spawnPoints.Add(worldPos);
            Debug.Log($"Spawn point registered at: {worldPos}");
        }

        Debug.Log($"Total spawn points: {spawnPoints.Count}");
    }

    private void SpawnZombie()
    {
        // Random pilih salah satu spawn point
        Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Count)];

        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Zombie spawned at: {spawnPosition}");
    }
}