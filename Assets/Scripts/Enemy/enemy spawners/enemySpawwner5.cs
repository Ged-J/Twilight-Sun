using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enemySpawwner5 : MonoBehaviour
{
    public GameObject[] enemies;
    public BoxCollider2D spawnArea;
    public int numberOfEnemiesToSpawn = 5;
    public bool wasTriggered = false;
    public GameObject bossHealth;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasTriggered || GameSession.boss3)
            return;

        wasTriggered = true;
        SpawnEnemies();
        bossHealth.SetActive(true);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            GameObject enemyToSpawn = enemies[Random.Range(0, enemies.Length)];
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnArea.size.x, spawnArea.size.x) / 2 + spawnArea.transform.position.x,
                Random.Range(-spawnArea.size.y, spawnArea.size.y) / 2 + spawnArea.transform.position.y
            );

            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(spawnedEnemy);
        }
    }

    public void EnemyDefeated(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        if (spawnedEnemies.Count == 0)
        {
            SceneManager.LoadScene("RandomGenScene"); 
        }
    }
}