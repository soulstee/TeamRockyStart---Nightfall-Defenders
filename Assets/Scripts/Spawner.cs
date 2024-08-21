using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform spawner;

    [SerializeField] private float spawnYRange;
    [SerializeField] private float waveTime;
    [SerializeField] private int maxEnemyCount;

    private float spawnTimeModifier;

    public float spawnInterval = 10f;

    private int currentEnemyCount = 0;

    public GameObject[] enemyPrefab;
    private Enemy[] currentEnemies;

    private void Start(){
        currentEnemies = new Enemy[maxEnemyCount];

        SpawnWave();
    }

    private void SpawnWave(){
        GameManager.waveEnded = false;
    }

    private void SpawnEnemy(){
        if(currentEnemyCount < maxEnemyCount){

            //Chooses random enemy id in prefab list
            int randomEnemyType = (int)Random.Range(0, enemyPrefab.Length);

            GameObject enemySpawn = Instantiate(enemyPrefab[randomEnemyType], FindRandomSpawnPos(), Quaternion.identity);
            //Match type with prefab (probably will be removed later)
            Enemy.TypeOfEnemy en = (Enemy.TypeOfEnemy)randomEnemyType;
            enemySpawn.GetComponent<Enemy>().Initialize(en);
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
        selected.gameObject.transform.position = FindRandomSpawnPos();

        int randomEnemyType = (int)Random.Range(0, enemyPrefab.Length);
        Enemy.TypeOfEnemy en = (Enemy.TypeOfEnemy)randomEnemyType;
        selected.Initialize(en);

        selected.ResetEnemy();
    }

    private Vector3 FindRandomSpawnPos(){
        //Chooses random position to spawn enemies (gives a less linear look to enemy pathing)
        return new Vector3(spawner.transform.position.x, (float)Random.Range(spawner.transform.position.y-spawnYRange/2, spawner.transform.position.y+spawnYRange/2), spawner.transform.position.z);
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

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawner.position, spawnYRange/2);
    }
}
