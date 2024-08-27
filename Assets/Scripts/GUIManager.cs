using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public enum Phase{
        Prep,
        Wave,
    }

    public Phase phase;

    public WeaponSlot[] slots;
    public BuildSlot[] buildButtons;

    [Header("UI stuff")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI waveText;
    public GameObject startWaveButton;
    public Image towerHealthBar;

    private void Start(){
        foreach(BuildSlot b in buildButtons){
            b.Initialize();
        }

        towerHealthBar.fillAmount = 1f;
    }

    public void UpdateTowerHealthBar(float _damage){
        towerHealthBar.fillAmount -= _damage/100f;

        if(towerHealthBar.fillAmount <= 0){
            GameManager.instance.EndGame();
        }
    }

    // Update the points text display
    public void UpdatePointsText(int points)
    {
        pointsText.text = points.ToString(); // Set the points text
    }
}

[System.Serializable]
public class WeaponSlot{
    public GameObject slot;

    public GameObject[] upgradeButtons = new GameObject[3];
}

[System.Serializable]
public class BuildSlot{
    public GameObject button;
    public TextMeshProUGUI priceText;

    public int cost;

    public void Initialize(){
        priceText.text = "$" + cost.ToString();
    }
}
