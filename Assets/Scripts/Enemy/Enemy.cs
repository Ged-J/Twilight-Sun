using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject slash;
    public Transform bulletSpawn;
    private Transform player;
    public float fireRate;
    private float nextFire;
    public bool canMove, melee, ranged, shouldRotate;
    private bool inAttackRange;
    private Rigidbody2D rb;
    public float advance;
    public float retreat;
    public float moveSpeed;
    private Vector2 movement;
    private Vector3 direction;
    public bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;
        

        if (!ranged && !canMove && !melee)
        {
            return;
        }

        if (!ranged && canMove && melee && slash != null)
        {
            EnemySpacing();
            return;
        }

        if (ranged && canMove && !melee && bullet != null)
        {
            CheckTimeToFire();
            EnemySpacing();
            return;
        }

        if (ranged && !canMove && !melee && bullet != null)
        {
            CheckTimeToFire();
            return;
        }

        if (!ranged && canMove && melee)
        {
            EnemySpacing();
            return;
        }
        
    }

    private void shoot()
    {
        Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
    }

    private void Slash()
    {
        
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        print("yes");
        yield return new WaitForSeconds(3);
        direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var slashObject = Instantiate(slash, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle));
        Destroy(slashObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inAttackRange = true;
            Slash();
        }
    }

    void CheckTimeToFire()
    {
        nextFire -= Time.deltaTime;

        if (nextFire <= 0)
        {
            nextFire = fireRate;
            shoot();
        }
    }

    void EnemySpacing()
    {
        if (Vector2.Distance(transform.position, player.position) > advance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            isMoving = true;
        }
        else if (Vector2.Distance(transform.position, player.position) < advance && Vector2.Distance(transform.position, player.position) > retreat)
        {
            transform.position = transform.position;
            isMoving = false;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreat)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
            isMoving = true;

        }
    }
}
