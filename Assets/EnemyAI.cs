using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    private Transform target;
    public float speed = 20f;
    public float nextWaypointDistance = 3f;
    public GameObject slashGO;
    public GameObject projectileGO;
    public int slashDamage = 10;
    public int projectileDamage = 20;
    public bool isRanged = false;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool isAttacking = false;
    bool canAttack = true;
    Vector2 direction;

    Seeker seeker;
    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && !isAttacking && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        animator.SetFloat("X", (target.position.x - transform.position.x));
        animator.SetFloat("Y", (target.position.y - transform.position.y));
    }

    void attack()
    {
        isAttacking = true;
        if (!isRanged) 
        { 
            StartCoroutine("MeleeAttack");
        } else
        {
            animator.SetBool("isInRange", true);
            StartCoroutine("RangedAttack");
            animator.SetBool("isInRange", true);
        }
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(.5f);
        print("enemy melee attack");
        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newSlash = Instantiate(slashGO, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle));
        newSlash.GetComponent<slash>().Setup(slashDamage);
        Destroy(newSlash, 1);
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }

    IEnumerator RangedAttack()
    {
        yield return new WaitForSeconds(.5f);
        print("Ranged enemy attack");
        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var newFireBall = Instantiate(projectileGO, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle));
        newFireBall.GetComponent<EnemyProjectile>().Setup(projectileDamage, direction, isRanged);
        yield return new WaitForSeconds(.5f);
        isAttacking = false;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        print("step the fuck up bruh");
    //        attack();
    //    }
    //}
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag == "Player" && canAttack)
        {
            print("step up bruh");
            attack();
            StartCoroutine("TimeAttack");
        }
    }

    IEnumerator TimeAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }
}
