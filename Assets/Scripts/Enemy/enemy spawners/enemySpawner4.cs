using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner4 : MonoBehaviour
{
    public GameObject[] enemies;
    public BoxCollider2D spawnArea; 
    public GameObject healthUIPrefab; 
    public int numberOfEnemiesToSpawn = 5; 
    public bool wasTriggered = false;
    public GameObject bossHealth;

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
            // Select a random enemy prefab
            GameObject enemyToSpawn = enemies[Random.Range(0, enemies.Length)];

            // Calculate a random position within the Box Collider
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnArea.size.x, spawnArea.size.x) / 2 + spawnArea.transform.position.x,
                Random.Range(-spawnArea.size.y, spawnArea.size.y) / 2 + spawnArea.transform.position.y
            );

            // Instantiate the enemy
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

            // Instantiate and attach the health UI
            GameObject enemyHealthUI = Instantiate(healthUIPrefab, Vector3.zero, Quaternion.identity, spawnedEnemy.transform);
            enemyHealthUI.transform.localPosition = new Vector3(0, 1, 0); 
        }
    }
}