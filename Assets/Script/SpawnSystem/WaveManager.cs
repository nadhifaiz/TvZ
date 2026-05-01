using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float delayBetweenWaves = 3f;

    [Header("Daftarkan Spawner di Sini")]
    [SerializeField] private List<MonoBehaviour> spawnerObjects;

    private int currentWave = 0;
    private bool waveActive = false;
    private float waveEndTimer = 0f;
    private bool waitingDelay = false;
    private List<IWaveBehaviour> spawners = new List<IWaveBehaviour>();

    private void Awake()
    {
        foreach (var obj in spawnerObjects)
        {
            if (obj is IWaveBehaviour spawner)
                spawners.Add(spawner);
        }
    }

    private void Start()
    {
        if (spawners.Count == 0)
        {
            Debug.LogError("WaveManager: Tidak ada IWaveBehaviour ditemukan!");
            return;
        }

        StartNextWave();
    }

    private void Update()
    {
        if (waveActive && spawners.All(s => s.IsWaveComplete()))
        {
            waveActive = false;
            waitingDelay = true;
            waveEndTimer = delayBetweenWaves;
            Debug.Log($"Wave {currentWave} cleared! Next wave dalam {delayBetweenWaves}s");
        }

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
        foreach (var spawner in spawners)
            spawner.StartWave(currentWave);
    }
}