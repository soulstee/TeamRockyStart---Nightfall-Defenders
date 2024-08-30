using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPlacer2D : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject bearTrapPrefab;   // For bear trap
    public GameObject thornsPrefab;     // For thorns
    public GameObject anvilTrapPrefab;  // For anvil trap
    public GameObject occupiedCircle;   // Visual indicator for occupied spaces

    [Header("References")]
    private GridManager2D gridManager; // Reference to GridManager2D

    private GameObject[,] gridOccupiedCircles; // Track occupied circles

    // Static reference for the item to place
    public static GameObject buildSelected;

    private void Start()
    {
        gridManager = GetComponent<GridManager2D>();
        gridOccupiedCircles = new GameObject[gridManager.gridWidth, gridManager.gridHeight];

        // Create occupation circle graphics
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                if (!gridManager.occupiedCells[x, y])
                {
                    Vector2 gridPos = new Vector2(x, y);
                    GameObject ocTemp = Instantiate(occupiedCircle, gridManager.GridToWorldPosition(gridPos), Quaternion.identity);
                    ocTemp.SetActive(false);
                    gridOccupiedCircles[x, y] = ocTemp;
                }
            }
        }
    }

    public void CheckOpenPlacements()
    {
        if (MouseController.mouseMode == MouseController.MouseMode.Default)
        {
            for (int x = 0; x < gridManager.gridWidth; x++)
            {
                for (int y = 0; y < gridManager.gridHeight; y++)
                {
                    gridOccupiedCircles[x, y].SetActive(false);
                }
            }
        }
        else if (MouseController.mouseMode == MouseController.MouseMode.Build)
        {
            for (int x = 0; x < gridManager.gridWidth; x++)
            {
                for (int y = 0; y < gridManager.gridHeight; y++)
                {
                    if (!gridManager.occupiedCells[x, y])
                    {
                        gridOccupiedCircles[x, y].SetActive(true);
                    }
                    else
                    {
                        gridOccupiedCircles[x, y].SetActive(false);
                    }
                }
            }
        }
        else if (MouseController.mouseMode == MouseController.MouseMode.Upgrade)
        {
            // Put upgrade visual here
        }
    }

    public void PlaceItem()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 gridPosition = gridManager.GetNearestPointOnGrid(mousePosition);
        Vector2Int gridCoords = gridManager.WorldToGridCoordinates(gridPosition);

        if (gridManager.IsWithinBounds(gridCoords) && !gridManager.IsCellOccupied(gridCoords))
        {
            GameObject itemToPlace = GetItemToPlace();

            if (itemToPlace != null)
            {
                Instantiate(itemToPlace, gridPosition, Quaternion.identity);
                gridManager.SetCellOccupied(gridCoords, true);

                GameManager.instance.ChangeToDefaultMode();
            }
            else
            {
                Debug.LogWarning("No valid item selected.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot place item here. Position is either out of bounds or already occupied.");
        }
    }

    private GameObject GetItemToPlace()
    {
        if (buildSelected == bearTrapPrefab)
        {
            return bearTrapPrefab;
        }
        else if (buildSelected == thornsPrefab)
        {
            return thornsPrefab;
        }
        else if (buildSelected == anvilTrapPrefab) // Anvil Trap option
        {
            return anvilTrapPrefab;
        }
        else
        {
            return null;
        }
    }

    public void SetGridVisibility(bool visible)
    {
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                gridOccupiedCircles[x, y].SetActive(visible);
            }
        }
    }
}
