using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float TimeToDestroy = 10f;
    private float damage;
    private float speed;

    Vector3 direction;

    public void Initialize(float _damage, float _speed, Vector3 dir){
        damage = _damage;
        speed = _speed;
        direction = dir;
    }

    private void Start(){
        Destroy(this.gameObject, TimeToDestroy);
    }

    private void Update(){
        transform.position += direction * speed * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.CompareTag("Enemy")){
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(damage);

            Destroy(this.gameObject);
        }

    }
}