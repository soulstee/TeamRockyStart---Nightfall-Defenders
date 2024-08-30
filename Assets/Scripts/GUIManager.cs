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

    public Phase phase;

    public WeaponSlot[] slots;
    public BuildSlot[] buildButtons;

    [Header("UI Stuff")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI waveText;
    public GameObject startWaveButton;
    public Image towerHealthBar;

    [Header("Trap Prefabs")]
    public GameObject brickPrefab;
    public GameObject bearTrapPrefab;
    public GameObject thornsPrefab;
    public GameObject anvilPrefab;

    private BrickPlacer2D brickPlacer; // Reference to BrickPlacer2D

    private void Start()
    {
        // Initialize Build Buttons
        foreach (BuildSlot b in buildButtons)
        {
            b.Initialize();
        }

        // Get BrickPlacer2D component
        brickPlacer = FindObjectOfType<BrickPlacer2D>();

        // Add click listeners for the trap buttons
        buildButtons[0].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(brickPrefab));
        buildButtons[1].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(bearTrapPrefab));
        buildButtons[2].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(thornsPrefab));
        //buildButtons[3].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(anvilPrefab));

        towerHealthBar.fillAmount = 1f;
    }

    public void UpdateTowerHealthBar(float _damage)
    {
        towerHealthBar.fillAmount -= _damage / 100f;

        if (towerHealthBar.fillAmount <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

    // Update the points text display
    public void UpdatePointsText(int points)
    {
        pointsText.text = points.ToString(); // Set the points text
    }

    // Select the item and set it as the active prefab to build
    private void SelectItem(GameObject itemPrefab)
    {
        if (brickPlacer != null)
        {
            BrickPlacer2D.buildSelected = itemPrefab;
            Debug.Log(itemPrefab.name + " selected.");
        }
        else
        {
            Debug.LogError("BrickPlacer2D not found.");
        }
    }
}

[System.Serializable]
public class WeaponSlot
{
    public GameObject slot;

    public GameObject[] upgradeButtons = new GameObject[3];
}

[System.Serializable]
public class BuildSlot
{
    public GameObject button;
    public TextMeshProUGUI priceText;

    public int cost;

    public void Initialize()
    {
        priceText.text = "$" + cost.ToString();
    }
}
