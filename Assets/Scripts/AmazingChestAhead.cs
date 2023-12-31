using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmazingChestAhead : MonoBehaviour
{
    private bool IsOpen = false;
    private bool inTrigger;
    private Animation _animation;
    public SpellPickUp spellPickUp;
    void Start ()
    {
        _animation = GetComponent<Animation>();
    }
     
    void Update () {
        if(inTrigger && Input.GetButtonDown("Interact") & IsOpen == false) {
            IEnumerator Pickup()
            {
                _animation.Play("OpenChest");
                PickUpManager.instance.generateTier1Spell(transform);
                yield return new WaitForSecondsRealtime(0.5f);
                IsOpen = true;
            }

            StartCoroutine(Pickup());
        } 
    }
 
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            inTrigger = false;
        }
    }
    

 
        //if (IsOpen == true)
        //{
        //    if (((other.gameObject == Player) && Input.GetKeyUp(KeyCode.F)) == true)
        //    {
        //        GetComponent<Animation>().Play("CloseDoor");
        //        IsOpen = false;
        //    }
       // }
    }

