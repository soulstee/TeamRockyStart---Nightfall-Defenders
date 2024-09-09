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

    [Header("Slots")]
    public WeaponSlot[] slots;
    public BuildSlot[] buildButtons;

    [Header("UI Stuff")]
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI pointsTextUpdate;
    public TextMeshProUGUI waveText;
    public GameObject startWaveButton;
    public Image towerHealthBar;
    public GameObject lostScreen;
    public GameObject winScreen;

    [Header("Colors")]
    public Color weaponSlotSelectedColor;
    public Color weaponSlotDefaultColor;

    [Header("Prefabs and References")]
    public GameObject bearTrapPrefab; // Declare these
    public GameObject thornsPrefab;
    public GameObject anvilTrapPrefab;
    public BrickPlacer2D brickPlacer; // Declare this

    private void Start()
    {
        for (int i = 0; i < buildButtons.Length; i++)
        {
            buildButtons[i].Initialize(i);
        }

        for (int i = 0; i < slots.Length; i++)
        {
            for (int j = 0; j < slots[i].upgradeButtons.Length; j++)
            {
                if (slots[i].upgradeButtons[j].unlocked)
                {
                    UpdateUpgradeSlots(i);
                }
            }
        }

        // Ensure BrickPlacer2D component is found
        brickPlacer = FindObjectOfType<BrickPlacer2D>();

        // Add click listeners for the trap buttons
        buildButtons[0].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(bearTrapPrefab));
        buildButtons[1].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(thornsPrefab));
        buildButtons[2].button.GetComponent<Button>().onClick.AddListener(() => SelectItem(anvilTrapPrefab));

        towerHealthBar.fillAmount = 1f;
    }

    public void PlayButtonNoise(){
        AudioManager.instance.PlayNoise("Button");
    }

    public void PlayPurchaseNoise(){
        AudioManager.instance.PlayNoise("Purchase");
    }

    // Update the points text display
    public void UpdatePointsText()
    {
        pointsText.text = GameManager.playerPoints.ToString(); // Set the points text
        CheckAllUIUpdate(GameManager.playerPoints);
    }

    public void PointVisual(int point){
        if(point < 0){
            pointsTextUpdate.color = Color.red;
            pointsTextUpdate.text =  point.ToString();
        }else if(point >= 0){
            pointsTextUpdate.color = Color.green;
            pointsTextUpdate.text = "+" + point.ToString();
        }

        

        pointsTextUpdate.gameObject.GetComponent<Animator>().SetTrigger("Visual");
    }

    public void Quit(){
        Application.Quit();
    }

    private void CheckAllUIUpdate(int _points)
    {
        // Implementation here if needed
    }

    public void UpdateWeaponSlot(int _id)
    {
        foreach (WeaponSlot g in slots)
        {
            if (g.slot.GetComponent<Image>().color == weaponSlotSelectedColor)
            {
                g.slot.GetComponent<Image>().color = weaponSlotDefaultColor;
            }
        }

        slots[_id].slot.GetComponent<Image>().color = weaponSlotSelectedColor;
    }

    public void BuyWeaponSlot(int _id)
    {
        if (slots[_id].unlocked)
        {
            return;
        }

        GameManager.instance.UpdatePoints(-slots[_id].cost);

        slots[_id].unlocked = true;
    }

    public void BuyUpgradeSlot(int _id)
    {
        if (slots[_id].currentLevel < 3)
        {
            slots[_id].currentLevel++;
        }
        else
        {
            return;
        }

        UpgradeSlot _tempSlot = slots[_id].upgradeButtons[slots[_id].currentLevel - 1];

        if (GameManager.playerPoints >= _tempSlot.cost)
        {
            _tempSlot.unlocked = true;
            GameManager.instance.UpdatePoints(-_tempSlot.cost); // Use SpendPoints method
            PlayerShoot.weapons[_id].level = slots[_id].currentLevel;
            GameManager.instance.shootScript.UpdateWeapon();
            UpdateUpgradeSlots(_id);
            PlayPurchaseNoise();
        }
    }

    public void ResetUpgradeSlots(){
        foreach(WeaponSlot s in slots){
            s.currentLevel = 1;
            for(int i = 0; i < s.upgradeButtons.Length; i++){
                if(i == 0){
                    s.upgradeButtons[i].unlocked = true;
                }else{
                    s.upgradeButtons[i].unlocked = false;
                }
                UpdateUpgradeSlots(i);
                GameManager.instance.shootScript.UpdateWeapon();
            }
        }
    }

    private void UpdateUpgradeSlots(int _id)
    {
        foreach (UpgradeSlot temp in slots[_id].upgradeButtons)
        {
            if (temp.unlocked)
            {
                temp.slot.GetComponent<Image>().color = weaponSlotSelectedColor;
            }else{
                temp.slot.GetComponent<Image>().color = weaponSlotDefaultColor;
            }
        }
    }

    // Select the item and set it as the active prefab to build
    public void SelectItem(GameObject itemPrefab)
    {
        if (brickPlacer != null)
        {
            BrickPlacer2D.buildSelected = itemPrefab;
        }
    }
}

[System.Serializable]
public class WeaponSlot
{
    public GameObject slot;
    public bool unlocked;
    public int cost;
    public int currentLevel = 1;
    public UpgradeSlot[] upgradeButtons = new UpgradeSlot[3];
}

[System.Serializable]
public class UpgradeSlot
{
    public GameObject slot;
    public int level;
    public int cost;
    public bool unlocked;
}

[System.Serializable]
public class BuildSlot
{
    public GameObject button;
    public TextMeshProUGUI priceText;

    private int cost;

    public void Initialize(int _id)
    {
        cost = GameManager.instance.brickPlacer.buildingBlocks[_id].GetComponent<Block>().cost;
        priceText.text = "$" + cost.ToString();
    }
}
