using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile to be fired
    public Transform firePoint;          // Where the projectile is fired from
    public float fireRate;          // How often the cannon fires
    public float range;            // How far the cannon can detect enemies

    public Sprite restSprite;            // Sprite when the cannon is at rest
    public Sprite shootSprite;           // Sprite when the cannon shoots

    private float nextFireTime = 0f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = restSprite;  // Set the cannon to the rest sprite initially
    }

    private void Update()
    {
        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            // Look for the nearest enemy within range
            Collider2D nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                StartCoroutine(FireAt(nearestEnemy.transform));
                nextFireTime = Time.time + 1f / fireRate;  // Set the next fire time
            }
        }
    }

    private Collider2D FindNearestEnemy()
    {
        // Get all colliders within range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range);
        Collider2D nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        // Find the closest enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }

    private IEnumerator FireAt(Transform target)
    {
        // Change to the shooting sprite
        spriteRenderer.sprite = shootSprite;

        // Create the projectile and set its direction
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            Vector2 direction = (target.position - firePoint.position).normalized;
            rb.velocity = direction * 10f;  // Adjust the speed if needed
        }

        // Wait a moment to let the shooting sprite be visible
        yield return new WaitForSeconds(0.1f);

        // Change back to the rest sprite
        spriteRenderer.sprite = restSprite;
    }
}