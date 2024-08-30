using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Spawner spawner;

    public Wave[] waves;
    public static Wave currentWave;

    private int currentWaveNum = 0;
    public int maxWaves;

    public GridManager2D gridManager;
    public BrickPlacer2D brickPlacer;
    private GUIManager gui;

    public static bool waveEnded = true;

    public static float waveCurrentTime = 0;

    public static int playerPoints = 0;

    public int initialPoints;
    //How many points the player should start with. Adjustable in the inspector.

    private void Awake()
    {
        instance = this;
        gui = GetComponent<GUIManager>();
    }

    private void Start()
    {
        InitializePoints();
        //Sets the starting amount of points
    }

    private void InitializePoints()
    {
        playerPoints = initialPoints;
        gui.UpdatePointsText(playerPoints);
    }

    private void Update(){
        if(!waveEnded){
            GameManager.waveCurrentTime += Time.deltaTime;
        }
    }

    public static void UpdatePoints(int points){
        playerPoints += points;
    }

    public void StartWave(){
        currentWaveNum++;
        currentWave = waves[currentWaveNum-1];
        gui.waveText.text = "Wave " + currentWaveNum;
        spawner.SpawnWave();
        waveEnded = false;
    }

    public void EndWave(){
        waveEnded = true;
        waveCurrentTime = 0;
        gui.waveText.text = "Preparation Phase";
        if(currentWaveNum <= maxWaves){
            gui.startWaveButton.SetActive(true);
        }else{
            FinishLevel();
        }
    }

    public void EndGame(){
        Debug.Log("Lost game.");
    }

    private void FinishLevel(){
        Debug.Log("Finished level.");
    }

    public void ChangeToBuildMode(){
        MouseController.mouseMode = MouseController.MouseMode.Build;
        brickPlacer.CheckOpenPlacements();
    }

    public void ChangeToDefaultMode(){
        MouseController.mouseMode = MouseController.MouseMode.Default;
        brickPlacer.CheckOpenPlacements();
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
            gui.UpdatePointsText(playerPoints); // Update UI here
            Debug.LogError(playerPoints);
        }
        else
        {
            ChangeToDefaultMode();
            Debug.Log("Cannot afford this purchase!");
        }
    }
}
