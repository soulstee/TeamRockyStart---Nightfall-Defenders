using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public enum MouseMode{
        Default,
        Build,
    }

    public MouseMode mouseMode;

    [Header("References")]
    public BrickPlacer2D brickPlacer;

    private void Update(){
        HandleInput();
    }

    bool mouseOverEnemy = false;

    private void HandleInput(){

        if(Input.GetKeyDown(KeyCode.Tab)){
            mouseMode = mouseMode == MouseMode.Default ? MouseMode.Build : MouseMode.Default;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.right);

        mouseOverEnemy = false;

        if(hit.collider != null && hit.collider.gameObject.CompareTag("Enemy")){
            mouseOverEnemy = true;
            
            //Change cursor type to sprite that indicates ability to attack enemy
        }

        //Functionality during default & build mode

        if(Input.GetMouseButtonDown(0)){
            switch(mouseMode){
                case MouseMode.Default:

                    if(mouseOverEnemy){

                        //Fire weapon here

                        Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                        enemy.TakeDamage();
                    }

                    break;
                case MouseMode.Build:

                    brickPlacer.PlaceBlock();

                    break;
            }
        }
    }
}
