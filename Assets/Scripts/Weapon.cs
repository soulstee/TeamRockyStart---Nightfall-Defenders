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
}