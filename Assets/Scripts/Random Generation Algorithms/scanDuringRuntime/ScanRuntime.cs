using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanRuntime : MonoBehaviour
{
    public float scanDelay = 0.1f;

    void Start()
    {
        
        StartScan();
    }
    
    public void StartScan()
    {
        StartCoroutine(ScanAfterDelay());
    }

    private IEnumerator ScanAfterDelay()
    {
        yield return new WaitForSeconds(scanDelay);

        if (AstarPath.active != null)
        {
            AstarPath.active.Scan();
            Debug.Log("Pathfinding graph has been scanned and updated.");
        }
        else
        {
            Debug.LogError("AstarPath component is not active or not found in the scene.");
        }
    }
}

