using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Dictionary<int, Weapon> weapons = new Dictionary<int, Weapon>();

    public static Weapon currentWeapon;

    private static float currentDamage;
    private static float currentSpeed;
    public static float currentFireRate;

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

        if(Input.GetKeyDown(KeyCode.K)){
            GameManager.instance.UpdatePoints(3000);
        }

        if(Input.GetKeyDown(KeyCode.Tab) && MouseController.mouseMode == MouseController.MouseMode.Default){
            int temp = currentWeapon.weaponId + 1;

            if(temp > 2){
                temp = 0;
            }

            ChangeWeapon(temp);
        }
    }

    public void UpdateWeapon(){
        if(currentWeapon.level == 1){
            currentDamage = currentWeapon.damage;
            currentFireRate = currentWeapon.fireRate;
            currentSpeed = currentWeapon.speed;
        }else if(currentWeapon.level == 2){
            currentDamage = currentWeapon.damageLVL2;
            currentFireRate = currentWeapon.fireRateLVL2;
            currentSpeed = currentWeapon.speedLVL2;
        }else if(currentWeapon.level == 3){
            currentDamage = currentWeapon.damageLVL3;
            currentFireRate = currentWeapon.fireRateLVL3;
            currentSpeed = currentWeapon.speedLVL3;
        }
    }

    public void ChangeWeapon(int _id){
        if(weapons[_id].isUnlocked){
            currentWeapon = weapons[_id];
            GameManager.instance.gui.UpdateWeaponSlot(_id);
            animator.SetInteger("CurrentWeapon", _id+1);
            UpdateWeapon();
        }else{
            
        }
    }

    public Vector2 thing;

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
                    proj.GetComponent<Projectile>().Initialize(currentDamage, currentSpeed, dir);
                }
                else if(proj.GetComponent<HolyWaterProjectile>() != null){
                    proj.GetComponent<HolyWaterProjectile>().Initialize(currentDamage, currentSpeed, dir);
                }
        }
    }

    public static Vector2 rotate(Vector2 v, float delta) {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, thing);
    }
}