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
    }

    private void Update()
    {
        UpdateSlider();
        UpdatePosition();
    }

    private void UpdateSlider()
    {
        float fillF = frontHealthbar.fillAmount;
        float fillB = backHealthbar.fillAmount;
        float hFraction = (float)enemyController.health / enemyController.maxHealth;

        // Existing logic for updating health bar...

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