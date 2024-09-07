using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Dictionary<int, Weapon> weapons = new Dictionary<int, Weapon>();

    public static Weapon currentWeapon;

    public Transform firePoint;

    public Animator animator;

    public static List<GameObject> projectiles = new List<GameObject>();

    private void Awake(){
        Weapon[] temp = Resources.LoadAll<Weapon>("Weapons");

        foreach(Weapon w in temp){
            weapons.Add(w.weaponId, w);
        }

        animator = GetComponent<Animator>();
    }

    public void ResetWeapon(){
        ChangeWeapon(0);
    }

    private void Start(){
        for(int i = 0; i < weapons.Count; i++){
            weapons[i].level = 1;
        }
        ChangeWeapon(0);
    }

    private void Update(){
        if(GameManager.waveEnded){
            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab) && MouseController.mouseMode == MouseController.MouseMode.Default){
            int temp = currentWeapon.weaponId + 1;

            if(temp > 2){
                temp = 0;
            }

            ChangeWeapon(temp);
        }
    }

    public void ChangeWeapon(int _id){
        if(weapons[_id].isUnlocked){
            currentWeapon = weapons[_id];
            GameManager.instance.gui.UpdateWeaponSlot(_id);
            animator.SetInteger("CurrentWeapon", _id+1);
        }else{
            //Show locked something
        }
    }

    public void Shoot(Vector2 mousePos){
        if(firePoint != null){
            animator.SetTrigger("Fire");
            
            AudioManager.instance.PlaySetNoise(currentWeapon.shoot);
            GameObject proj = Instantiate(currentWeapon.projectile, firePoint.transform.position, Quaternion.identity);
            projectiles.Add(proj);
            Vector2 dir = (mousePos-(Vector2)firePoint.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            proj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if(proj.GetComponent<Projectile>() != null){
                proj.GetComponent<Projectile>().Initialize(currentWeapon.damage*currentWeapon.level, currentWeapon.speed*currentWeapon.level, dir);
            }
            else if(proj.GetComponent<HolyWaterProjectile>() != null){
                proj.GetComponent<HolyWaterProjectile>().Initialize(currentWeapon.damage*currentWeapon.level, currentWeapon.speed*currentWeapon.level, dir);
            }
        }
    }
}