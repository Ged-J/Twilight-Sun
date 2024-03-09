using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private int enemyCount;

    private void Start()
    {
        // Start the coroutine to count enemies after a delay
        StartCoroutine(CountEnemiesAfterDelay(1f)); // 1 second delay, adjust as needed
    }
    
    IEnumerator CountEnemiesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Count all enemies present after the delay
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void EnemyDefeated()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            // Load next scene when all enemies are defeated
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
