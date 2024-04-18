using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private int enemyCount;

    private void Start()
    {
        // coroutine to count enemies after a delay
        StartCoroutine(CountEnemiesAfterDelay(1f)); // 1 second delay
    }
    
    IEnumerator CountEnemiesAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Count all enemies present after the delay
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        print("Enemy count" + enemyCount);
    }

    public void EnemyDefeated()
    {
        enemyCount--;
        
        print("Enemy count after defeat" + enemyCount);

        if (enemyCount <= 0)
        {
            // When all enemies are defeated, increase difficulty for the next level
            DifficultyManager.instance.LevelCompleted();
            
            // Load next scene when all enemies are defeated
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    public void RecountEnemies()
    {
        StopAllCoroutines(); // Stop any existing counting coroutines to prevent duplicate counts
        StartCoroutine(CountEnemiesAfterDelay(0.5f)); // Short delay to ensure all enemies are loaded
    }

}
