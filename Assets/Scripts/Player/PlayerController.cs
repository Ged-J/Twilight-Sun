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
    
    /*public AudioSource backgroundMusicSource;
    public AudioSource enemyMusicSource;*/
    
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
        } else
        {
            currentHealth = maxHealth;
        }

        if (currentMana != maxMana)
        {
            currentMana = GameSession.RestoreCurrentMana();
        } else
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
        mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        var dir = mousePos - cam.WorldToScreenPoint(transform.position);
        shootDir = (dir - transform.position).normalized;
        myTimeBasicAttack = myTimeBasicAttack + Time.deltaTime;
        myTimeTeleport = myTimeTeleport + Time.deltaTime;
        //world or local can be interchanged dependant on the effect wanted
        mousePosTest = cam.ScreenToWorldPoint(Input.mousePosition);
        lookdir = mousePosTest - rb.position;
        angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;
        
        // Get a reference to your SpriteRenderer component (do this in Awake or Start)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Then, in the Update method, where you're checking the mouse position:
        if (lookdir.x >= 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else
        {
            spriteRenderer.flipX = true; // Face left
        }
        
        //first spell slot
        if (!castingMagic & canCast) {
            if (Input.GetKeyDown(KeyCode.Q))
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
                    //currentMana -= abilities[0].data.manaCost;
                    MPBar.instance.TakeDamage(abilities[0].data.manaCost);
                    CastSpell();
                }
            } else if (Input.GetKeyDown(KeyCode.E))
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
                    //currentMana -= abilities[1].data.manaCost;
                    MPBar.instance.TakeDamage(abilities[1].data.manaCost);
                    CastSpell();
                }
            } else if (Input.GetKeyDown(KeyCode.R))
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
                    //currentMana -= abilities[2].data.manaCost;
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
            if (manaRechargeDelay <= 0) {
                currentMana += (manaRechargeRate * Time.deltaTime);
            }
            if (currentMana > maxMana) currentMana = maxMana;
        }

        //Basic attack
        if (Input.GetButtonDown("BasicAttack") && myTimeBasicAttack > nextFire & canCast)
        {
            //print("yup");
            nextFire = myTimeBasicAttack + fireDelta;
            if (basicAttackProjectile != null)
            {
                newBasicAttackProjectile = Instantiate(basicAttackProjectile, transform.position, transform.rotation);
            }
            
            newBasicAttackProjectile.transform.GetComponent<BasicAttackProjectile>().Setup(shootDir);
            
            //if fails uncomment this line and add rigidbody to fireball
            /*newBasicAttackProjectile.GetComponent<Rigidbody2D>().rotation = angle;*/
            newBasicAttackProjectile.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);

            // code if animated

            nextFire = nextFire - myTimeBasicAttack;
            myTimeBasicAttack = 0.0F;
        }
    }

    private void CastSpell()
    {
        if (spellslot != 0)
        {
            //print("non healing");
            newAbilityInstance = Instantiate(abilities[spellslot], transform.position, transform.rotation);
            newAbilityInstance.transform.GetComponent<Ability>().Setup(shootDir, spellslot);
            newAbilityInstance.transform.rotation =
                Quaternion.Euler(transform.rotation.x, transform.rotation.y, angle);
        }
        else
        {
            //print("healing");
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
        //print(currentHealth);
        StartCoroutine("DamageAnim");
        if (currentHealth <= 0)
        {
            StartCoroutine(death());
            //Destroy(this.gameObject);
            //Add death animation and transion to checkpoint or some shit here
        }
    }

    IEnumerator death()
    {
        /*yield return new WaitForSeconds(.5f);*/
        // Assuming you want to clear saved positions and other state-related tasks
        SavedPositionManager.savedPositions.Clear();

        yield return new WaitForSeconds(1f);

        // Find the RoomContentGenerator and call Regenerate
        var roomContentGenerator = FindObjectOfType<RoomContentGenerator>();
        if(roomContentGenerator != null)
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

        // Reload the scene or do other necessary cleanup
        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);*/
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
        //print("Current Health : " + currentHealth);
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
        /*else if (collision.gameObject.CompareTag("Enemy"))
        {
            isInCombat = true;
            Debug.Log("Collided with Enemy");

            // Stop the current background music
            /*if (backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Stop();
            }#1#

            // Start playing the enemy's music
            AudioSource enemyAudio = collision.gameObject.GetComponent<AudioSource>();
            if (enemyAudio != null)
            {
                Debug.Log("Enemy Audio Source found");

                if (!enemyAudio.isPlaying)
                {
                    enemyMusicSource.clip = enemyAudio.clip;
                    enemyMusicSource.Play();
                    Debug.Log("Playing Enemy Music");
                }
                else
                {
                    Debug.Log("Enemy music is already playing");
                }
            }
            else
            {
                Debug.Log("No AudioSource found on Enemy");
            }
        }*/
    }
    
    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isInCombat = false;
            // Check if enemy music is playing, stop it and resume background music
            if (enemyMusicSource.isPlaying)
            {
                enemyMusicSource.Stop();
                backgroundMusicSource.Play();
            }
        }
    }*/


}
