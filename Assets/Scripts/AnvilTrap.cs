using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public Sprite[] activeSprites; // Sprites to display when the trap is activated
    public Sprite inactiveSprite;  // Sprite to display when the trap is inactive
    public float activationDelay = 0.1f; // Delay between sprite changes during activation
    public float rechargeTime = 5f; // Time it takes for the trap to recharge

    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D trapCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trapCollider = GetComponent<Collider2D>();
        SetTrapInactive(); // Start with the trap inactive
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive && other.CompareTag("Enemy"))
        {
            StartCoroutine(ActivateTrap(other));
        }
    }

    private IEnumerator ActivateTrap(Collider2D enemy)
    {
        isActive = true;

        // Sequentially switch sprites for the active animation
        foreach (Sprite sprite in activeSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(activationDelay);
        }

        // Deal damage to the enemy and any other close enemies
        DealDamage(enemy);

        // Set trap to inactive state
        SetTrapInactive();

        // Wait for recharge time before reactivating
        yield return new WaitForSeconds(rechargeTime);

        // Trap is ready to be activated again
        isActive = false;
        spriteRenderer.sprite = activeSprites[0]; // Set to the first active sprite
    }

    private void DealDamage(Collider2D enemy)
    {
        // Apply damage to the main enemy
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.TakeDamage(50f); // Example damage amount
        }

        // Check for other enemies close to the trap
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D nearbyEnemy in nearbyEnemies)
        {
            if (nearbyEnemy.CompareTag("Enemy") && nearbyEnemy != enemy)
            {
                Enemy otherEnemy = nearbyEnemy.GetComponent<Enemy>();
                if (otherEnemy != null)
                {
                    otherEnemy.TakeDamage(25f); // Deal less damage to nearby enemies
                }
            }
        }
    }

    private void SetTrapInactive()
    {
        spriteRenderer.sprite = inactiveSprite; // Set to the inactive sprite
        trapCollider.enabled = false;          // Disable the collider while inactive
    }
}
