using UnityEngine;

public class Sun : MonoBehaviour
{
    [Header("Sun Physics Settings")]
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float maxFallSpeed = 2f;
    [SerializeField] private float gravity = 9.8f;

    [Header("Wind Settings")]
    [SerializeField] private float windStrength = 0.5f;      // Amplitudo angin kiri-kanan
    [SerializeField] private float windFrequency = 1.2f;     // Seberapa cepat angin berubah arah
    [SerializeField] private float windTurbulence = 0.3f;    // Noise tambahan biar lebih organik

    [Header("Fall Target Settings")]
    [SerializeField] private float maxY = -4f;
    [SerializeField] private float minY = -6f;

    [Header("Floating Settings")]
    [SerializeField] private float floatAmplitude = 0.3f;    // Seberapa jauh naik-turun
    [SerializeField] private float floatFrequency = 1f;      // Kecepatan naik-turun

    [Header("Sun Settings")]
    [SerializeField] private int sunValue = 25;
    [SerializeField] private float lifetime = 10f;

    private Vector3 spawnPosition;       // Posisi awal spawn (referensi X)
    private Vector3 floatAnchor;         // Posisi anchor saat mulai floating
    private float currentFallSpeed;
    private float fallTargetY;           // Y tujuan yang di-random
    private float windPhaseOffset;       // Biar tiap Sun punya angin beda

    private State currentState = State.Falling;

    private void Start()
    {
        spawnPosition = transform.position;
        currentFallSpeed = fallSpeed;

        // Random target Y antara minY dan maxY
        fallTargetY = Random.Range(minY, maxY);

        // Random phase offset biar angin tiap sun tidak sinkron
        windPhaseOffset = Random.Range(0f, Mathf.PI * 2f);

        // Hancurkan sun setelah lifetime habis
        Invoke(nameof(DestroySun), lifetime);
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Falling:
                UpdateFalling();
                break;

            case State.Floating:
                UpdateFloating();
                break;
        }
    }

    private void UpdateFalling()
    {
        // === GRAVITY ===
        currentFallSpeed += gravity * Time.deltaTime;
        currentFallSpeed = Mathf.Min(currentFallSpeed, maxFallSpeed);

        float newY = transform.position.y - currentFallSpeed * Time.deltaTime;

        // === WIND PHYSICS (kiri-kanan) ===
        // Kombinasi sin + perlin noise biar gerakannya lebih natural
        float windSin = Mathf.Sin(Time.time * windFrequency + windPhaseOffset);
        float windNoise = (Mathf.PerlinNoise(Time.time * windTurbulence + windPhaseOffset, 0f) - 0.5f) * 2f;
        float windOffset = (windSin * 0.7f + windNoise * 0.3f) * windStrength;

        float newX = spawnPosition.x + windOffset;

        transform.position = new Vector3(newX, newY, spawnPosition.z);

        // === CEK SUDAH SAMPAI TARGET ===
        if (transform.position.y <= fallTargetY)
        {
            // Snap ke target Y agar tidak overshoot
            transform.position = new Vector3(transform.position.x, fallTargetY, spawnPosition.z);
            floatAnchor = transform.position;
            currentState = State.Floating;
        }
    }

    private void UpdateFloating()
    {
        // === FLOATING (atas-bawah sinusoidal) ===
        float floatY = floatAnchor.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // X tetap di posisi saat landing (tidak bergerak kiri-kanan saat floating)
        transform.position = new Vector3(
            floatAnchor.x,
            floatY,
            floatAnchor.z
        );
    }

    private void DestroySun()
    {
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        ResourceManager resourceManager = FindAnyObjectByType<ResourceManager>();
        if (resourceManager != null)
        {
            resourceManager.AddSun(sunValue);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("ResourceManager not found in the scene!");
        }
    }
}

public enum State
{
    Falling,
    Floating
}