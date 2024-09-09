using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public string name;
    public int weaponId;
    public bool isUnlocked;
    public float speed;
    public float damage;
    public float fireRate;
    public int level = 1;
    public GameObject projectile;
    public AudioClip shoot;

    private void Start(){
        level = 1;
    }

    [Header("Level 2")]
    public float damageLVL2;
    public float speedLVL2;
    public float fireRateLVL2;

    [Header("Level 3")]
    public float damageLVL3;
    public float speedLVL3;
    public float fireRateLVL3;
}