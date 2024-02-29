using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MPBar : MonoBehaviour
{
    public static MPBar instance;

    public Image frontHealthbar;
    public Image backHealthbar;
    private PlayerController player;
    private float chipSpeed = 6f;
    private float currentVal, maxVal;
    private float lerpTimer;
    private string currentText, maxText;
    private float timer;    
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text maxHpText;
    // Start is called before the first frame update

    private void Awake()
    {
        //gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
    }

    private void Start()
    {
        instance = this;
        player = FindObjectOfType<PlayerController>();
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
        
        /*if (player == null)
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null) return; // Player still not found, exit the method to avoid errors
    }*/
        
        float fillF = frontHealthbar.fillAmount;
        float fillB = backHealthbar.fillAmount;
        float hFraction = (float) player.currentMana / player.maxMana;
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
        player.currentMana = player.currentMana - value;
        lerpTimer = 0;
    }
    
    public void RestoreMP(int value)
    {
        player.currentMana = player.currentMana + value;
        lerpTimer = 0;
    }
}
