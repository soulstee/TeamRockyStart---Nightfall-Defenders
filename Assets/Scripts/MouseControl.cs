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

    private bool showGridIndicators = false;

    private void Update()
    {
        HandleInput();
        HandleGridVisibility();
    }

    private void HandleInput()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = new RaycastHit2D(); // Ensure hit is always initialized

        if (mouseMode == MouseMode.Default)
            hit = Physics2D.Raycast(mousePos, Vector3.right);

        bool mouseOverEnemy = false;
        Enemy enemy = null;

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            mouseOverEnemy = true;
            enemy = hit.collider.GetComponent<Enemy>();
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch (mouseMode)
            {
                case MouseMode.Default:
                    if (enemy != null)
                    {
                        float damageAmount = 10f; // Example damage amount
                        enemy.TakeDamage(damageAmount);
                    }
                    break;

                case MouseMode.Build:
                    // Call PlaceItem instead of PlaceBlock
                    if (brickPlacer != null)
                    {
                        brickPlacer.PlaceItem();
                    }
                    break;

                case MouseMode.Upgrade:
                    // Implement upgrade functionality here if needed
                    break;
            }
        }

        // Toggle grid indicators with TAB key
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showGridIndicators = !showGridIndicators;
            if (brickPlacer != null)
            {
                brickPlacer.SetGridVisibility(showGridIndicators);
            }
        }
    }

    private void HandleGridVisibility()
    {
        // Additional code to handle other aspects of grid visibility, if necessary
    }

    public void ChangeToBuildMode()
    {
        mouseMode = MouseMode.Build;
        if (brickPlacer != null)
        {
            brickPlacer.CheckOpenPlacements();
        }
    }

    public void ChangeToDefaultMode()
    {
        mouseMode = MouseMode.Default;
        if (brickPlacer != null)
        {
            brickPlacer.CheckOpenPlacements();
        }
    }
}
