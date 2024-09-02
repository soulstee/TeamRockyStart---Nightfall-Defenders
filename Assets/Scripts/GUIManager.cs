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

    [Header("Slots")]

    public WeaponSlot[] slots;
    public BuildSlot[] buildButtons;

    [Header("UI stuff")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI waveText;
    public GameObject startWaveButton;
    public Image towerHealthBar;

    [Header("Colors")]

    public Color weaponSlotSelectedColor;
    public Color weaponSlotDefaultColor;

    private void Start(){
        for(int i = 0; i < buildButtons.Length; i++){
            buildButtons[i].Initialize(i);
        }

        for(int i = 0; i < slots.Length; i++){
            for(int j = 0; j < slots[i].upgradeButtons.Length; j++){
                if(slots[i].upgradeButtons[j].unlocked){
                    UpdateUpgradeSlots(i);
                }
            }
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
    public void UpdatePointsText()
    {
        pointsText.text = GameManager.playerPoints.ToString(); // Set the points text
        CheckAllUIUpdate(GameManager.playerPoints);
    }

    private void CheckAllUIUpdate(int _points){
        
    }

    public void UpdateWeaponSlot(int _id){
        foreach(WeaponSlot g in slots){
            if(g.slot.GetComponent<Image>().color == weaponSlotSelectedColor){
                g.slot.GetComponent<Image>().color = weaponSlotDefaultColor;
            }
        }

        slots[_id].slot.GetComponent<Image>().color = weaponSlotSelectedColor;
    }

    public void BuyWeaponSlot(int _id){
        if(slots[_id].unlocked){
            return;
        }

        GameManager.instance.UpdatePoints(-slots[_id].cost);

        slots[_id].unlocked = true;
    }

    public void BuyUpgradeSlot(int _id){
        if(slots[_id].currentLevel < 3 ){
            slots[_id].currentLevel++;}
        else{
            return;
        }

        UpgradeSlot _tempSlot = slots[_id].upgradeButtons[slots[_id].currentLevel-1];

        if(GameManager.playerPoints >= _tempSlot.cost){
            _tempSlot.unlocked = true;
            GameManager.instance.UpdatePoints(-_tempSlot.cost);
            PlayerShoot.weapons[_id].level = slots[_id].currentLevel;
            UpdateUpgradeSlots(_id);
        }
    }

    private void UpdateUpgradeSlots(int _id){
        foreach(UpgradeSlot temp in slots[_id].upgradeButtons){
            if(temp.unlocked == true){
                temp.slot.GetComponent<Image>().color = weaponSlotSelectedColor;
            }
        }
    }
}

[System.Serializable]
public class WeaponSlot{
    public GameObject slot;
    public bool unlocked;
    public int cost;
    public int currentLevel = 1;
    public UpgradeSlot[] upgradeButtons = new UpgradeSlot[3];
}
[System.Serializable]
public class UpgradeSlot{
    public GameObject slot;
    public int level;
    public int cost;
    public bool unlocked;
}

[System.Serializable]
public class BuildSlot{
    public GameObject button;
    public TextMeshProUGUI priceText;

    private int cost;

    public void Initialize(int _id){
        cost = GameManager.instance.brickPlacer.buildingBlocks[_id].GetComponent<Block>().cost;
        priceText.text = "$" + cost.ToString();
    }
}
