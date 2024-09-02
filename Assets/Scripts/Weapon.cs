using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start(){
        level = 1;
    }
}