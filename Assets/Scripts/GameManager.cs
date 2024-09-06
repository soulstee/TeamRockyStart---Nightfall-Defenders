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
    public PlayerShoot shootScript;
    public GUIManager gui;

    public static bool waveEnded = true;

    public static float waveCurrentTime = 0;

    public static int playerPoints = 100;

    private void Awake()
    {
        instance = this;
        gui = GetComponent<GUIManager>();
    }

    private void Update(){
        if(!waveEnded){
            GameManager.waveCurrentTime += Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.K)){
            UpdatePoints(1000);
        }
    }

    public void UpdatePoints(int points){
        playerPoints += points;

        gui.UpdatePointsText();
        gui.PointVisual(points);
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

    public void ResetGame(){
        waveEnded = true;
        waveCurrentTime = 0;
        gui.waveText.text = "Preparation Phase";
        currentWaveNum = 0;
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

    public bool AffordablePurchaseCheck(int price)
    {
        if(playerPoints < price)
        {
            return false;
        }
        return true;
    }
}
