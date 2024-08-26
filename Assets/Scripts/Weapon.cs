using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public string name;
    public int weaponId;
    public float speed;
    public float damage;
    public float fireRate;
    public int level = 1;
    public GameObject projectile;
}