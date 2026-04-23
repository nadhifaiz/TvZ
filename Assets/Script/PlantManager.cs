using UnityEngine;
using UnityEngine.InputSystem;

public class PlantManager : MonoBehaviour
{
    [Header("Plant Settings")]
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private GameObject plantPrefab2;
    [SerializeField] private GridManager gridManager;

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
            isPlantingMode = false;
            selectedPlantIndex = -1;
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

        Vector3 spawnPosition = gridManager.GetCellCenterWorld(mousePosition);
        switch (selectedPlantIndex)
        {
            case 0:
                Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
                break;
            case 1:
                Instantiate(plantPrefab2, spawnPosition, Quaternion.identity);
                break;
            default:
                Debug.LogError("Invalid plant index selected.");
                return;
        }

        gridManager.SetCellOccupied(mousePosition, true);
        Debug.Log($"Planted Plant {selectedPlantIndex + 1} at {spawnPosition}");

        // Bug 2 fix: reset planting mode setelah berhasil nanam
        isPlantingMode = false;
        selectedPlantIndex = -1;
    }
}