using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    public GameObject[] items;

    public void OnBossExecute()
    {
        GameSession.addKey();
        print(GameSession.keys.ToString());

        // Check if the items array is not null and has elements before instantiating
        if (items != null && items.Length > 0)
        {
            Instantiate(items[0], transform.position, transform.rotation);
        }
    }
}