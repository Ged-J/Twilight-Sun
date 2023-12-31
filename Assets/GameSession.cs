using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static int keys;
    public static Ability[] abilities;
    public static float currentHealth, currentMana;
    public static bool boss1 = false;
    public static bool boss2 = false;
    public static bool boss3 = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        Debug.Log("game session awoken");
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public static Ability[] HasAbilities()
    {
        if (abilities != null)
        {
            return abilities;
        }
        return null;
    }

    public static void StoreAbilties(Ability[] abilitieS)
    {
        abilities = abilitieS;
    }

    public static void StoreManaHealth(float cHP, float cMana)
    {
        currentHealth = cHP;
        currentMana = cMana;
    }

    public static float RestoreCurrentHealth()
    {
        return currentHealth;
    }

    public static float RestoreCurrentMana()
    {
        return currentMana;
    }

    public static void addKey()
    {
        keys++;
    }
}
