using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int totalWaves = 10;
    [SerializeField] private int totalZombiesPerWave = 20;
    [SerializeField] private float delayBetweenWaves = 3f;

    private int currentWave = 0;
    private bool waveActive = false;
    private float waveEndTimer = 0f;
    private bool waitingDelay = false;

    private ZombieSpawner zombieSpawner;

    private void OnDisable()
    {

    }

    private void Start()
    {
        zombieSpawner = FindAnyObjectByType<ZombieSpawner>();
        if (zombieSpawner == null)
        {
            Debug.LogError("WaveManager: ZombieSpawner tidak ditemukan!");
            return;
        }

        zombieSpawner.OnZombieDied += CheckWaveCleared;

        StartNextWave();
    }

    private void Update()
    {
        // Delay antar wave
        if (!waitingDelay) return;

        waveEndTimer -= Time.deltaTime;
        if (waveEndTimer <= 0f)
        {
            waitingDelay = false;
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        if (currentWave >= totalWaves)
        {
            Debug.Log("Semua wave selesai!");
            GameManager.Instance.TriggerWin();
            return;
        }

        currentWave++;
        waveActive = true;

        Debug.Log($"Wave {currentWave} dimulai!");
        zombieSpawner.StartSpawning(totalZombiesPerWave);
    }

    private void CheckWaveCleared()
    {
        if (!waveActive) return;
        if (!zombieSpawner.AllZombiesDead()) return;

        waveActive = false;
        waitingDelay = true;
        waveEndTimer = delayBetweenWaves;

        Debug.Log($"Wave {currentWave} cleared! Next wave dalam {delayBetweenWaves}s");
    }

    private void OnDestroy()
    {
        if (zombieSpawner != null)
            zombieSpawner.OnZombieDied -= CheckWaveCleared;
    }
}