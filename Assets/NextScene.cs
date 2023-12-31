using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    private bool IsOpen = false;
    public GameObject Image;
    private bool inTrigger;
    public int Scene;
    private PlayerController playerController;
    void Start ()
    {
        SceneTransitions.Fadein();
        playerController = FindObjectOfType<PlayerController>();
    }
     
    void Update () {
        if(inTrigger && Input.GetButtonDown("Interact") & IsOpen == false) {
            IEnumerator Pickup()
            {
                IsOpen = true;
                yield return new WaitForSecondsRealtime(0.5f);
                SceneTransitions.Fadeout();
                yield return new WaitForSecondsRealtime(1.5f);
                playerController.SaveAbilities();
                playerController.SaveMANAHEALTH();
                SceneManager.LoadScene(Scene);
                
            }

            StartCoroutine(Pickup());
        } 
    }
 
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            inTrigger = true;
            Image.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Image.SetActive(false);
            inTrigger = false;
        }
    }
}
