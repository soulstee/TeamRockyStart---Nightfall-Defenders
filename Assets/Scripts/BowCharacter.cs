using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCharacter : MonoBehaviour
{
    public GameObject arrowPrefab; // The arrow prefab
    public Transform arrowSpawnPoint; // The point from where arrows are fired
    public float arrowSpeed = 10f; // Speed of the fired arrow

    private Animator animator; // Reference to the Animator

    void Start()
    {
        // Get the Animator component attached to the character
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Shoot an arrow when the left mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        // Play the bow firing animation
        animator.Play("Bow_Fire");

        // Instantiate the arrow at the spawn point
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Keep the z position at 0 for 2D

        // Calculate the direction to shoot the arrow towards the mouse position
        Vector2 direction = (mousePosition - arrowSpawnPoint.position).normalized;

        // Get the Rigidbody2D component of the arrow and apply force to it
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * arrowSpeed;
        }

        // Start a coroutine to revert to idle after firing
        StartCoroutine(RevertToIdle());
    }

    private IEnumerator RevertToIdle()
    {
        // Wait until the end of the current frame to ensure the firing animation plays
        yield return new WaitForEndOfFrame();

        // Wait for the duration of the Bow_Fire animation (adjust the duration as needed)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Switch back to idle animation
        animator.Play("Bow_Idle");
    }
}
