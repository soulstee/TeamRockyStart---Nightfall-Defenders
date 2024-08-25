using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum TypeOfEnemy{
    Zombie = 0,
    Wolf,
    Vampire,
    }

    public TypeOfEnemy type;

    public float speed = 5f;
    public float health = 100f;

    private Vector2 target;

    private bool isDead = false;
    private bool inPool = false;

    public void Initialize(Vector2 _target){
        target = _target;
    }

    private void Update(){
        //Move enemy toward target

        if(target != null && transform.position.x != target.x)
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed * Time.deltaTime);
    }

    public void ResetEnemy(){
        isDead = false;
        inPool = false;
    }

    public bool isInPool(){
        return inPool;
    }

    public void TakeDamage(){
        Debug.Log("Enemy took damage!");
    }
}
