using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 10f; // Damage dealt by the arrow

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the arrow collides with an enemy
        if (other.CompareTag("Enemy"))
        {
            // Deal damage to the enemy
            other.GetComponent<Enemy>().TakeDamage(damage);

            // Destroy the arrow after hitting the enemy
            Destroy(gameObject);
        }
        else if (other.CompareTag("Tower"))
        {
            // If the arrow hits the Tower, do nothing
            // Optionally, you could add code here if needed
        }
        else if (other.CompareTag("DestroyProjectile"))
        {
            Destroy(gameObject);
            //Destroys the arrow once it collides with an object dedicated to destroying the arrow
        }
    }
}
