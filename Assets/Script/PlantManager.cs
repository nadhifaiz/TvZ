using UnityEngine;

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
        if (Input.GetMouseButtonDown(0) && isPlantingMode)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        if (gridManager.IsCellOccupied(mousePosition))
        {
            Debug.Log("Cell is already occupied. Cannot plant here.");
            isPlantingMode = false;
            selectedPlantIndex = -1;
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
    }
}
