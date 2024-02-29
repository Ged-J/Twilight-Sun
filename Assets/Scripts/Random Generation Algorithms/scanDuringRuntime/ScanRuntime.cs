using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanRuntime : MonoBehaviour
{
    // Optionally, add a delay before scanning to ensure all game setup processes have completed
    public float scanDelay = 0.1f;

    IEnumerator Start()
    {
        // Wait for a short delay to ensure all initialization processes have completed
        yield return new WaitForSeconds(scanDelay);

        // Check if the AstarPath is active and not null
        if (AstarPath.active != null)
        {
            // Scan the graph
            AstarPath.active.Scan();
            Debug.Log("Pathfinding graph has been scanned and updated.");
        }
        else
        {
            Debug.LogError("AstarPath component is not active or not found in the scene.");
        }
    }
}
