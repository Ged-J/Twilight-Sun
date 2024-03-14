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
    
    private bool isStuck = false;
    private float stuckTimer = 0f;
    private Vector2 lastPosition;
    private float checkStuckInterval = 0.5f; // Check every half second

    public float separationDistance = 1.5f; // The distance to maintain from other enemies
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().transform;
        
        AdjustDamageBasedOnDifficulty();
        DifficultyManager.instance.OnDifficultyChanged += AdjustDamageBasedOnDifficulty;

        InvokeRepeating("UpdatePath", 0f, .5f);
    }
    
    private void OnDestroy()
    {
        DifficultyManager.instance.OnDifficultyChanged -= AdjustDamageBasedOnDifficulty;
    }

    private void AdjustDamageBasedOnDifficulty()
    {
        int difficultyScore = DifficultyManager.instance.GetCurrentDifficultyScore();
        // Adjust slashDamage and projectileDamage based on the difficulty score. Adjust formula as needed.
        slashDamage = 10 + (difficultyScore * 2); // Example: Starting at 10 damage, plus 2 for each difficulty level
        projectileDamage = 20 + (difficultyScore * 4); // Adjust as needed
    }

    void UpdatePath() {
        if (seeker.IsDone() && !isAttacking && target != null) {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget <= detectionRange) {
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

        /*// Set the animator parameters for X and Y directions
        animator.SetFloat("X", (target.position.x - transform.position.x));
        animator.SetFloat("Y", (target.position.y - transform.position.y));*/
        
        // Add simple forward obstacle detection to trigger path recalculation
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Midground")); // Adjust the distance as needed
        if (hit.collider != null) {
            // Detected an obstacle, trigger path recalculation
            UpdatePath();
        }
        
        // Check if we have moved since the last check
        if ((Vector2)transform.position == lastPosition) {
            stuckTimer += Time.fixedDeltaTime;
        } else {
            stuckTimer = 0;
            isStuck = false;
        }

        // Update the lastPosition for the next check
        lastPosition = transform.position;

        // If we've been stuck for more than the interval, consider us stuck
        if (stuckTimer > checkStuckInterval) {
            isStuck = true;
        }

        // If stuck, try to recalculate the path
        if (isStuck) {
            UpdatePath();
            isStuck = false; // Reset stuck status after attempting to find a new path
        }
        
        SeparateFromOtherEnemies();
        
        DetectAndAvoidWalls();
        
    }
    
    void SeparateFromOtherEnemies()
    {
        // Find all enemies within the separationDistance
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, separationDistance, LayerMask.GetMask("Enemy"));
        Vector2 separationForce = Vector2.zero;
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != gameObject && hit is BoxCollider2D) // Ignore self
            {
                Vector2 awayFromEnemy = transform.position - hit.transform.position;
                // Adjust the force based on distance (closer enemies push harder)
                separationForce += awayFromEnemy.normalized / awayFromEnemy.magnitude;
            }
        }

        // Apply a force that separates this enemy from others
        if (separationForce != Vector2.zero)
        {
            rb.AddForce(separationForce * speed);
        }
    }
    
    void DetectAndAvoidWalls()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, LayerMask.GetMask("Midground"));
        if (hit.collider != null)
        {
            // Wall detected between enemy and player
            Debug.DrawLine(transform.position, hit.point, Color.red);

            // Force a path update if we hit a wall
            UpdatePath();
        }
        else
        {
            Debug.DrawRay(transform.position, direction * detectionRange, Color.green);
        }
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
        /*print("enemy melee attack");*/
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
        /*print("Ranged enemy attack");*/
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
            /*print("step up bruh");*/
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
