using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnLocations;
    public bool wasTriggered = false;
    public GameObject bossHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasTriggered || GameSession.boss2)
            return;
        wasTriggered = true;
        for (int i = 0; i < enemies.Length; i++)
        {
            Instantiate(enemies[i], spawnLocations[i].position, spawnLocations[i].rotation);
        }
        bossHealth.SetActive(true);
    }
}
