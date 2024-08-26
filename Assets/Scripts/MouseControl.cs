using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public enum MouseMode
    {
        Default,
        Build,
        Upgrade,
    }

    public static MouseMode mouseMode;

    [Header("References")]
    public BrickPlacer2D brickPlacer;

    private void Update()
    {
        HandleInput();
    }

    bool mouseOverEnemy = false;

    RaycastHit2D hit;

    private void HandleInput(){

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(mouseMode == MouseMode.Default)
            hit = Physics2D.Raycast(mousePos, Vector3.right);

        // Reset mouseOverEnemy flag
        bool mouseOverEnemy = false;

        // Check if the mouse is over an enemy
        Enemy enemy = null;
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            mouseOverEnemy = true;
            enemy = hit.collider.GetComponent<Enemy>();

            // Change cursor type to indicate the ability to attack the enemy (if needed)
        }

        // Handle mouse click based on the current mode
        if (Input.GetMouseButtonDown(0))
        {
            switch (mouseMode)
            {
                case MouseMode.Default:
                    if (enemy != null)
                    {
                        // Fire weapon here and deal damage to the enemy
                        float damageAmount = 10f; // Example damage amount
                        enemy.TakeDamage(damageAmount);
                    }
                    break;

                case MouseMode.Build:
                    brickPlacer.PlaceBlock();
                    break;

                case MouseMode.Upgrade:
                    // Implement upgrade functionality here if needed
                    break;
            }
        }
    }

    private void ChangeToBuildMode(){
        mouseMode = MouseMode.Build;
        brickPlacer.CheckOpenPlacements();
    }

    private void ChangeToDefaultMode(){
        mouseMode = MouseMode.Default;
        brickPlacer.CheckOpenPlacements();
    }
}
