using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float currentHealth, currentMana;

    public float maxHealth = 100, maxMana = 100;
    public GameObject basicAttackProjectile;
    public Camera cam;
    public Rigidbody2D rb;

    Vector2 lookdir;
    Vector2 mousePosTest;
    float angle;
    Vector3 shootDir;

    //Basic attack
    private float myTimeBasicAttack = 0.0F;
    private float nextFire = 0.5F;
    private float fireDelta = 1.0F;
    private GameObject newBasicAttackProjectile;
    private Vector3 mousePos;

    //Teleport
    private float myTimeTeleport = 0.0F;
    private float nextTeleport = 1F;
    private float teleportDelta = 1F;

    //Magic spells
    public float timeBetweenCasts = 0.25f;

    private bool castingMagic = false;
    public float manaRechargeRate = 10f;
    public float manaRechargeDelay;
    private float currentCastTimer;
    public bool canCast = true;

    public Ability[] abilities;
    public Ability newAbilityInstance;
    int spellslot;

    private PlayerMovement playerMovement;
    private Animator animator;

    private Vector2 aimInput;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        if (GameSession.HasAbilities() != null)
        {
            abilities = GameSession.HasAbilities();
        }

        if (currentHealth != maxHealth)
        {
            currentHealth = GameSession.RestoreCurrentHealth();
        }
        else
        {
            currentHealth = maxHealth;
        }

        if (currentMana != maxMana)
        {
            currentMana = GameSession.RestoreCurrentMana();
        }
        else
        {
            currentMana = maxMana;
        }

        // Find and assign the main camera
        cam = Camera.main;
    }

    public void SaveAbilities()
    {
        GameSession.StoreAbilties(abilities);
    }

    public void SaveMANAHEALTH()
    {
        GameSession.StoreManaHealth(currentHealth, currentMana);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the right stick for aiming
        aimInput = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));

        // Calculate the aim direction based on the right stick input
        if (aimInput.magnitude > 0.1f)
        {
            Vector3 aimDirection = new Vector3(aimInput.x, aimInput.y, 0f);
            mousePos = cam.WorldToScreenPoint(transform.position) + aimDirection;
        }
        else
        {
            // Use mouse position if right stick is not being used
            mousePos = Input.mousePosition;
        }

        mousePos.z = cam.nearClipPlane;
        var dir = mousePos - cam.WorldToScreenPoint(transform.position);
        shootDir = (dir - transform.position).normalized;

        myTimeBasicAttack = myTimeBasicAttack + Time.deltaTime;
        myTimeTeleport = myTimeTeleport + Time.deltaTime;

        //world or local can be interchanged dependant on the effect
        mousePosTest = cam.ScreenToWorldPoint(mousePos);
        lookdir = mousePosTest - rb.position;
        angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;

        // Gets a reference to SpriteRenderer component 
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (lookdir.x >= 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else
        {
            spriteRenderer.flipX = true; // Face left
        }

        //first spell slot
        if (!castingMagic & canCast)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("SpellSlot1"))
            {
                if (abilities[0] == null)
                {
                    return;
                }
                spellslot = 0;
                var hasEnoughMana = currentMana - abilities[0].data.manaCost >= 0f;
                castingMagic = true;
                currentCastTimer = 0;
                if (hasEnoughMana)
                {
                    MPBar.instance.TakeDamage(abilities[0].data.manaCost);
                    CastSpell();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("SpellSlot2"))
            {
                if (abilities[1] == null)
                {
                    return;
                }
                spellslot = 1;
                var hasEnoughMana = currentMana - abilities[1].data.manaCost >= 0f;
                castingMagic = true;
                currentCastTimer = 0;
                if (hasEnoughMana)
                {
                    MPBar.instance.TakeDamage(abilities[1].data.manaCost);
                    CastSpell();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("SpellSlot3"))
            {
                if (abilities[2] == null)
                {
                    return;
                }
                spellslot = 2;
                var hasEnoughMana = currentMana - abilities[2].data.manaCost >= 0f;
                castingMagic = true;
                currentCastTimer = 0;
                if (hasEnoughMana)
                {
                    MPBar.instance.TakeDamage(abilities[2].data.manaCost);
                    CastSpell();
                }
            }
        }

        if (castingMagic)
        {
            currentCastTimer += Time.deltaTime;

            if (currentCastTimer > timeBetweenCasts) castingMagic = false;
        }

        if (castingMagic)
        {
            manaRechargeDelay = 1.5f;
        }
        if (manaRechargeDelay >= 0)
        {
            manaRechargeDelay -= Time.deltaTime;
        }

        if (currentMana < maxMana && !castingMagic && !Input.GetKey(KeyCode.Q))
        {
            if (manaRechargeDelay <= 0)
            {
                currentMana += (manaRechargeRate * Time.deltaTime);
            }
            if (currentMana > maxMana) currentMana = maxMana;
        }

        //Basic attack
        if ((Input.GetButtonDown("BasicAttack") || Input.GetButtonDown("joystick button 0")) && myTimeBasicAttack > nextFire & canCast)
        {
            nextFire = myTimeBasicAttack + fireDelta;
            if (basicAttackProjectile != null)
            {
                newBasicAttackProjectile = Instantiate(basicAttackProjectile, transform.position, transform.rotation);
            }

            newBasicAttackProjectile.transform.GetComponent<BasicAttackProjectile>().Setup(shootDir);

            newBasicAttackProjectile.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);

            nextFire = nextFire - myTimeBasicAttack;
            myTimeBasicAttack = 0.0F;
        }
    }

    private void CastSpell()
    {
        if (spellslot != 0)
        {
            newAbilityInstance = Instantiate(abilities[spellslot], transform.position, transform.rotation);
            newAbilityInstance.transform.GetComponent<Ability>().Setup(shootDir, spellslot);
            newAbilityInstance.transform.rotation =
                Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        }
        else
        {
            newAbilityInstance = Instantiate(abilities[spellslot], transform.position, transform.rotation);
            newAbilityInstance.transform.parent = transform;
            newAbilityInstance.transform.GetComponent<Ability>().Setup(shootDir, spellslot);
            Heal(newAbilityInstance.data.baseHealing);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        HPBar.instance.TakeDamage(dmg);
        StartCoroutine("DamageAnim");
        if (currentHealth <= 0)
        {
            StartCoroutine(death());
        }
    }

    IEnumerator death()
    {
        SavedPositionManager.savedPositions.Clear();

        yield return new WaitForSeconds(1f);

        // Find the RoomContentGenerator and call Regenerate
        var roomContentGenerator = FindObjectOfType<RoomContentGenerator>();
        if (roomContentGenerator != null)
        {
            roomContentGenerator.Regenerate();
        }

        DifficultyManager.instance.PlayerDied();

        // Recount enemies after regeneration
        var enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            enemyManager.RecountEnemies();
        }
    }

    IEnumerator DamageAnim()
    {
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("isDamaged", false);
    }

    public void Heal(int heal)
    {
        print("healed");
        if (currentHealth < maxHealth)
        {
            currentHealth += heal;
            HPBar.instance.RestoreHP(heal);
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private bool isInCombat = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pit"))
        {
            SceneManager.LoadScene("Main_Menu");
        }
    }
}