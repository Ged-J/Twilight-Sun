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


    private void Start()
    {
        DPSCanvas = GameObject.FindGameObjectWithTag("DamageCanvas");
        text = textGO.GetComponentInChildren<TextMeshProUGUI>();
        print(text);
    }

    private void Awake()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        print(health);

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
            
            LoadNextLevel();
            
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
            Destroy(this.gameObject);
            //newBloodSpatter = Instantiate(bloodSplatter, transform.position, transform.rotation);
            //Destroy(newBloodSpatter, 3);
        }
    }
    
    void LoadNextLevel()
    {
        // Load a specific scene
        SceneManager.LoadScene("End Credits");

        // Load the next scene in the build settings
        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // SceneManager.LoadScene(currentSceneIndex + 1);
    }
    
    
    
}
