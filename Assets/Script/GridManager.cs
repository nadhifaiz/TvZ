using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private Tilemap worldTilemap;

    private CellType[][] worldGrid;
    private int gridWidth, gridHeight;

    private void Start()
    {
        gridWidth = worldTilemap.cellBounds.size.x;
        gridHeight = worldTilemap.cellBounds.size.y;
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
        if (IsWithinBounds(cellPos))
        {
            return worldGrid[cellPos.x][cellPos.y] == CellType.Occupied;
        }

        return false;
    }

    public void SetCellOccupied(Vector3 worldPosition, bool occupied)
    {
        Vector3Int cellPos = WorldToCell(worldPosition);
        if (IsWithinBounds(cellPos))
        {
            worldGrid[cellPos.x][cellPos.y] = occupied ? CellType.Occupied : CellType.Empty;
        }
    }

    private bool IsWithinBounds(Vector3Int cellPos)
    {
        return cellPos.x >= 0 && cellPos.x < gridWidth && cellPos.y >= 0 && cellPos.y < gridHeight;
    }

    private Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return worldTilemap.WorldToCell(worldPosition);
    }

    private Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return worldTilemap.CellToWorld(cellPosition);
    }

    public Vector3 GetCellCenterWorld(Vector3 worldPosition)
    {
        Vector3Int cellPos = WorldToCell(worldPosition);
        return CellToWorld(cellPos);
    }
}

public enum CellType
{
    Empty,
    Occupied,
}
