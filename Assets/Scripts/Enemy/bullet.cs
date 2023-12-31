using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float moveSpeed;
    private Transform player;
    private Vector2 target;
    public Rigidbody2D rigidbody;
    private Vector3 direction;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);

        direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rigidbody.rotation = angle;
        direction.Normalize();
    }

    private void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        transform.position += direction * Time.deltaTime * moveSpeed;

        //if (transform.position.x == target.x && transform.position.y == target.y)       
        
            DestroyBullet();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            collision.GetComponent<PlayerController>().TakeDamage(10);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject, 5);
    }
}
