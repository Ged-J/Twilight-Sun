using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

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
    public Animator animator;

    public float detectionRange = 10f; 
    
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
            // Calculate the distance to the target
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            // Only updates the path if the target is within the detection range
            if (distanceToTarget <= detectionRange)
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
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
    
    void Update() {
        // Update the target regularly to ensure it's always aiming for the current player position.
        target = FindObjectOfType<PlayerController>().transform;
    }


    void FixedUpdate()
    {
        if (path == null)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            animator.SetBool("isMoving", false);
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Calculate direction and force for movement
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        // Apply force for movement
        rb.AddForce(force);

        // Check if the enemy is moving significantly and update the animator
        animator.SetBool("isMoving", force.magnitude > 0.00001f); // Adjust threshold as needed

        // Check the direction to the player
        Vector2 playerDirection = target.position - transform.position;
        if (playerDirection.x > 0)
        {
            // Player is to the right, face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (playerDirection.x < 0)
        {
            // Player is to the left, face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Check if we're close enough to the next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Set the animator parameters for X and Y directions
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
    //        print("step up bruh");
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
    
    void OnDrawGizmosSelected() {
        if (target != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(target.position, 0.5f); // Visualize the current target position
        }
    }
}
