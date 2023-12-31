using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueScript1 : MonoBehaviour
{
    public void EnemyFelled()
    {
        GameSession.boss1 = true;
    }
}
