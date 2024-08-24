using UnityEngine;

public class GridManager2D : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 gridOrigin = Vector2.zero;  // The starting position of the grid
    public int gridWidth = 10;                 // Number of cells horizontally
    public int gridHeight = 10;                // Number of cells vertically
    public float gridHeightOffset = 0.5f;
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
        float y = Mathf.Round((position.y - gridOrigin.y) / cellSize) * cellSize + gridOrigin.y;

        float gridAng = Mathf.Atan((gridHeightOffset/gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float x = (int)(position.x - gridOrigin.x) / cellSize * cellSize + gridOrigin.x + gridHeightOffset - triangleOffset*y;
        Debug.Log(x);

        return new Vector2(x, y);
    }

    /// <summary>
    /// Converts a world position to grid coordinates.
    /// </summary>
    public Vector2 WorldToGridCoordinates(Vector2 position)
    {
        float gridAng = Mathf.Atan((gridHeightOffset/gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float y = Mathf.Round((position.y - gridOrigin.y) / cellSize);
        float x = ((position.x - gridOrigin.x) / cellSize)  + gridHeightOffset - (triangleOffset*y);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Converts grid coordinates to world position.
    /// </summary>
    public Vector2 GridToWorldPosition(Vector2 gridCoords)
    {
        float gridAng = Mathf.Atan((gridHeightOffset/gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float y = gridCoords.y * cellSize + gridOrigin.y;
        float x = gridCoords.x * cellSize + gridOrigin.x   + gridHeightOffset - (triangleOffset*y);
        return new Vector2(x, y);
    }

    /// <summary>
    /// Checks if a cell is within grid bounds.
    /// </summary>
    public bool IsWithinBounds(Vector2 gridCoords)
    {
        return gridCoords.x >= 0f && gridCoords.x * gridCoords.y < gridWidth && gridCoords.y >= 0f && gridCoords.y < gridHeight;
    }

    /// <summary>
    /// Checks if a grid cell is occupied.
    /// </summary>
    public bool IsCellOccupied(Vector2 gridCoords)
    {
        if (!IsWithinBounds(gridCoords))
            return true; // Consider out-of-bounds as occupied

        return occupiedCells[(int)gridCoords.x, (int)gridCoords.y];
    }

    /// <summary>
    /// Sets the occupation status of a grid cell.
    /// </summary>
    public void SetCellOccupied(Vector2 gridCoords, bool occupied)
    {
        if (IsWithinBounds(gridCoords))
            occupiedCells[(int)gridCoords.x, (int)gridCoords.y] = occupied;
    }

    void OnDrawGizmos()
    {
        if (!showGrid)
            return;

        Gizmos.color = gridColor;
        for (int x = 0; x <= gridWidth; x++)
        {
            Vector3 start = new Vector3(gridOrigin.x + x * cellSize + (gridHeightOffset), gridOrigin.y, 0f);
            Vector3 end = new Vector3(gridOrigin.x + x * cellSize, gridOrigin.y + gridHeight * cellSize, 0f);
            Gizmos.DrawLine(start, end);
        }
        for (int y = 0; y <= gridHeight; y++)
        {
            float gridAng = Mathf.Atan((gridHeightOffset/gridHeight));
            float triangleOffset = Mathf.Tan(gridAng);

            Vector3 start = new Vector3(gridOrigin.x + gridHeightOffset - triangleOffset*y, gridOrigin.y + y * cellSize, 0f);
            Vector3 end = new Vector3(gridOrigin.x + gridWidth * cellSize  + gridHeightOffset - triangleOffset*y, gridOrigin.y + y * cellSize, 0f);
            Gizmos.DrawLine(start, end);
        }
    }
}
