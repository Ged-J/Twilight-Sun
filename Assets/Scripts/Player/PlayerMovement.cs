using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDistance = 50f;
    [SerializeField] private LayerMask dashLayerMask;
    
    public Rigidbody2D rb;
    public Animator animator;
    private Camera cam;
    private bool isDashButtonDown;
    public bool canDash = true;
    private float dashTimer = 0f;
    private PlayerController player;
    public float dodgeIFrames;
    private bool dashing;
    
    Vector2 movement;
    Vector3 mousePos;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
        player = GetComponent<PlayerController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Get input from the left stick for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        
        // Get input from the right stick for aiming
        Vector2 aimInput = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));
        
        // Calculate the aim direction based on the right stick input
        if (aimInput.magnitude > 0.1f)
        {
            Vector3 aimDirection = new Vector3(aimInput.x, aimInput.y, 0f);
            mousePos = cam.WorldToScreenPoint(transform.position + aimDirection);
        }
        else
        {
            // Use mouse position if right stick is not being used
            mousePos = Input.mousePosition;
        }
        
        mousePos.z = cam.nearClipPlane;
        var dir = mousePos - cam.WorldToScreenPoint(transform.position);
        
        // Set animator parameters based on movement and aim direction
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        
        // Check for dash input (X/A button on controller)
        if (Input.GetButtonDown("Dash"))
        {
            isDashButtonDown = true;
        }

        if (dashTimer >= 0)
        {
            dashTimer -= Time.deltaTime;
        }
    }

    private IEnumerator IFrames()
    {
        Debug.Log("Player turned invincible!");
        dashing = true;
        animator.SetTrigger("Dodge");
        SFXManager.PlaySFX("Wind1",1f,0.5f);
        //player.currentMana -= 10;
        MPBar.instance.TakeDamage(10);
        player.manaRechargeDelay = 1.5f;
        player.canCast = false;
        CapsuleCollider2D collider2D = player.GetComponent<CapsuleCollider2D>();
        collider2D.enabled = false;

        yield return new WaitForSeconds(dodgeIFrames);
        
        collider2D.enabled = true;
        player.canCast = true;
        dashing = false;
        Debug.Log("Player is no longer invincible!");
    }
    
    private void FixedUpdate()
    {
        if (isDashButtonDown && canDash && dashTimer <= 0)
        {
            if (player.currentMana >= 10)
            {
                Vector2 dashPosition = (Vector2) transform.position + movement * dashDistance;

                RaycastHit2D raycastHit2D =
                    Physics2D.Raycast(transform.position, movement, dashDistance, dashLayerMask);
                if (raycastHit2D.collider != null)
                {
                    dashPosition = raycastHit2D.point;
                }

                StartCoroutine(IFrames());
                rb.MovePosition(dashPosition);
                dashTimer = 0.8f;
                isDashButtonDown = false;
            }
            else
            {
                isDashButtonDown = false;
            }
        }
        else
        {
            isDashButtonDown = false;
            if (!dashing)
            {
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
