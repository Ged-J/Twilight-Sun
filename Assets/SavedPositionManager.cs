using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
 
public class SavedPositionManager : MonoBehaviour // Static class to remember player positions per scene.
{
    private static SavedPositionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public static Dictionary<int, Vector3> savedPositions = new Dictionary<int, Vector3>();
}
