using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform spawner;

    private float spawnTimeModifier;

    public float spawnInterval = 10f;

    public int currentEnemyCount = 0;

    private GridManager2D gridManager;

    public GameObject[] enemyPrefab;
    [HideInInspector]
    public static List<Enemy> currentEnemies = new List<Enemy>(); 

    private int[] enemiesToSpawn = new int[3];
    private int totalEnemiesSpawned;

    private void Start(){
        gridManager = GameManager.instance.gridManager;
    }

    public void SpawnWave(){
        spawnInterval = GameManager.currentWave.spawnInterval;
    }

    private void SpawnEnemy(){
        // Chooses random enemy id in prefab list
        int randomEnemyType = FindEnemyToSpawn();
        
        // Ensure randomEnemyType is within the bounds of enemyPrefab array
        if (randomEnemyType >= 0 && randomEnemyType < enemyPrefab.Length) {
            enemiesToSpawn[randomEnemyType]--;
            
            Vector2 randPos = FindRandomSpawnPos();

            GameObject enemySpawn = Instantiate(enemyPrefab[randomEnemyType], randPos, Quaternion.identity);
            // Match type with prefab
            currentEnemies.Add(enemySpawn.GetComponent<Enemy>());
            enemySpawn.GetComponent<Enemy>().Initialize(FindTarget(randPos), this);

            // Add to number after adding to array
            currentEnemyCount++;
            totalEnemiesSpawned++;
        } else {
            Debug.LogError("randomEnemyType is out of bounds for enemyPrefab array");
        }
    }

    private int FindEnemyToSpawn(){
        // Ensure the random index is within the bounds of enemiesToSpawn array
        int r = Random.Range(0, enemiesToSpawn.Length);

        return r;  // Return the random index found earlier if no other spawn was valid
    }

    private Vector2 FindRandomSpawnPos(){
        // Chooses random position to spawn enemies (gives a less linear look to enemy pathing)
        float spawnOriginX = -7;
        float randSpawnY = (int)(Random.Range(0, gridManager.gridHeight + 1)) * gridManager.cellSize + gridManager.gridOrigin.y;

        return new Vector2(spawnOriginX, randSpawnY);
    }

    private Vector2 FindTarget(Vector2 _enemyPos){
        float gridAng = Mathf.Atan((gridManager.gridHeightOffset / gridManager.gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float endOfGridX = (int)gridManager.gridWidth * gridManager.cellSize + gridManager.gridOrigin.x + 5f - (triangleOffset * _enemyPos.y);

        return new Vector2(endOfGridX, _enemyPos.y);
    }

    private float spawnIntervalTime = 0;

    void Update()
    {
        if (!GameManager.waveEnded){
            spawnIntervalTime += Time.deltaTime;
        }

        // Spawns enemies at the spawnInterval variable
        // Ends section when wave time is up and enemies are dead
        if (spawnIntervalTime >= spawnInterval && !GameManager.waveEnded && totalEnemiesSpawned < GameManager.currentWave.totalEnemies)
        {
            spawnIntervalTime = 0;
            // Spawn Enemy function here
            SpawnEnemy();
        }

        if (!GameManager.waveEnded && GameManager.waveCurrentTime > GameManager.currentWave.waveTime && totalEnemiesSpawned >= GameManager.currentWave.totalEnemies && currentEnemyCount <= 0)
        {
            GameManager.instance.EndWave();

            totalEnemiesSpawned = 0;
            spawnIntervalTime = 0;
            spawnTimeModifier = 0;
        }
    }
}
