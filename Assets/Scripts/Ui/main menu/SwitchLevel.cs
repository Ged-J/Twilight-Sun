using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour
{
    public GameObject card;
    public GameObject cutscene;
    public PlayableDirector director;

    private void Start()
    {
        cutscene.SetActive(false);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (card.activeInHierarchy) return;
        cutscene.SetActive(true);
        director.Play();
    }

    public void SwitchScene()
    {
        SceneManager.LoadScene("OutdoorLevel");
    }
}
