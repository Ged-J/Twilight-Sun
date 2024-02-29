using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTextBillboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        // Find and assign the main camera by its tag
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Calculate the rotation needed to look at the camera along the Y axis
            Vector3 lookAtRotation = mainCamera.transform.position - transform.position;
            lookAtRotation.x = 0f; // Keep the object's original X-axis rotation
            lookAtRotation.z = 0f; // Keep the object's original Z-axis rotation
            Quaternion rotation = Quaternion.LookRotation(lookAtRotation);
            
            // Apply the rotation around the Y-axis to the object
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
    }
}
