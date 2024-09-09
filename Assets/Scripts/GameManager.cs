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
    public Tower towerScript;
    public GUIManager gui;

    public static bool waveEnded = true;

    public static float waveCurrentTime = 0;

    public static int playerPoints = 100;

    private void Awake()
    {
        instance = this;
        Debug.developerConsoleVisible = true;
        gui = GetComponent<GUIManager>();
        gui.UpdatePointsText();
    }

    private void Update(){
        if(!waveEnded){
            GameManager.waveCurrentTime += Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && waveEnded ==  true){
            StartWave();
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
        gui.startWaveButton.SetActive(false);
        gui.waveText.text = "Wave " + currentWaveNum;
        spawner.SpawnWave();
        waveEnded = false;
    }

    public void EndWave(){
        waveEnded = true;
        waveCurrentTime = 0;

        if(currentWaveNum > maxWaves-1){
            WinGame();
        }

        gui.waveText.text = "Preparation Phase";
        if(currentWaveNum <= maxWaves-1){
            gui.startWaveButton.SetActive(true);
        }
    }

    public void LostGame(){
        waveEnded = true;
        gui.lostScreen.SetActive(true);
        foreach(Enemy e in Spawner.currentEnemies){

            if(e != null)
                Destroy(e.gameObject);
        }
    }

    private void WinGame(){
        gui.waveText.text = "You Won!";

        gui.winScreen.SetActive(true);
    }

    public void ResetGame(){
        playerPoints = 100;
        gui.UpdatePointsText();
        waveEnded = true;
        waveCurrentTime = 0;
        currentWave = waves[0];
        for(int i = 0; i < Spawner.currentEnemies.Count; i++){
            Debug.Log(Spawner.currentEnemies[i].gameObject.name);
            Destroy(Spawner.currentEnemies[i].gameObject);
        }
        foreach(GameObject proj in PlayerShoot.projectiles){
            Destroy(proj);
        }
        brickPlacer.Reset();
        gui.waveText.text = "Preparation Phase";
        currentWaveNum = 0;
        towerScript.ResetHealth();
        shootScript.ResetWeapon();
        gui.ResetUpgradeSlots();
        gui.startWaveButton.SetActive(true);
        Spawner.currentEnemies = new List<Enemy>();
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
