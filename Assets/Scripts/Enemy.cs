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
    
    private Transform targetTower; // Reference to the tower

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

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("Enemy took damage!");
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            Die();
        }
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
