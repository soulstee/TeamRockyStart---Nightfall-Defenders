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
    public Enemy[] currentEnemies;

    private int[] enemiesToSpawn;
    private int totalEnemiesSpawned;

    private void Start(){
        gridManager = GameManager.instance.gridManager;
    }

    public void SpawnWave(){
        currentEnemies = new Enemy[GameManager.currentWave.totalEnemies];
        enemiesToSpawn = GameManager.currentWave.enemiesID;
        spawnInterval = GameManager.currentWave.spawnInterval;
    }

    private void SpawnEnemy(){

            //Chooses random enemy id in prefab list
            int randomEnemyType = FindEnemyToSpawn();
            enemiesToSpawn[randomEnemyType]--;

            Vector2 randPos = FindRandomSpawnPos();

            GameObject enemySpawn = Instantiate(enemyPrefab[randomEnemyType], randPos, Quaternion.identity);
            //Match type with prefab (probably will be removed later)
            Enemy.TypeOfEnemy en = (Enemy.TypeOfEnemy)randomEnemyType;
            enemySpawn.GetComponent<Enemy>().Initialize(FindTarget(randPos), this);
            currentEnemies[currentEnemyCount] = enemySpawn.GetComponent<Enemy>();

            //Add to number after adding to array
            currentEnemyCount++;
            totalEnemiesSpawned++;
    }

    private int FindEnemyToSpawn(){
        bool[] validSpawns = new bool[enemiesToSpawn.Length];

        int r = (int)Random.Range(0,2);
        return r;

        for(int i = 0; i < enemiesToSpawn.Length; i++){
            if(enemiesToSpawn[i] > 0){
                validSpawns[i] = true;
            }else{
                validSpawns[i] = false;
            }
        }

        for(int i = 0; i < validSpawns.Length; i++){
            if(validSpawns[i]){
                int yn = (int)Random.Range(0,1);
                if(yn == 0){
                    return i;
                }
            }
        }

        return 0;
    }

    private Vector2 FindRandomSpawnPos(){
        //Chooses random position to spawn enemies (gives a less linear look to enemy pathing)
        float spawnOriginX = (int)spawner.position.x * gridManager.cellSize;
        float randSpawnY = (int)(Random.Range(0,gridManager.gridHeight+1)/2) * gridManager.cellSize + gridManager.gridOrigin.y ;

        return new Vector2(spawnOriginX, randSpawnY);
    }

    private Vector2 FindTarget(Vector2 _enemyPos){
        float gridAng = Mathf.Atan((gridManager.gridHeightOffset/gridManager.gridHeight));
        float triangleOffset = Mathf.Tan(gridAng);

        float endOfGridX = (int)gridManager.gridWidth * gridManager.cellSize + gridManager.gridOrigin.x + 5f - (triangleOffset*_enemyPos.y);

        return new Vector2(endOfGridX, _enemyPos.y);
    }

    private float spawnIntervalTime = 0;

    void Update()
    {
        if(!GameManager.waveEnded){
            spawnIntervalTime += Time.deltaTime;

            //Modifies spawn time slighty by time in wave (change later)
            spawnTimeModifier = 1 + GameManager.waveCurrentTime/500;
        }

        //Spawns enemies at the spawnInterval variable
        //Ends section when wave time is up and enemies are dead
        if (spawnIntervalTime >= spawnInterval/spawnTimeModifier && !GameManager.waveEnded && totalEnemiesSpawned<GameManager.currentWave.totalEnemies)
        {
            spawnIntervalTime = 0;
            //Spawn Enemy function here
            SpawnEnemy();
        }

        if(!GameManager.waveEnded && GameManager.waveCurrentTime > GameManager.currentWave.waveTime && currentEnemyCount == 0 && totalEnemiesSpawned == GameManager.currentWave.totalEnemies){

            GameManager.instance.EndWave();

            totalEnemiesSpawned = 0;
            spawnIntervalTime = 0;
            spawnTimeModifier = 0;
        }
    }
}
