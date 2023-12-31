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
        Instantiate(items[0], transform.position, transform.rotation);
    }
}
