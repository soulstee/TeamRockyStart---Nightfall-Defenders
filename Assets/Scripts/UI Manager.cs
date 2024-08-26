using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For standard Unity UI
// using TMPro; // Uncomment this if you're using TextMeshPro

public class UIManager : MonoBehaviour
{
    public Text pointsText; // For standard Unity UI
    // public TMP_Text pointsText; // Uncomment this if you're using TextMeshPro

    private void Start()
    {
        UpdatePointsText(0); // Initialize with 0 points
    }

    public void UpdatePointsText(int points)
    {
        pointsText.text = "Points: " + points.ToString();
    }
}
