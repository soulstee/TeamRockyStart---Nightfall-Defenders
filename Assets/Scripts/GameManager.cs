using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GridManager2D gridManager;
    public UIManager uiManager; // Add this line

    public static bool waveEnded = true;

    private int playerPoints = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AddPoints(int points)
    {
        playerPoints += points;
        Debug.Log("Points Added! Current Points: " + playerPoints);
        uiManager.UpdatePointsText(playerPoints); // Update UI here
    }

    public int GetPoints()
    {
        return playerPoints;
    }

    public void SpendPoints(int amount)
    {
        if (playerPoints >= amount)
        {
            playerPoints -= amount;
            Debug.Log("Points Spent! Current Points: " + playerPoints);
            uiManager.UpdatePointsText(playerPoints); // Update UI here
        }
        else
        {
            Debug.Log("Not enough points!");
        }
    }
}
