using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistance : MonoBehaviour
{
    private void Awake()
    {
        int numCumeras = FindObjectsOfType<Camera>().Length;

        if (numCumeras > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
