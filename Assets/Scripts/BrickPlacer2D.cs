using UnityEngine;

[RequireComponent(typeof(GridManager2D))]
public class BrickPlacer2D : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject brickPrefab;            // Regular Brick Prefab
    public GameObject occupiedCircle;

    [Header("References")]
    private GridManager2D gridManager;         // Reference to GridManager2D

    GameObject[,] gridOccupiedCircles = new GameObject[3,5];

    private void Start(){
        gridManager = GetComponent<GridManager2D>();
        //Create occupation circle graphics

        for(int x = 0; x < gridManager.gridWidth; x++){
            for(int y = 0; y < gridManager.gridHeight; y++){
                if(!gridManager.occupiedCells[x, y]){
                    Vector2 gridPos = new Vector2(x,y);
                    GameObject ocTemp = Instantiate(occupiedCircle, gridManager.GridToWorldPosition(gridPos), Quaternion.identity);
                    ocTemp.SetActive(false);
                    gridOccupiedCircles[x,y] = ocTemp;
                }
            }
        }
    }

    public void CheckOpenPlacements(){
        //Check if occupable circle is needed

        if(MouseController.mouseMode == MouseController.MouseMode.Default){
            for(int x = 0; x < gridManager.gridWidth; x++){
                for(int y = 0; y < gridManager.gridHeight; y++){
                    gridOccupiedCircles[x,y].SetActive(false);
                }
            }
        }else if(MouseController.mouseMode == MouseController.MouseMode.Build){
            for(int x = 0; x < gridManager.gridWidth; x++){
                for(int y = 0; y < gridManager.gridHeight; y++){
                    if(!gridManager.occupiedCells[x, y]){
                        gridOccupiedCircles[x,y].SetActive(true);
                    }else{
                        gridOccupiedCircles[x,y].SetActive(false);
                    }
                }
            }
        }else if(MouseController.mouseMode == MouseController.MouseMode.Upgrade){
            //Put upgrade visual here
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
            Instantiate(brickPrefab, gridPosition, Quaternion.identity);
            gridManager.SetCellOccupied(gridCoords, true);
            GameManager.instance.ChangeToDefaultMode();
        }
        else
        {
            Debug.LogWarning("Cannot place block here. Position is either out of bounds or already occupied.");
        }
    }
}
