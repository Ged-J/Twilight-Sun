using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class bigdoor : MonoBehaviour
{
    private bool IsOpen = false;
    public GameObject text2;
    private bool inTrigger;
    public TMP_Text text;
    private Animator _animator;
    void Start ()
    {
        SceneTransitions.Fadein();
        _animator = GetComponent<Animator>();
    }
     
    void Update ()
    {
        text.text = (3 - GameSession.keys).ToString();
        if (GameSession.keys.Equals(3) && GameSession.boss1 && GameSession.boss2 && GameSession.boss3   )
        {
            text2.SetActive(false);
            Destroy(gameObject);
        }
        if(inTrigger && Input.GetButtonDown("Interact") & IsOpen == false) {
  //
        } 
    }
 
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            inTrigger = true;
            PlayerMovement playerController = FindObjectOfType<PlayerMovement>();
            playerController.canDash = false;
            text2.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            text2.SetActive(false);
            PlayerMovement playerController = FindObjectOfType<PlayerMovement>();
            playerController.canDash = true;
            inTrigger = false;
        }
    }
}
