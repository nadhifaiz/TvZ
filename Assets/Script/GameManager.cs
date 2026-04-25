using UnityEngine;
using System;

public enum GameState { Playing, Paused, Win, Lose }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; } = GameState.Playing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;

        OnGameStateChanged?.Invoke(newState);
        Debug.Log($"GameState: {newState}");
    }

    // ── Shortcut methods ────────────────────────────────────
    public void PauseGame() => SetState(GameState.Paused);
    public void ResumeGame() => SetState(GameState.Playing);
    public void TriggerWin() => SetState(GameState.Win);
    public void TriggerLose() => SetState(GameState.Lose);
}