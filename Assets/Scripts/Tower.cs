using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health of the tower
    private float currentHealth;

    private GUIManager guiManager;

    private void Start()
    {
        currentHealth = maxHealth; // Initialize the tower's health to maxHealth
        guiManager = FindObjectOfType<GUIManager>(); // Find the GUIManager in the scene
    }

    // Method to take damage from enemies
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        // Update the health bar in the GUI
        guiManager.UpdateTowerHealthBar(damage);

        // Check if the tower's health has reached 0
        if (currentHealth <= 0)
        {
            DestroyTower(); // Destroy the tower
        }
    }

    // Method to handle the destruction of the tower
    private void DestroyTower()
    {
        // Play destruction effects, sounds, etc. (optional)
        Debug.Log("The tower has been destroyed!");

        // Optionally, trigger end of game or other effects
        GameManager.instance.EndGame(); // Example: end the game
    }
}
