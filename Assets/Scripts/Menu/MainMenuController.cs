using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private bool hasPlayed;
    private bool ready;
    public Animation ui;

    void Start()
    {
        MusicManager.SetMusic("Retro_Forest_David_Fesliyan");
        //AmbienceManager.SetAmbience("Nature-sounds-winter-atmosphere");
        Cursor.lockState = CursorLockMode.None;
        //audioSource = this.GetComponent<AudioSource>();
        //audioSource.clip = mainMenu;
        //audioSource.Play();
        StartCoroutine(r());

        IEnumerator r()
        {
            SceneTransitions.Fadein();
            yield return new WaitForSecondsRealtime(0.1f);
            ready = true;
        }
    }

    public void BeginButton()
    {
        if (hasPlayed) return;
        hasPlayed = true;
        StartCoroutine(Sus());
       
        IEnumerator Sus()
        {
           
            SFXManager.PlaySFX("Notification 2",1f,0.5f);
            yield return new WaitForSeconds((float) 0.5f);
            ui.Play("UiFade");
            ui.Play("PlayerMoveInMainMenu");
            MusicManager.FadeOut(3f, 0);
            yield return new WaitForSeconds((float) 1f);
            SceneTransitions.Fadeout();
            yield return new WaitForSeconds((float) 3f);
            SceneManager.LoadScene(1);
            //ani.Play();
        }
    }

    private void Update()
    {
        //if (ready)
       // {
        //    if (Input.anyKey)
         //   {
         //       BeginButton();
        //    }
       // }
    }

}