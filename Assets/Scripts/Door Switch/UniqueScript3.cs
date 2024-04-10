using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueScript3 : MonoBehaviour
{
    public void EnemyFelled()
    {
        GameSession.boss3 = true;
    }
}
