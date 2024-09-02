using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnvilTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    public Sprite[] activeSprites;  // Sprites for active trap animation
    public Sprite inactiveSprite;   // Sprite for inactive trap state
    public float activationDelay;   // Delay between sprite changes during activation
    public float rechargeTime;      // Time to recharge the trap

    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D trapCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trapCollider = GetComponent<Collider2D>();
        SetTrapInactive();  // Ensure trap starts as inactive
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is an enemy and trap is inactive
        if (collision.CompareTag("Enemy") && !isActive)
        {
            // Get the enemy component
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Enemy collided with the trap.");
                StartCoroutine(ActivateTrap(enemy));
            }
        }
    }

    private IEnumerator ActivateTrap(Enemy enemy)
    {
        isActive = true;
        trapCollider.enabled = false; // Disable the collider to prevent re-triggering

        // Sequentially change sprites for active trap animation
        foreach (Sprite sprite in activeSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(activationDelay);
        }

        // Deal damage to the enemy
        DealDamage(enemy);

        // Recharge and reset trap
        yield return new WaitForSeconds(rechargeTime);

        SetTrapInactive();
    }

    private void DealDamage(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.TakeDamage(150f); // Example damage amount
        }
    }

    private void SetTrapInactive()
    {
        spriteRenderer.sprite = inactiveSprite; // Set inactive sprite
        trapCollider.enabled = true;            // Re-enable collider for new activations
        isActive = false;                       // Mark trap as inactive
    }
}