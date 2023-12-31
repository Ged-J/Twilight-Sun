using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollection : MonoBehaviour
{
    [SerializeField]
    private GameObject[] elements;
    private bool visible;

    public bool Visible => visible;

    public void Show()
    {
        foreach (GameObject item in elements)
        {
            item.SetActive(true);
            visible = true;
        }
    }

    public void Hide()
    {
        foreach (GameObject item in elements)
        {
            item.SetActive(false);
            visible = false;
        }
    }
}