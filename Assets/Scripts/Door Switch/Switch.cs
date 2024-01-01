using UnityEngine;

using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject door; // Assign your door GameObject here
    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed near switch");
            door.GetComponent<Door>().ToggleDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player has the tag "Player"
        {
            isPlayerNear = true;
            Debug.Log("Player entered switch area");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player exited switch area");
        }
    }
}

