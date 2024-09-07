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

    public static MouseMode mouseMode = MouseMode.Default;

    [Header("References")]
    public BrickPlacer2D brickPlacer;
    public PlayerShoot playerShoot;

    private void Update()
    {
        HandleInput();
    }

    private float nextFireTime;

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (mouseMode)
            {
                case MouseMode.Default:

                    if(Time.time <= nextFireTime || GameManager.waveEnded){
                        return;
                    }

                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if(mousePosition.y > -0.8){
                        playerShoot.Shoot(mousePosition);
                        nextFireTime = Time.time + PlayerShoot.currentWeapon.fireRate;
                    }

                    break;

                case MouseMode.Build:
                    // Call PlaceItem instead of PlaceBlock
                    Vector2 mousePositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (brickPlacer != null && mousePositionY.y > -0.8)
                    {
                        brickPlacer.PlaceItem();
                    }
                    break;
            }
        }
    }

    public void ChangeToBuildMode()
    {
        mouseMode = MouseMode.Build;
        if (brickPlacer != null)
        {
            brickPlacer.CheckOpenPlacements();
        }
    }

    public void ChangeToDefaultMode(){
        mouseMode = MouseMode.Default;
        if (brickPlacer != null)
        {
            brickPlacer.CheckOpenPlacements();
        }
    }
}
