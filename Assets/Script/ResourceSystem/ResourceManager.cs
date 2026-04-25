using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private int baseSun = 100;
    [SerializeField] private int currentSun;

    private void Start()
    {
        currentSun = baseSun;
        Debug.Log($"Starting Sun: {currentSun}");
    }

    public bool SpendSun(int amount)
    {
        if (currentSun >= amount)
        {
            currentSun -= amount;
            Debug.Log($"Spent {amount} Sun. Remaining Sun: {currentSun}");
            return true;
        }
        Debug.Log($"Not enough Sun to spend. Required: {amount}, Available: {currentSun}");
        return false;
    }

    public void AddSun(int amount)
    {
        currentSun += amount;
        Debug.Log($"Added {amount} Sun. Current Sun: {currentSun}");
    }

    public int GetCurrentSun()
    {
        return currentSun;
    }

}
