using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using TMPro;
using Pathfinding;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    public int health;
    public GameObject bloodSplatter;
    public GameObject damagePosition;

    private TextMeshProUGUI newText;
    private TextMeshProUGUI text;
    public GameObject textGO;
    public GameObject DPSCanvas;

    private GameObject newBloodSpatter;

    private EnemyManager enemyManager;
    
    public event Action<int, int> OnHealthChanged;


    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        
        DPSCanvas = GameObject.FindGameObjectWithTag("DamageCanvas");
        text = textGO.GetComponentInChildren<TextMeshProUGUI>();
        /*print(text);*/
        
        // If for some reason it's not desirable or possible to call AdjustDifficulty here, make sure to directly invoke any necessary updates:
        AdjustDifficulty();
    }

    private void Awake()
    {
        health = CalculateHealthBasedOnDifficulty();
        DifficultyManager.instance.OnDifficultyChanged += AdjustDifficulty;
    }
    
    private void OnDestroy()
    {
        DifficultyManager.instance.OnDifficultyChanged -= AdjustDifficulty;
    }

    private void AdjustDifficulty()
    {
        int previousMaxHealth = maxHealth;
        maxHealth = CalculateMaxHealthBasedOnDifficulty();
        health = Mathf.Clamp(health, 0, maxHealth); // Adjust current health proportionally, if desired
    
        // Notify UI to update (this part will be implemented in a moment)
        OnHealthChanged?.Invoke(health, maxHealth);
    }
    
    private int CalculateMaxHealthBasedOnDifficulty()
    {
        int difficultyScore = DifficultyManager.instance.GetCurrentDifficultyScore();
        return 1 + (difficultyScore * 10); // Example formula, adjust as needed
    }

    private int CalculateHealthBasedOnDifficulty()
    {
        int difficultyScore = DifficultyManager.instance.GetCurrentDifficultyScore();
        // Decrease or increase health based on difficulty score. Adjust formula as needed.
        return 100 + (difficultyScore * 20); // Example: Starting at 100 health, plus 20 for each difficulty level
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        health = Mathf.Max(0, health); // Ensure health doesn't go below 0
        OnHealthChanged?.Invoke(health, maxHealth);
        /*print(health);*/

        //newText = Instantiate(text, transform.position, transform.rotation);
        newText = Instantiate(text, damagePosition.transform.position, transform.rotation);
        //newText.transform.parent = DPSCanvas.transform;
        newText.transform.SetParent(DPSCanvas.transform);
        newText.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2), ForceMode2D.Impulse);
        newText.SetText(dmg.ToString());
        newText.color = Color.red;
        
        bosshp.instance.TakeDamage(dmg);

        if (health <= 0)
        {
            
            /*LoadNextLevel();*/
            
            if (this.GetComponent<DropItems>() != null)
            {
                this.GetComponent<DropItems>().OnBossExecute();
            }

            if (this.GetComponent<UniqueScript1>() != null)
            {
                this.GetComponent<UniqueScript1>().EnemyFelled();
            }

            if (this.GetComponent<UniqueScript2>() != null)
            {
                this.GetComponent<UniqueScript2>().EnemyFelled();
            }

            if (this.GetComponent<UniqueScript3>() != null)
            {
                this.GetComponent<UniqueScript3>().EnemyFelled();
            } 
            enemySpawwner5 spawner = FindObjectOfType<enemySpawwner5>();
            if (spawner != null)
            {
                spawner.EnemyDefeated(gameObject);
            }
            enemyManager.EnemyDefeated();
            Destroy(this.gameObject);
            //newBloodSpatter = Instantiate(bloodSplatter, transform.position, transform.rotation);
            //Destroy(newBloodSpatter, 3);
        }
    }
    
    /*void LoadNextLevel()
    {
        // Load a specific scene
        SceneManager.LoadScene("End Credits");

        // Load the next scene in the build settings
        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // SceneManager.LoadScene(currentSceneIndex + 1);
    }*/
    
    
    
}
