using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image slot1;
    public Image slot2;
    public Image slot3;
    //public Image q;
  //  public Image e;
   // public Image r;
    private PlayerController playerController;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.abilities[0] != null) {
            slot1.color = new Color(1, 1, 1, 255);
            slot1.sprite = playerController.abilities[0].data.icon;
        }
        else
        {
            slot1.color = new Color(1, 1, 1, 0);
        }
        if (playerController.abilities[1] != null) {
            slot2.color = new Color(1, 1, 1, 255);
            slot2.sprite = playerController.abilities[1].data.icon;
        }
        else
        {
            slot2.color = new Color(1, 1, 1, 0);
        }
        if (playerController.abilities[2] != null) {
            slot3.color = new Color(1, 1, 1, 255);
            slot3.sprite = playerController.abilities[2].data.icon;
        }
        else
        {
            slot3.color = new Color(1, 1, 1, 0);
        }
    }
}
