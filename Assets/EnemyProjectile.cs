using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject newHitEffect;
    PlayerController player;
    int damage;
    float projectileSpeed = 1f;
    Vector3 shootDir;
    bool isRanged;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("you got got");
            player.TakeDamage(damage);
            print(player.currentHealth);
            newHitEffect = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(newHitEffect, 3);
            Destroy(this.gameObject);
        }
    }

    public void Setup(int damage, Vector3 shootDir, bool isRanged)
    {
        player = FindObjectOfType<PlayerController>();
        this.damage = damage;
        this.shootDir = shootDir;
        this.isRanged = isRanged;
        Destroy(this.gameObject, 10);
    }

    private void Update()
    {
        if (!isRanged)
            return;

        transform.position += shootDir * projectileSpeed * Time.deltaTime;
    }
}
