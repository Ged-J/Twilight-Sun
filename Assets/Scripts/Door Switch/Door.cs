using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEngine;

using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private SpriteRenderer doorRenderer; // For visual representation
    private Collider2D doorCollider;     // For physical blocking

    private void Start()
    {
        doorRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();

        // Initialize door as closed
        doorRenderer.enabled = true; // Door is visible
        doorCollider.enabled = true; // Door collider is active
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        Debug.Log("Toggling door. Open: " + isOpen);

        if (isOpen)
        {
            // Open the door
            doorRenderer.enabled = false; // Make door invisible
            doorCollider.enabled = false; // Disable collider so player can pass through
        }
        else
        {
            // Close the door
            doorRenderer.enabled = true;  // Make door visible again
            doorCollider.enabled = true;  // Enable collider so door blocks player
        }
    }
}

