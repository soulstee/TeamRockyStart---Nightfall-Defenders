using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum TypeOfEnemy
    {
        Zombie = 0,
        Wolf,
        Ghost,
    }

    public TypeOfEnemy type;
    public float speed;
    public float health;
    public float damage; // Damage dealt to the tower
    public int pointsOnDeath;
    public float timeDisturbed;
    public float attackRate; // Attacks per second

    bool disturbed;
    float waveTimeSave;

    private float originalSpeed;
    public Vector2 target;
    private bool isDead = false;
    private bool inPool = false;
    private Transform targetTower; // Reference to the tower
    private Spawner spawner;
    private Animator anim;
    private bool isAttacking = false; // Tracks if the enemy is attacking the tower

    private void Start()
    {
        originalSpeed = speed; // Save the original speed value
        targetTower = GameObject.FindWithTag("Tower")?.transform; // Find the tower in the scene
        anim = GetComponent<Animator>();

        if (targetTower == null)
        {
            Debug.LogError("No tower found with the tag 'Tower'. Ensure your tower is tagged correctly.");
        }
    }

    private void Update()
    {
        if(disturbed){
            if((waveTimeSave + timeDisturbed) >= GameManager.waveCurrentTime){
                float increase = GameManager.waveCurrentTime-waveTimeSave;
                speed = Mathf.Lerp(0.25f, originalSpeed, increase);
            }else{
                speed = originalSpeed;
                disturbed = false;
            }
        }

        if (target != null && !isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
        }
    }

    public void Initialize(Vector2 _target, Spawner _spawner)
    {
        spawner = _spawner;
        target = _target;
    }

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
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            Hit();
        }
    }

    public void Hit()
    {
        anim.SetTrigger("Hit");
        waveTimeSave = GameManager.waveCurrentTime;
        disturbed = true;
    }

    private void Die()
    {
        for (int i = 0; i < spawner.currentEnemies.Length; i++)
        {
            if (spawner.currentEnemies[i] == this)
            {
                spawner.currentEnemyCount--;
            }
        }
        GameManager.instance.UpdatePoints(pointsOnDeath);
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
        if (other.CompareTag("Tower") && !isAttacking)
        {
            StartCoroutine(AttackTower()); // Start attacking the tower
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Tower"))
        {
            StopCoroutine(AttackTower()); // Stop attacking when leaving the tower range
            isAttacking = false;
        }
    }

    private IEnumerator AttackTower()
    {
        isAttacking = true;
        Tower tower = targetTower.GetComponent<Tower>();

        while (isAttacking && tower != null)
        {
            tower.TakeDamage(damage); // Inflict damage on the tower
            Debug.Log("Attacking tower with " + damage + " damage.");
            yield return new WaitForSeconds(1f / attackRate); // Attack based on attack rate
        }
    }
}
