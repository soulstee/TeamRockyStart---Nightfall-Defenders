using UnityEngine;

public class BrickPlacer2D : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject brickPrefab;            // Regular Brick Prefab
    public GameObject spikeBrickPrefab;       // Spike Brick Prefab
    public GameObject occupiedCircle;

    [Header("References")]
    public GridManager2D gridManager;         // Reference to GridManager2D

    private enum PlacementMode { Brick, SpikeBrick }
    private PlacementMode currentMode = PlacementMode.Brick;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Toggle between placement modes using Spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMode = currentMode == PlacementMode.Brick ? PlacementMode.SpikeBrick : PlacementMode.Brick;
            Debug.Log("Current Mode: " + currentMode.ToString());
        }

        // Place block on left mouse click
        //if (Input.GetMouseButtonDown(0))
        //{
        //    PlaceBlock();
        //}
    }

    GameObject[] gridOccupiedCircles = new GameObject[15];

    public void CheckOpenPlacements(){
        foreach(GameObject g in gridOccupiedCircles){
            Destroy(g);
        }

        for(int x = 0; x < gridManager.gridWidth; x++){
            for(int y = 0; y < gridManager.gridHeight; y++){
                if(!gridManager.occupiedCells[x, y]){
                    Vector2 gridPos = new Vector2(x,y);
                    Instantiate(occupiedCircle, gridManager.GridToWorldPosition(gridPos), Quaternion.identity);
                }
            }
        }
    }

    public void PlaceBlock()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gridPosition = gridManager.GetNearestPointOnGrid(mousePosition);
        Vector2Int gridCoords = gridManager.WorldToGridCoordinates(gridPosition);

        // Check if the position is within bounds and not occupied
        if (gridManager.IsWithinBounds(gridCoords) && !gridManager.IsCellOccupied(gridCoords))
        {
            GameObject prefabToPlace = currentMode == PlacementMode.Brick ? brickPrefab : spikeBrickPrefab;
            Instantiate(prefabToPlace, gridPosition, Quaternion.identity);
            gridManager.SetCellOccupied(gridCoords, true);
        }
        else
        {
            Debug.LogWarning("Cannot place block here. Position is either out of bounds or already occupied.");
        }
    }
}
