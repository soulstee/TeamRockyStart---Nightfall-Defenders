using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Dictionary<int, Weapon> weapons = new Dictionary<int, Weapon>();

    public Weapon currentWeapon;

    public Transform firePoint;

    private void Awake(){
        Weapon[] temp = Resources.LoadAll<Weapon>("Weapons");

        foreach(Weapon w in temp){
            weapons.Add(w.weaponId, w);
        }
    }

    private void Start(){
        currentWeapon = weapons[0];
    }

    private float nextFireTime = 0f;

    private void Update(){
        if(GameManager.waveEnded){
            return;
        }

        if (Input.GetMouseButtonDown(0) && MouseController.mouseMode == MouseController.MouseMode.Default && Time.time >= nextFireTime)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Shoot(mousePosition);
            nextFireTime = Time.time + currentWeapon.fireRate;
        }
    }

    private void Shoot(Vector2 mousePos){
        if(firePoint != null){
            GameObject proj = Instantiate(currentWeapon.projectile, firePoint.transform.position, Quaternion.identity);
            Vector2 dir = (mousePos-(Vector2)firePoint.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            proj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            proj.GetComponent<Projectile>().Initialize(currentWeapon.damage, currentWeapon.speed, dir);
        }
    }
}
