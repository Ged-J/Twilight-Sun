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
        mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        var dir = mousePos - cam.WorldToScreenPoint(transform.position);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        //if want wasd animations
        //animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        //animator.SetFloat("Speed", movement.sqrMagnitude);
        
        //mouse animations
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        
        //dodge
        if (Input.GetButtonDown("Dodge"))
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

                //add mana cost, iframe, animaton and sfx :)
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
