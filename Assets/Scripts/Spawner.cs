using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform spawner;

    [SerializeField] private float waveTime;
    [SerializeField] private int maxEnemyCount;

    private float spawnTimeModifier;

    public float spawnInterval = 10f;

    private int currentEnemyCount = 0;

    private GridManager2D gridManager;

    public GameObject[] enemyPrefab;
    private Enemy[] currentEnemies;

    private void Start(){
        currentEnemies = new Enemy[maxEnemyCount];

        gridManager = GameManager.instance.gridManager;

        SpawnWave();
    }

    private void SpawnWave(){
        GameManager.waveEnded = false;
    }

    private void SpawnEnemy(){
        if(currentEnemyCount < maxEnemyCount){

            //Chooses random enemy id in prefab list
            int randomEnemyType = (int)Random.Range(0, enemyPrefab.Length);

            Vector2 randPos = FindRandomSpawnPos();

            GameObject enemySpawn = Instantiate(enemyPrefab[randomEnemyType], randPos, Quaternion.identity);
            //Match type with prefab (probably will be removed later)
            Enemy.TypeOfEnemy en = (Enemy.TypeOfEnemy)randomEnemyType;
            enemySpawn.GetComponent<Enemy>().Initialize(FindTarget(randPos));
            currentEnemies[currentEnemyCount] = enemySpawn.GetComponent<Enemy>();

            //Add to number after adding to array
            currentEnemyCount++;
        }
        else if(currentEnemyCount > maxEnemyCount){
            //Object pooling - Teleport old enemy objects and change their type
            //Doesn't induce lag on instantiate

            for(int i = 0; i < currentEnemyCount; i++){
                if(currentEnemies[i].isInPool()){
                    //Find enemy in the pool
                    SpawnPoolEnemy(currentEnemies[i]);
                }
            }
        }
    }

    //Reset position and type and then reset and set their type
    private void SpawnPoolEnemy(Enemy selected){
        Vector2 randPos = FindRandomSpawnPos();
        selected.gameObject.transform.position = randPos;

        int randomEnemyType = (int)Random.Range(0, enemyPrefab.Length);
        Enemy.TypeOfEnemy en = (Enemy.TypeOfEnemy)randomEnemyType;
        selected.Initialize(FindTarget(randPos));

        selected.ResetEnemy();
    }

    private Vector2 FindRandomSpawnPos(){
        //Chooses random position to spawn enemies (gives a less linear look to enemy pathing)
        float spawnOriginX = (int)spawner.position.x * gridManager.cellSize;
        float randSpawnY = (int)(Random.Range(0,gridManager.gridHeight+1)/2) * gridManager.cellSize + gridManager.gridOrigin.y ;

        return new Vector2(spawnOriginX, randSpawnY);
    }

    private Vector2 FindTarget(Vector2 _enemyPos){
        float endOfGridX = (int)gridManager.gridWidth * gridManager.cellSize + gridManager.gridOrigin.x;

        return new Vector2(endOfGridX, _enemyPos.y);
    }

    private float waveCurrentTime = 0;
    private float spawnIntervalTime = 0;

    void Update()
    {
        if(!GameManager.waveEnded){
            waveCurrentTime += Time.deltaTime;
            spawnIntervalTime += Time.deltaTime;

            //Modifies spawn time slighty by time in wave (change later)
            spawnTimeModifier = 1 + waveCurrentTime/500;
        }

        //Spawns enemies at the spawnInterval variable
        //Ends section when wave time is up and enemies are dead
        if (spawnIntervalTime >= spawnInterval/spawnTimeModifier && !GameManager.waveEnded)
        {
            spawnIntervalTime = 0;
            //Spawn Enemy function here
            SpawnEnemy();
        }

        if(!GameManager.waveEnded && waveCurrentTime > waveTime && currentEnemyCount == 0){
            GameManager.waveEnded = true;

            waveCurrentTime = 0;
            spawnIntervalTime = 0;
            spawnTimeModifier = 0;

            //End Wave function here
        }
    }
}
