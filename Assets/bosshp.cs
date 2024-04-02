using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class bosshp : MonoBehaviour
{
public static bosshp instance;

public TMP_Text name;
    public Image frontHealthbar;
    public Image backHealthbar;
    private EnemyController enemyController;
    private float chipSpeed = 6f;
    private float currentVal, maxVal;
    private float lerpTimer;
    private string currentText, maxText;
    private float timer;    

    // Start is called before the first frame update

    private void Awake()
    {
        //gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    private void OnEnable()
    {
        instance = this;
        enemyController = FindObjectOfType<EnemyController>();
        if (enemyController != null)
        {
            // First, split the name to remove the "(Clone)" part, if any
            string enemyName = enemyController.gameObject.name.Split(new string[] {"(Clone)"}, System.StringSplitOptions.None)[0].Trim();
        
            // Next, replace underscores with an empty string to remove them
            enemyName = enemyName.Replace("_", " ");
        
            // Assign the processed name to the text component
            name.text = enemyName;
        }
    }

    // Update is called once per frame
    private void Update()
    {
       // hpText.text = player.getHealth().ToString();
       // maxHpText.text = _player.getMaxHealth().ToString();
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        float fillF = frontHealthbar.fillAmount;
        float fillB = backHealthbar.fillAmount;
        float hFraction = (float) enemyController.health / enemyController.maxHealth;
        if (fillB > hFraction)
        {
            frontHealthbar.fillAmount = hFraction;
            Color color = new Color32(161,161,4,255);
            backHealthbar.color = color;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthbar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthbar.color = Color.clear;
            backHealthbar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthbar.fillAmount = Mathf.Lerp(fillF, backHealthbar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(int value)
    {
        enemyController.health = enemyController.health - value;
        lerpTimer = 0;
    }
    
    public void RestoreHP(int value)
    {
        enemyController.health = enemyController.health + value;
        lerpTimer = 0;
    }
}

