using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterProjectile : MonoBehaviour
{
    public float baseDamage;     // Base damage when hitting an enemy directly
    public float splashRadius;    // How big the splash damage area is
    public float splashDamage;   // How much damage the splash does
    private float speed;
    public float TimeToDestroy = 10f;
    Vector3 direction;

    public void Initialize(float _damage, float _speed, Vector3 dir){
        baseDamage = _damage;
        speed = _speed;
        direction = dir;
    }

    private void Update(){
        transform.position += direction * speed * Time.deltaTime;
    }

    private void Start(){
        Destroy(this.gameObject, TimeToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hit an enemy
        if (other.CompareTag("Enemy"))
        {
            // Apply direct base damage to the enemy hit
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                AudioManager.instance.PlayNoise("HolyHit");
                enemy.TakeDamage(baseDamage);
            }

            // Apply splash damage to nearby enemies
            ApplySplashDamage();
            
            // Destroy the projectile after hitting the enemy
            PlayerShoot.projectiles.Remove(this.gameObject);
            Destroy(gameObject);
        }
        // Destroy the projectile if it hits the ground or anything other than the Tower
        else if (!other.CompareTag("Tower"))
        {
            ApplySplashDamage();
            Destroy(gameObject);
        }
    }

    private void ApplySplashDamage()
    {
        // Find all enemies within the splash radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, splashRadius);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(splashDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Show the splash radius in the editor for easier adjustment
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }
}
