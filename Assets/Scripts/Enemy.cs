using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Include this line to access Unity classes

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

    private float originalSpeed;
    private Vector2 target;
    private bool isDead = false;
<<<<<<< HEAD
    
    private Transform targetTower; // Reference to the tower
=======
<<<<<<< Updated upstream
=======
    private bool inPool = false;
    private Transform targetTower; // Reference to the tower
>>>>>>> Stashed changes
>>>>>>> parent of 741a4e3 (Bow Change)

    private void Start()
    {
        originalSpeed = speed; // Save the original speed value
    }

    private void Update()
    {
        // Move enemy toward target
        if (target != null && transform.position.x != target.x)
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
    }

    public void Initialize(Vector2 _target)
    {
        target = _target;
    }

<<<<<<< HEAD
=======
<<<<<<< Updated upstream
    public void TakeDamage(float damage)
=======
    public void ResetEnemy()
    {
        isDead = false;
        inPool = false;
    }

    public bool isInPool()
    {
        return inPool;
    }

>>>>>>> parent of 741a4e3 (Bow Change)
    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Enemy took damage!");
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
<<<<<<< HEAD
            Die();
=======
<<<<<<< Updated upstream
            Die();
=======
            // Handle enemy death
            Destroy(gameObject);
>>>>>>> Stashed changes
>>>>>>> parent of 741a4e3 (Bow Change)
        }
    }

    private void Die(){
        GameManager.instance.UpdatePoints(10);
        Destroy(this.gameObject);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
