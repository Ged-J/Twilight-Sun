using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellPickUp : MonoBehaviour
{
    private GameObject Player;
    private PlayerController _playerController;
    private bool picked = false;
    private GameObject UI;
    private UICollection _uiCollection;
    private bool inTrigger;
    private PopupUI _popupUI;
    private GameObject oldScroll;
    private GameObject newScroll;
    public Ability spell;

    public SpellPickUp(Ability spell)
    {
        this.spell = spell;
    }
    
    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _playerController = Player.GetComponent<PlayerController>();
        UI = GameObject.FindGameObjectWithTag("PopUpUI");
        _popupUI = UI.GetComponent<PopupUI>();
        _uiCollection = UI.GetComponent<UICollection>();
    }
    
    public float holdTime;
    private float timer = 0.0f;
    private bool pressingq = false;
    private bool pressinge = false;
    private bool pressingr = false;
    void Update () {
        if (!picked)
        {
            if (inTrigger)
            {
                if (spell.data.isHealing)
                {
                    _popupUI.Spell1.SetActive(true);
                    _popupUI.Spell2.SetActive(false);
                    _popupUI.Spell3.SetActive(false);
                    _popupUI.healdmgtext.text = "Base Healing: ";
                }
                else
                {
                    _popupUI.Spell1.SetActive(false);
                    _popupUI.Spell2.SetActive(true);
                    _popupUI.Spell3.SetActive(true);
                    _popupUI.healdmgtext.text = "Base Damage: ";
                }
                _popupUI.icon.sprite = spell.data.icon;
                _popupUI.name.text = spell.data.id;
                _popupUI.damage.text = spell.data.baseDamage.ToString();
                _popupUI.cost.text = spell.data.manaCost.ToString();
                _uiCollection.Show();
            }
        }

        //Q SPELL
        if (inTrigger && Input.GetKeyDown(KeyCode.Q) && !picked && spell.data.isHealing)
        {
            if (_playerController.abilities[0] == null)
            {
                IEnumerator Pickup()
                {
                    _playerController.abilities[0] = spell;
                    SFXManager.PlaySFX("pickup");
                    timer = 0f;
                    _uiCollection.Hide();
                    _popupUI.slot1.fillAmount = 0f;
                    picked = true;
                    Debug.Log("picked up the scroll wowwwwwwwwwwwwwww");
                    yield return new WaitForSecondsRealtime(1f);
                    //play a sick animation/sfx
                    Destroy(gameObject);
                }
                StartCoroutine(Pickup());
            }
            else
            {
                timer = Time.time;
                pressingq = true;
            }
        }
        if (pressingq)
        {
            _popupUI.q.sprite = Resources.Load<Sprite>("UI/Keys/QPressed");
            _popupUI.slot1.fillAmount += 1f/1f * Time.deltaTime;
        }
        else
        {
            _popupUI.q.sprite = Resources.Load<Sprite>("UI/Keys/Q");
            _popupUI.slot1.fillAmount = 0f;
        }
        if (inTrigger && Input.GetKeyUp(KeyCode.Q) && !picked && spell.data.isHealing)
        {
            pressingq = false;
            timer = Time.time - timer;
            Debug.Log("Pressed for : " + timer + " Seconds");
            if (timer >= holdTime)
            {
                IEnumerator Pickup()
                {
                    oldScroll = PickUpManager.instance.getScroll(_playerController.abilities[0]);
                    
                    _playerController.abilities[0] = spell;
                    SFXManager.PlaySFX("pickup");
                    timer = 0f;
                    _popupUI.slot1.fillAmount = 0f;
                    _uiCollection.Hide();
                    picked = true;
                    Debug.Log("picked up the scroll wowwwwwwwwwwwwwww");
                    
                    yield return new WaitForSecondsRealtime(0.5f);
                    
                    newScroll = Instantiate(oldScroll, transform.position, transform.rotation);
                    Destroy(gameObject);
                    
                    //play a sick animation/sfx
                    
                }
                StartCoroutine(Pickup());
            }
            else
            {
                timer = 0f;
                _popupUI.slot1.fillAmount = 0f;
            }
        }
        
        //E SPELL
        if (inTrigger && Input.GetKeyDown(KeyCode.E) && !picked && !spell.data.isHealing)
        {
            if (_playerController.abilities[1] == null)
            {
                IEnumerator Pickup()
                {
                    _playerController.abilities[1] = spell;
                    SFXManager.PlaySFX("pickup");
                    _uiCollection.Hide();
                    _popupUI.slot2.fillAmount = 0f;
                    timer = 0f;
                    picked = true;
                    Debug.Log("picked up the scroll wowwwwwwwwwwwwwww");
                    yield return new WaitForSecondsRealtime(1f);
                    //play a sick animation/sfx
                    Destroy(gameObject);
                }
                StartCoroutine(Pickup());
            }
            else
            {
                timer = Time.time;
                pressinge = true;
            }
        }
        if (pressinge)
        {
            _popupUI.e.sprite = Resources.Load<Sprite>("UI/Keys/EPressed");
            _popupUI.slot2.fillAmount += 1f/1f * Time.deltaTime;
        }
        else
        {
            _popupUI.e.sprite = Resources.Load<Sprite>("UI/Keys/E");
            _popupUI.slot2.fillAmount = 0f;
        }
        if (inTrigger && Input.GetKeyUp(KeyCode.E) && !picked && !spell.data.isHealing)
        {
            pressinge = false;
            timer = Time.time - timer;
            Debug.Log("Pressed for : " + timer + " Seconds");
            if (timer >= holdTime)
            {
                IEnumerator Pickup()
                {
                    oldScroll = PickUpManager.instance.getScroll(_playerController.abilities[1]);
                    
                    _playerController.abilities[1] = spell;
                    SFXManager.PlaySFX("pickup");
                    timer = 0f;
                    _popupUI.slot2.fillAmount = 0f;
                    _uiCollection.Hide();
                    picked = true;
                    Debug.Log("picked up the scroll wowwwwwwwwwwwwwww");
                    yield return new WaitForSecondsRealtime(0.5f);
                    newScroll = Instantiate(oldScroll, transform.position, transform.rotation);
                    Destroy(gameObject);
                    
                    //play a sick animation/sfx
                    
                }
                StartCoroutine(Pickup());
            }
            else
            {
                timer = 0f;
                _popupUI.slot2.fillAmount = 0f;
            }
        }
        
        //R SPELL
        if (inTrigger && Input.GetKeyDown(KeyCode.R) && !picked && !spell.data.isHealing)
        {
            if (_playerController.abilities[2] == null)
            {
                IEnumerator Pickup()
                {
                    _playerController.abilities[2] = spell;
                    SFXManager.PlaySFX("pickup");
                    _uiCollection.Hide();
                    timer = 0f;
                    _popupUI.slot3.fillAmount = 0f;
                    picked = true;
                    Debug.Log("picked up the scroll wowwwwwwwwwwwwwww");
                    yield return new WaitForSecondsRealtime(0.5f);
                    
                    //play a sick animation/sfx
                    Destroy(gameObject);
                }
                StartCoroutine(Pickup());
            }
            else
            {
                timer = Time.time;
                pressingr = true;
            }
        }
        if (pressingr)
        {
            _popupUI.r.sprite = Resources.Load<Sprite>("UI/Keys/RPressed");
            _popupUI.slot3.fillAmount += 1f/1f * Time.deltaTime;
        }
        else
        {
            _popupUI.r.sprite = Resources.Load<Sprite>("UI/Keys/R");
            _popupUI.slot3.fillAmount = 0f;
        }
        if (inTrigger && Input.GetKeyUp(KeyCode.R) && !picked && !spell.data.isHealing)
        {
            pressingr = false;
            timer = Time.time - timer;
            Debug.Log("Pressed for : " + timer + " Seconds");
            if (timer >= holdTime)
            {
                IEnumerator Pickup()
                {
                    oldScroll = PickUpManager.instance.getScroll(_playerController.abilities[2]);
                    Debug.Log(oldScroll.name);
                    _playerController.abilities[2] = spell;
                    SFXManager.PlaySFX("pickup");
                    timer = 0f;
                    _popupUI.slot3.fillAmount = 0f;
                    _uiCollection.Hide();
                    picked = true;
                    Debug.Log("picked up the scroll wowwwwwwwwwwwwwww");
                    yield return new WaitForSecondsRealtime(0.5f);
                    newScroll = Instantiate(oldScroll, transform.position, transform.rotation);
                    Destroy(gameObject);
                    
                    //play a sick animation/sfx
                    
                }
                StartCoroutine(Pickup());
            }
            else
            {
                timer = 0f;
                _popupUI.slot3.fillAmount = 0f;
            }
        }
        
    }
 
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerController.canCast = false;
            inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            pressingq = false;
            pressinge = false;
            pressingr = false;
            _popupUI.slot1.fillAmount = 0f;
            _popupUI.slot2.fillAmount = 0f;
            _popupUI.slot3.fillAmount = 0f;
            timer = 0f;
            _playerController.canCast = true;
            inTrigger = false;
            _uiCollection.Hide();
        }
    }
}
