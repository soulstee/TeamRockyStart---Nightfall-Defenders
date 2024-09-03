using UnityEngine;

public class ProjectileDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is a projectile
        if (other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);  // Destroy the projectile
        }
    }
}
