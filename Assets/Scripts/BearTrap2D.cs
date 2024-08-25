using System.Collections;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public float trapDuration = 3f;
    public float damage = 10f;
    public float rechargeTime = 5f;

    private bool isActivated = false;
    private bool isRecharging = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.name); // Log the name of the object that collided

        if (other.CompareTag("Enemy") && !isActivated && !isRecharging)
        {
            Debug.Log("Bear Trap activated on " + other.name);
            StartCoroutine(TrapEnemy(other.GetComponent<Enemy>()));
        }
    }

    private IEnumerator TrapEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            isActivated = true;
            enemy.TakeDamage();

            float originalSpeed = enemy.speed;
            enemy.speed = 0f;

            yield return new WaitForSeconds(trapDuration);

            if (enemy != null && enemy.health > 0)
            {
                enemy.speed = originalSpeed;
            }

            StartCoroutine(RechargeTrap());
        }
    }

    private IEnumerator RechargeTrap()
    {
        isActivated = false;
        isRecharging = true;

        yield return new WaitForSeconds(rechargeTime);

        isRecharging = false;
    }
}
