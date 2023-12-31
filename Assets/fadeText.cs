using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fadeText : MonoBehaviour
{
    private float disappearTimer;
    private Color textColor;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        textColor = textMesh.color;
        disappearTimer = .2f;
    }

    private void Update()
    {
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
