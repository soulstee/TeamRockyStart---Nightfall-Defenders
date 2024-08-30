using System.Collections;
using UnityEngine;

public class BearTrap2D : MonoBehaviour
{
    public float trapDamage = 10f;          // Damage dealt by the trap
    public float trapDuration = 2f;         // Time the enemy stays trapped
    public float rechargeTime = 5f;         // Time before the trap can be used again

    public Sprite openTrapSprite;           // Sprite for the open trap
    public Sprite closedTrapSprite;         // Sprite for the closed trap

    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    private bool isActive = true;
    private bool isRecharging = false;

    private void Start()
    {
        // Get the SpriteRenderer component attached to the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite to the open trap sprite
        spriteRenderer.sprite = openTrapSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isActive && !isRecharging)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(TrapEnemy(enemy));
            }
        }
    }

    private IEnumerator TrapEnemy(Enemy enemy)
    {
        // Deal damage to the enemy
        enemy.TakeDamage(trapDamage);

        // Disable enemy movement by setting speed to 0
        float originalSpeed = enemy.speed;
        enemy.speed = 0f;

        // Change the sprite to the closed trap
        spriteRenderer.sprite = closedTrapSprite;

        // Deactivate the trap
        isActive = false;

        // Wait for the trap duration
        yield return new WaitForSeconds(trapDuration);

        // Restore enemy movement speed
        enemy.speed = originalSpeed;

        // Start the recharge process
        isRecharging = true;
        yield return new WaitForSeconds(rechargeTime);

        // Reactivate the trap and change the sprite back to the open trap
        isActive = true;
        isRecharging = false;
        spriteRenderer.sprite = openTrapSprite;
    }
}
