using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public enum Phase
    {
        Prep,
        Wave,
    }

    public Phase phase; // Current game phase

    public WeaponSlot[] slots; // Array of weapon slots
    public BuildSlot[] buildButtons; // Array of build buttons

    [Header("UI Stuff")]
    public TextMeshProUGUI pointsText; // Text element to display player points
    public TextMeshProUGUI waveText; // Text element to display current wave
    public GameObject startWaveButton; // Button to start a new wave
    public Image towerHealthBar; // Image representing the tower's health

    private void Start()
    {
        // Initialize each build button with its cost
        foreach (BuildSlot b in buildButtons)
        {
            b.Initialize();
        }

        // Set the initial tower health to full
        towerHealthBar.fillAmount = 1f;
    }

    // Update the tower health bar based on damage taken
    public void UpdateTowerHealthBar(float damage)
    {
        // Reduce health based on damage
        float newFillAmount = towerHealthBar.fillAmount - (damage / 100f);
        towerHealthBar.fillAmount = Mathf.Clamp(newFillAmount, 0, 1); // Ensure fillAmount stays between 0 and 1

        // Update the color from green (full health) to red (empty health)
        towerHealthBar.color = Color.Lerp(Color.red, Color.green, towerHealthBar.fillAmount);

        // End the game if health is zero
        if (towerHealthBar.fillAmount <= 0)
        {
            GameManager.instance.EndGame(); // Call the end game method
        }
    }

<<<<<<< HEAD
<<<<<<< Updated upstream
    public void UpdatePointsText(){
=======
    public void UpdatePointsText(int points){
>>>>>>> parent of 590a628 (Bow Update)
        pointsText.text = GameManager.playerPoints.ToString();
=======
    // Update the points text display
    public void UpdatePointsText(int points)
    {
        pointsText.text = points.ToString(); // Set the points text
>>>>>>> Stashed changes
    }
}

// Class to represent a weapon slot
[System.Serializable]
public class WeaponSlot
{
    public GameObject slot; // GameObject for the weapon slot
    public GameObject[] upgradeButtons = new GameObject[3]; // Array of upgrade buttons
}

// Class to represent a build slot
[System.Serializable]
public class BuildSlot
{
    public GameObject button; // GameObject for the build button
    public TextMeshProUGUI priceText; // Text element for displaying the build cost
    public int cost; // Cost of the build

    // Initialize the build slot with its cost
    public void Initialize()
    {
        priceText.text = "$" + cost.ToString(); // Set the price text
    }
}