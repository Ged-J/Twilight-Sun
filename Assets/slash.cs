using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slash : MonoBehaviour
{
    PlayerController player;
    int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            /*print("you got got");*/
            player.TakeDamage(damage);
            /*print(player.currentHealth);*/
        }
    }

    public void Setup(int damage)
    {
        player = FindObjectOfType<PlayerController>();
        this.damage = damage;
    }
}
