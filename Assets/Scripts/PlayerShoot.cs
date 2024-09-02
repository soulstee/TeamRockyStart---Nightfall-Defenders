using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Dictionary<int, Weapon> weapons = new Dictionary<int, Weapon>();

    public static Weapon currentWeapon;

    public Transform firePoint;

    private Animator animator;

    private void Awake(){
        Weapon[] temp = Resources.LoadAll<Weapon>("Weapons");

        foreach(Weapon w in temp){
            weapons.Add(w.weaponId, w);
        }

        animator = GetComponent<Animator>();
    }

    private void Start(){
        for(int i = 0; i < weapons.Count; i++){
            weapons[i].level = 1;
        }
        ChangeWeapon(0);
    }

    private float nextFireTime = 0f;

    private void Update(){
        if(GameManager.waveEnded){
            return;
        }

        if (Input.GetMouseButton(0) && MouseController.mouseMode == MouseController.MouseMode.Default && Time.time >= nextFireTime && !GameManager.waveEnded)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Shoot(mousePosition);
            nextFireTime = Time.time + currentWeapon.fireRate;
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
        }else{
            //Show locked something
        }
    }

    private void Shoot(Vector2 mousePos){
        if(firePoint != null){
            animator.Play("Bow_Fire");

            GameObject proj = Instantiate(currentWeapon.projectile, firePoint.transform.position, Quaternion.identity);
            Vector2 dir = (mousePos-(Vector2)firePoint.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            proj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            proj.GetComponent<Projectile>().Initialize(currentWeapon.damage*currentWeapon.level, currentWeapon.speed*currentWeapon.level, dir);
        }
    }
}