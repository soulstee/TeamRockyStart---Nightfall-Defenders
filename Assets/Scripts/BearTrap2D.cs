using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap2D : MonoBehaviour
{
    public float trapDamage = 10f;          // Damage dealt by the trap
    public float trapDuration = 2f;         // Time the enemy stays trapped
    public float rechargeTime = 5f;         // Time before the trap can be used again

    private bool isActive = true;
    private bool isRecharging;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isActive && !isRecharging)
        {
            StartCoroutine(TrapEnemy(collision.GetComponent<Enemy>()));
        if (collision.CompareTag("Enemy") && isActive)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                StartCoroutine(TrapEnemy(enemy));
            }
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

        // Wait for trap duration
        yield return new WaitForSeconds(trapDuration);

        // Restore enemy movement speed
        enemy.speed = originalSpeed;

        // Deactivate trap and start recharge
        isActive = false;
        yield return new WaitForSeconds(rechargeTime);
        isActive = true;
    }
}
