using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public TMP_Text nameText;
    public Image frontHealthbar;
    public Image backHealthbar;
    private EnemyController enemyController;
    private float chipSpeed = 6f;
    private float lerpTimer;

    public void Setup(EnemyController controller)
    {
        enemyController = controller;
        nameText.text = enemyController.gameObject.name;

        // Subscribe to the OnHealthChanged event
        enemyController.OnHealthChanged += UpdateHealthDisplay;
        // Initialize the health bar display
        UpdateHealthDisplay(enemyController.health, enemyController.maxHealth);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if(enemyController != null) 
        {
            enemyController.OnHealthChanged -= UpdateHealthDisplay;
        }
    }

    private void UpdateHealthDisplay(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        frontHealthbar.fillAmount = healthPercentage;
        // Add logic to adjust backHealthbar if using a delayed effect
        backHealthbar.fillAmount = Mathf.Lerp(backHealthbar.fillAmount, healthPercentage, lerpTimer * chipSpeed);
        lerpTimer += Time.deltaTime;
    }

    private void Update()
    {
        UpdateSlider();
        UpdatePosition();
    }

    private void UpdateSlider()
    {
        if(backHealthbar.fillAmount > frontHealthbar.fillAmount)
        {
            lerpTimer += Time.deltaTime;
            float fill = Mathf.Lerp(backHealthbar.fillAmount, frontHealthbar.fillAmount, lerpTimer * chipSpeed);
            backHealthbar.fillAmount = fill;
        }
        else
        {
            lerpTimer = 0;
        }
    }

    private void UpdatePosition()
    {
        if (enemyController != null)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(enemyController.transform.position);
            transform.position = screenPosition + new Vector2(0, 50); // Adjust the Y offset as needed
        }
    }

    public void TakeDamage(int value)
    {
        enemyController.TakeDamage(value);
        lerpTimer = 0;
    }

    public void RestoreHP(int value)
    {
        enemyController.health += value;
        lerpTimer = 0;
    }
}