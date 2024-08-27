using System.Collections;
using System.Collections.Generic;
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
    public float damage = 10f; // Damage dealt to the tower

    private float originalSpeed;
    private Vector2 target;
    private bool isDead = false;
<<<<<<< Updated upstream
=======
    private bool inPool = false;
    private Transform targetTower; // Reference to the tower
>>>>>>> Stashed changes

    private void Start()
    {
        originalSpeed = speed; // Save the original speed value
        targetTower = GameObject.FindWithTag("Tower")?.transform; // Find the tower in the scene

        if (targetTower == null)
        {
            Debug.LogError("No tower found with the tag 'Tower'. Ensure your tower is tagged correctly.");
        }
    }

    private void Update()
    {
        if (target != null && !isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
        }
    }

    public void Initialize(Vector2 _target)
    {
        target = _target;
    }

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

    public void TakeDamage(float damageAmount)
>>>>>>> Stashed changes
    {
        Debug.Log("Enemy took damage!");
        health -= damageAmount;
        if (health <= 0)
        {
            isDead = true;
<<<<<<< Updated upstream
            Die();
=======
            // Handle enemy death
            Destroy(gameObject);
>>>>>>> Stashed changes
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
        {
            AttackTower(); // Attack the tower
        }
    }

    private void AttackTower()
    {
        if (targetTower != null)
        {
            Tower tower = targetTower.GetComponent<Tower>();
            if (tower != null)
            {
                tower.TakeDamage(damage); // Inflict damage on the tower
                Debug.Log("Attacking tower with " + damage + " damage.");
            }
            else
            {
                Debug.LogError("Tower component not found on the target tower.");
            }
        }
    }
}
