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
    private Transform target;

    private bool isDead = false;
    private bool inPool = false;

    public void Initialize(TypeOfEnemy _type){
        type = _type;

        //Do changes and settings here

        target = GameManager.instance.FindTarget(this.transform);
        Debug.Log(target);
    }

    private void Update(){
        //Move enemy toward target

        if(target != null && transform.position.x != target.position.x)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }

    public void ResetEnemy(){
        isDead = false;
        inPool = false;
    }

    public bool isInPool(){
        return inPool;
    }
}
