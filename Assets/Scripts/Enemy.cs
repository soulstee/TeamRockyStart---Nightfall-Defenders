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
        if(anim == null){return;}

        anim.SetTrigger("Hit");
        waveTimeSave = GameManager.waveCurrentTime;
        disturbed = true;

        //Plays sounds half the time to not get annoying
        int rSound = (int)Random.Range(0,2);

        if(type == TypeOfEnemy.Wolf && rSound == 0){
            int r = (int)Random.Range(0,3);
            switch(r){
                case 0:
                    AudioManager.instance.PlayNoise("WolfHit1");
                    break;
                case 1:
                    AudioManager.instance.PlayNoise("WolfHit2");
                    break;
                case 2:
                    AudioManager.instance.PlayNoise("WolfHit3");
                    break;
            }
        }else if(type == TypeOfEnemy.Ghost && rSound == 0){

        }else if(type == TypeOfEnemy.Zombie && rSound == 0){
            AudioManager.instance.PlayNoise("ZombieHit");
        }
    }

    private void Die()
    {
        GameManager.instance.UpdatePoints(pointsOnDeath);
        spawner.currentEnemyCount--;
        Destroy(this.gameObject);

        if(type == TypeOfEnemy.Wolf){
            AudioManager.instance.PlayNoise("WolfDeath");
        }else if(type == TypeOfEnemy.Ghost){
            AudioManager.instance.PlayNoise("GhostDeath");
        }else if(type == TypeOfEnemy.Zombie){

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
            yield return new WaitForSeconds(1f / attackRate); // Attack based on attack rate
        }
    }
}
