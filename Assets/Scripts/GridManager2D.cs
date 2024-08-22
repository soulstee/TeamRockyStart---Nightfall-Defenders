using UnityEngine;

public class GridManager2D : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 gridOrigin = Vector2.zero;  // The starting position of the grid
    public int gridWidth = 10;                 // Number of cells horizontally
    public int gridHeight = 10;                // Number of cells vertically
    public float cellSize = 1f;                // Size of each grid cell

    [Header("Visual Settings")]
    public bool showGrid = true;               // Toggle grid visibility
    public Color gridColor = Color.gray;       // Color of the grid lines

    private bool[,] occupiedCells;             // Tracks occupied cells

    void Awake()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        occupiedCells = new bool[gridWidth, gridHeight];
    }

    /// <summary>
    /// Snaps a world position to the nearest grid point.
    /// </summary>
    public Vector2 GetNearestPointOnGrid(Vector2 position)
    {
        float x = Mathf.Round((position.x - gridOrigin.x) / cellSize) * cellSize + gridOrigin.x;
        float y = Mathf.Round((position.y - gridOrigin.y) / cellSize) * cellSize + gridOrigin.y;
        return new Vector2(x, y);
    }

    /// <summary>
    /// Converts a world position to grid coordinates.
    /// </summary>
    public Vector2Int WorldToGridCoordinates(Vector2 position)
    {
        int x = Mathf.RoundToInt((position.x - gridOrigin.x) / cellSize);
        int y = Mathf.RoundToInt((position.y - gridOrigin.y) / cellSize);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Converts grid coordinates to world position.
    /// </summary>
    public Vector2 GridToWorldPosition(Vector2Int gridCoords)
    {
        float x = gridCoords.x * cellSize + gridOrigin.x;
        float y = gridCoords.y * cellSize + gridOrigin.y;
        return new Vector2(x, y);
    }

    /// <summary>
    /// Checks if a cell is within grid bounds.
    /// </summary>
    public bool IsWithinBounds(Vector2Int gridCoords)
    {
        return gridCoords.x >= 0 && gridCoords.x < gridWidth && gridCoords.y >= 0 && gridCoords.y < gridHeight;
    }

    /// <summary>
    /// Checks if a grid cell is occupied.
    /// </summary>
    public bool IsCellOccupied(Vector2Int gridCoords)
    {
        if (!IsWithinBounds(gridCoords))
            return true; // Consider out-of-bounds as occupied

        return occupiedCells[gridCoords.x, gridCoords.y];
    }

    /// <summary>
    /// Sets the occupation status of a grid cell.
    /// </summary>
    public void SetCellOccupied(Vector2Int gridCoords, bool occupied)
    {
        if (IsWithinBounds(gridCoords))
            occupiedCells[gridCoords.x, gridCoords.y] = occupied;
    }

    void OnDrawGizmos()
    {
        if (!showGrid)
            return;

        Gizmos.color = gridColor;
        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(gridOrigin.x + x * cellSize, gridOrigin.y, 0f);
            Vector3 end = new Vector3(gridOrigin.x + x * cellSize, gridOrigin.y + gridHeight * cellSize, 0f);
            Gizmos.DrawLine(start, end);
        }
        for (int y = 0; y <= gridHeight; y++)
        {
            Vector3 start = new Vector3(gridOrigin.x, gridOrigin.y + y * cellSize, 0f);
            Vector3 end = new Vector3(gridOrigin.x + gridWidth * cellSize, gridOrigin.y + y * cellSize, 0f);
            Gizmos.DrawLine(start, end);
        }
    }
}
