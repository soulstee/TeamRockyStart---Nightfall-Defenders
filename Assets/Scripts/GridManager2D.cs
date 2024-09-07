using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BrickPlacer2D))]
public class GridManager2D : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 gridOrigin = Vector2.zero;  // The starting position of the grid
    public int gridWidth;                 // Number of cells horizontally
    public int gridHeight;                // Number of cells vertically
    public float gridHeightOffset = 0.5f;
    public float cellSize = 1f;                // Size of each grid cell

    [Header("Visual Settings")]
    public bool showGrid = true;               // Toggle grid visibility
    public Color gridColor = Color.gray;       // Color of the grid lines

    [Header("References")]
    [HideInInspector]
    public BrickPlacer2D brickPlacer;

    [HideInInspector]
    public bool[,] occupiedCells;             // Tracks occupied cells

    void Awake()
    {
        brickPlacer = GetComponent<BrickPlacer2D>();
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
        float y = Mathf.Round((position.y - gridOrigin.y)) + gridOrigin.y;

        float gridAng = Mathf.Atan((gridHeightOffset/gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float x = 0;

        for(int i = 0; i < gridWidth; i++){
            x = i + gridOrigin.x + gridHeightOffset - triangleOffset*y;

            if(Vector2.Distance(position, new Vector2(x,y)) < 0.6f){
                return new Vector2(x, y);
            }
        }

        return new Vector2(x, y);
    }

    /// <summary>
    /// Converts a world position to grid coordinates.
    /// </summary>
    public Vector2Int WorldToGridCoordinates(Vector2 position)
    {
        float gridAng = Mathf.Atan((gridHeightOffset/gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float x = 0;

        float y = Mathf.Round((position.y - gridOrigin.y) / cellSize);
        
        for(int i = 0; i < gridWidth; i++){
            x = i + gridOrigin.x + gridHeightOffset - triangleOffset*y;

            if(Vector2.Distance(position, new Vector2(x,y)) < 0.3f){
                return new Vector2Int(i, (int)y);
            }
        }
        return Vector2Int.zero;
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
        if (IsWithinBounds(gridCoords)){
            occupiedCells[gridCoords.x, gridCoords.y] = occupied;
            brickPlacer.CheckOpenPlacements();
        }
    }

    void OnDrawGizmos()
    {
        if (!showGrid)
            return;

        Gizmos.color = gridColor;
        for (int x = 0; x <= gridWidth-1; x++)
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