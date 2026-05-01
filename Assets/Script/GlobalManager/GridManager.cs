using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Tilemap worldTilemap;

    [Header("Planting Area Settings")]
    [SerializeField] private Vector2Int plantAreaMin;
    [SerializeField] private Vector2Int plantAreaMax;

    private CellType[][] worldGrid;
    private int gridWidth, gridHeight;

    private void Start()
    {
        // Ukuran array otomatis dari plantArea, tidak perlu input manual
        gridWidth = plantAreaMax.x - plantAreaMin.x;
        gridHeight = plantAreaMax.y - plantAreaMin.y;
        Debug.Log($"Grid Size: {gridWidth} x {gridHeight}");
        Debug.Log($"Plant Area: {plantAreaMin} to {plantAreaMax}");
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        worldGrid = new CellType[gridWidth][];
        for (int x = 0; x < gridWidth; x++)
        {
            worldGrid[x] = new CellType[gridHeight];
            for (int y = 0; y < gridHeight; y++)
            {
                worldGrid[x][y] = CellType.Empty;
            }
        }
    }

    public bool IsCellOccupied(Vector3 worldPosition)
    {
        Vector3Int cellPos = WorldToCell(worldPosition);
        if (!IsWithinBounds(cellPos)) return false;

        return worldGrid[cellPos.x - plantAreaMin.x][cellPos.y - plantAreaMin.y] == CellType.Occupied;
    }

    public void SetCellOccupied(Vector3 worldPosition, bool occupied)
    {
        Vector3Int cellPos = WorldToCell(worldPosition);
        if (!IsWithinBounds(cellPos)) return;

        worldGrid[cellPos.x - plantAreaMin.x][cellPos.y - plantAreaMin.y] = occupied ? CellType.Occupied : CellType.Empty;
    }

    private bool IsWithinBounds(Vector3Int cellPos)
    {
        bool result = cellPos.x > plantAreaMin.x && cellPos.x < plantAreaMax.x &&
                  cellPos.y > plantAreaMin.y && cellPos.y < plantAreaMax.y;
        Debug.Log($"CellPos: {cellPos} | Bounds: {plantAreaMin} to {plantAreaMax} | WithinBounds: {result}");
        return result;
    }

    public bool IsWithinPlantArea(Vector3 worldPosition)
    {
        Vector3Int cellPos = WorldToCell(worldPosition);
        return IsWithinBounds(cellPos);
    }

    private Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return worldTilemap.WorldToCell(worldPosition);
    }

    public Vector3 GetCellCenterWorld(Vector3 worldPosition)
    {
        return worldTilemap.GetCellCenterWorld(WorldToCell(worldPosition));
    }
}

public enum CellType
{
    Empty,
    Occupied,
}