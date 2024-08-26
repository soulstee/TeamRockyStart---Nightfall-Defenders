using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum TypeOfEnemy
    {
        Zombie = 0,
        Wolf,
        Vampire,
    }

    public TypeOfEnemy type;

    public float speed = 5f;
    public float health = 100f;

    public int pointsValue = 10;  // Points awarded when this enemy is killed

    private Vector2 target;
    private bool isDead = false;
    private bool inPool = false;

    public void Initialize(Vector2 _target)
    {
        target = _target;
    }

    private void Update()
    {
        // Move enemy toward target
        if (target != null && transform.position.x != target.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
        }
    }

    // Call this method when the enemy takes damage
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    // Handle enemy death
    private void Die()
    {
        isDead = true;

        // Award points to the player
        GameManager.instance.AddPoints(pointsValue);

        // Optionally, destroy the enemy or handle pooling
        Destroy(gameObject);
    }

    public void ResetEnemy()
    {
        isDead = false;
        inPool = false;
        health = 100f;  // Reset health or set it dynamically as needed
    }

    public bool isInPool()
    {
        return inPool;
    }
}
