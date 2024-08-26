using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public int waveNumber;

    public int[] enemiesID;
    public int totalEnemies; //How many enemies will be in a wave (add up all enemiesID numbers)

    public float spawnInterval;

    public float waveTime;
}
