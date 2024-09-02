using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public Sprite[] activeSprites;   // Sprites to display when the trap is activated
    public Sprite inactiveSprite;    // Sprite to display when the trap is inactive
    public float activationDelay = 0.1f; // Delay between sprite changes during activation
    public float rechargeTime = 5f;  // Time it takes for the trap to recharge

    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D trapCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trapCollider = GetComponent<Collider2D>();
        SetTrapInactive();  // Start with the trap inactive
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isActive)
        {
            Debug.Log("Enemy collided with the trap.");
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Starting trap activation.");
                StartCoroutine(ActivateTrap(enemy));
            }
        }
    }

    private IEnumerator ActivateTrap(Enemy enemy)
    {
        isActive = true;

        // Sequentially switch sprites for the active animation
        foreach (Sprite sprite in activeSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(activationDelay);
        }

        // Deal damage to the enemy
        DealDamage(enemy);

        // Set trap to inactive state
        SetTrapInactive();

        // Wait for recharge time before reactivating
        yield return new WaitForSeconds(rechargeTime);

        // Trap is ready to be activated again
        isActive = false;
        trapCollider.enabled = true; // Enable the collider again after recharging
        spriteRenderer.sprite = inactiveSprite;
    }

    private void DealDamage(Enemy enemy)
    {
        // Apply damage to the main enemy
        if (enemy != null)
        {
            enemy.TakeDamage(150f); // Example damage amount
        }
    }

    private void SetTrapInactive()
    {
        spriteRenderer.sprite = inactiveSprite; // Set to the inactive sprite
        trapCollider.enabled = true;            // Keep collider enabled
        isActive = false;                       // Trap is inactive by default
    }
}