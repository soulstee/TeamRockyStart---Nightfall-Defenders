using UnityEngine;
using UnityEngine.UI;

public class CannonPlacement : MonoBehaviour
{
    public GameObject cannonPrefab;   // The cannon prefab to place
    public Button placeCannonButton;  // The UI button used to place/remove the cannon
    public Transform targetSpace;     // The position where the cannon will be placed

    private GameObject placedCannon;  // The instance of the placed cannon

    void Start()
    {
        // Add a listener to the button click event
        placeCannonButton.onClick.AddListener(OnPlaceCannonButtonClick);
    }

    void OnPlaceCannonButtonClick()
    {
        if (placedCannon == null)
        {
            // Instantiate the cannon at the target space's position and rotation
            placedCannon = Instantiate(cannonPrefab, targetSpace.position, targetSpace.rotation);

            // Optionally, update the button text to indicate removal
            placeCannonButton.GetComponentInChildren<Text>().text = "Remove Cannon";
        }
        else
        {
            // Destroy the placed cannon
            Destroy(placedCannon);

            // Reset the placedCannon reference
            placedCannon = null;

            // Optionally, update the button text back to placement
            placeCannonButton.GetComponentInChildren<Text>().text = "Place Cannon";
        }
    }
}
