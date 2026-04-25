using UnityEngine;
using UnityEngine.InputSystem;

public class PlantManager : MonoBehaviour
{
    [Header("Plant Settings")]
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private GameObject plantPrefab2;
    [SerializeField] private GameObject plantPrefab3;

    private GridManager gridManager;
    private ResourceManager resourceManager;

    private void Awake()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        resourceManager = FindAnyObjectByType<ResourceManager>();
    }

    private int selectedPlantIndex = -1;
    private bool isPlantingMode = false;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && isPlantingMode)
        {
            // Bug 1 fix: konversi Screen Space → World Space
            Vector3 screenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(screenPos);
            mouseWorldPosition.z = 0;
            PlantSeed(mouseWorldPosition);
        }
    }

    public void SelectPlant(int plantIndex)
    {
        if (isPlantingMode && selectedPlantIndex == plantIndex)
        {
            ResetPlantingMode();
            Debug.Log("Exited planting mode.");
        }
        else
        {
            selectedPlantIndex = plantIndex;
            isPlantingMode = true;
            Debug.Log($"Selected Plant {selectedPlantIndex + 1} for planting.");
        }
    }

    public void PlantSeed(Vector3 mousePosition)
    {
        if (!gridManager.IsWithinPlantArea(mousePosition))
        {
            Debug.Log("Clicked outside of planting area. Please click within the designated area.");
            // Bug 3 fix: gak reset planting mode, biar user bisa klik tempat lain
            return;
        }
        if (gridManager.IsCellOccupied(mousePosition))
        {
            Debug.Log("Cell is already occupied. Cannot plant here.");
            // Bug 3 fix: gak reset planting mode, biar user bisa klik tempat lain
            return;
        }

        GameObject selectedPlant = null;

        switch (selectedPlantIndex)
        {
            case 0:
                selectedPlant = plantPrefab;
                break;
            case 1:
                selectedPlant = plantPrefab2;
                break;
            case 2:
                selectedPlant = plantPrefab3;
                break;
            default:
                Debug.LogError("Invalid plant index selected.");
                return;
        }

        Plant newPlant = selectedPlant.GetComponent<Plant>();

        if (newPlant == null || resourceManager == null)
        {
            Debug.LogError("Selected plant prefab does not have a Plant component or ResourceManager not found!");
            ResetPlantingMode();
            return;
        }

        if (!resourceManager.SpendSun(newPlant.Cost))
        {
            Debug.Log($"Not enough Sun to plant. Required: {newPlant.Cost}, Available: {resourceManager.GetCurrentSun()}");
            ResetPlantingMode();
            return;
        }

        Vector3 spawnPosition = gridManager.GetCellCenterWorld(mousePosition);
        Instantiate(selectedPlant, spawnPosition, Quaternion.identity);
        gridManager.SetCellOccupied(mousePosition, true);
        Debug.Log($"Planted Plant {selectedPlantIndex + 1} at {spawnPosition}");
        ResetPlantingMode();
    }

    private void ResetPlantingMode()
    {
        isPlantingMode = false;
        selectedPlantIndex = -1;
    }
}