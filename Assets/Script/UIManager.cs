using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("Game UI")]
    [SerializeField] private TextMeshProUGUI sunText;
    [SerializeField] private GameObject plantingModeIndicator;

    private ResourceManager resourceManager;

    private void Start()
    {
        resourceManager = FindAnyObjectByType<ResourceManager>();
        if (resourceManager != null && sunText != null)
        {
            sunText.text = $"Sun: {resourceManager.GetCurrentSun()}";
        }

        if (plantingModeIndicator != null)
        {
            plantingModeIndicator.SetActive(false);
        }

        // Pastikan semua panel tertutup saat mulai
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnPauseGame();
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        ResourceManager.OnSunChanged += HandleSunChanged;
        PlantManager.OnPlantingModeChanged += HandlePlantingModeChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        ResourceManager.OnSunChanged -= HandleSunChanged;
        PlantManager.OnPlantingModeChanged -= HandlePlantingModeChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        // Tutup semua panel dulu
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        switch (state)
        {
            case GameState.Paused:
                pausePanel.SetActive(true);
                break;
            case GameState.Win:
                winPanel.SetActive(true);
                break;
            case GameState.Lose:
                losePanel.SetActive(true);
                break;

        }
    }

    private void HandleSunChanged(int currentSun)
    {
        if (sunText != null)
        {
            sunText.text = $"Sun: {currentSun}";
        }
    }

    private void HandlePlantingModeChanged(bool isPlanting, int plantIndex)
    {
        if (plantingModeIndicator != null)
        {
            plantingModeIndicator.SetActive(isPlanting);
        }
    }

    // Handle Button Menu
    public void OnResumeButton() => GameManager.Instance.ResumeGame();
    public void OnPauseGame() => GameManager.Instance.PauseGame();
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void OnQuitButton() => Application.Quit();
}
