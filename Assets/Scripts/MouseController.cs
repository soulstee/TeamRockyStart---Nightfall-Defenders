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

    private void HandleInput()
    {
        // Switch between Default and Build modes with Tab key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mouseMode = mouseMode == MouseMode.Default ? MouseMode.Build : MouseMode.Default;
            brickPlacer.CheckOpenPlacements();
        }

        // Get the mouse position in the world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);  // Use Vector2.zero for ray direction to avoid issues

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
}
