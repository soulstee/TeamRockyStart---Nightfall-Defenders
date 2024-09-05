using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float maxHealth = 100f;       // Maximum health of the tower
    private float currentHealth;         // Current health of the tower

    public GameObject greenHealthBar;    // The Green health object
    public GameObject redHealthBar;      // The Red health object

    private float originalBarWidth;      // Original width of the health bar

    private void Start()
    {
        currentHealth = maxHealth;       // Initialize the tower's health to maxHealth

        // Record the original width of the Green health bar
        originalBarWidth = greenHealthBar.transform.localScale.x;

        // Ensure the health bars are in their initial state
        UpdateHealthBar();
    }

    // Method to take damage from enemies
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        // Update the health bars
        UpdateHealthBar();

        // Check if the tower's health has reached 0
        if (currentHealth <= 0)
        {
            DestroyTower(); // Destroy the tower
        }
    }

    // Method to update the health bar UI
    private void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth; // Calculate the current health percentage

        // Update the Green health bar width (shrink as health decreases)
        Vector3 greenScale = greenHealthBar.transform.localScale;
        greenScale.x = originalBarWidth * healthPercent;
        greenHealthBar.transform.localScale = greenScale;

        // Update the Red health bar width (expand as health decreases)
        Vector3 redScale = redHealthBar.transform.localScale;
        redScale.x = originalBarWidth * (1 - healthPercent);  // The remaining part is red
        redHealthBar.transform.localScale = redScale;
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
