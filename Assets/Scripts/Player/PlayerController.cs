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
    private float fireDelta = 0.5F;
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
        //might need to shoot right to work https://cdn.discordapp.com/attachments/897093586816991242/952340518619185172/unknown.png
        //world or local can be interchanged dependant on the effect wanted
        mousePosTest = cam.ScreenToWorldPoint(Input.mousePosition);
        lookdir = mousePosTest - rb.position;
        angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg;

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
            //print("yes");
            nextFire = myTimeBasicAttack + fireDelta;
            if (basicAttackProjectile != null)
            {
                newBasicAttackProjectile = Instantiate(basicAttackProjectile, transform.position, transform.rotation);
            }
            
            newBasicAttackProjectile.transform.GetComponent<BasicAttackProjectile>().Setup(shootDir);
            
            //if fails uncomment this line and add rigidbody to fireball
            //newBasicAttackProjectile.GetComponent<Rigidbody2D>().rotation = angle;
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
        yield return new WaitForSeconds(.5f);
        SceneTransitions.Fadeout();
        SavedPositionManager.savedPositions.Clear();
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
    
    IEnumerator DamageAnim()
    {
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("isDamaged", false);
    }

    public void Heal(int heal)
    {
        print("cock");
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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pit"))
        {
            SceneManager.LoadScene("Main_Menu"); 
        }
    }
    
    
}
