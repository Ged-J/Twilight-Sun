using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueScript2 : MonoBehaviour
{
    public void EnemyFelled()
    {
        GameSession.boss2 = true;
    }
}
