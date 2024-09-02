using UnityEngine;

[RequireComponent(typeof(GridManager2D))]
public class BrickPlacer2D : MonoBehaviour
{
    [Header("Prefab Settings")]
    public static GameObject currentBuildPrefab;
    public GameObject[] buildingBlocks;
    public GameObject occupiedCircle;

    [Header("References")]
    private GridManager2D gridManager;         // Reference to GridManager2D

    GameObject[,] gridOccupiedCircles = new GameObject[3,5];

    public static GameObject buildSelected;

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

    public void SelectBuildingBlock(int _id){
        currentBuildPrefab = buildingBlocks[_id];
    }

    public void CheckOpenPlacements(){
        //Check if occupable circle is needed

        if(MouseController.mouseMode == MouseController.MouseMode.Default){
            for(int x = 0; x < gridManager.gridWidth; x++){
                for(int y = 0; y < gridManager.gridHeight; y++){
                    gridOccupiedCircles[x,y].SetActive(false);
                }
            }
        }else if(MouseController.mouseMode == MouseController.MouseMode.Build && GameManager.playerPoints >= currentBuildPrefab.GetComponent<Block>().cost){
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
        }else{
            MouseController.mouseMode = MouseController.MouseMode.Default;
        }
    }

    public void PlaceBlock()
    {
        if(GameManager.playerPoints < currentBuildPrefab.GetComponent<Block>().cost){
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gridPosition = gridManager.GetNearestPointOnGrid(mousePosition);
        Vector2Int gridCoords = gridManager.WorldToGridCoordinates(gridPosition);

        // Check if the position is within bounds and not occupied
        if (gridManager.IsWithinBounds(gridCoords) && !gridManager.IsCellOccupied(gridCoords))
        {
            Instantiate(currentBuildPrefab, gridPosition, Quaternion.identity);
            gridManager.SetCellOccupied(gridCoords, true);

            GameManager.instance.UpdatePoints(-currentBuildPrefab.GetComponent<Block>().cost);

            GameManager.instance.ChangeToDefaultMode();
        }
        else
        {
            Debug.LogWarning("Cannot place block here. Position is either out of bounds or already occupied.");
        }
    }
}
