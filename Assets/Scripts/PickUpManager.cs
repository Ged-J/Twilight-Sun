using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance;
    [SerializeField] public GameObject[] Tier1Spells;
    [SerializeField] public GameObject[] Tier2Spells;
    [SerializeField] public GameObject[] Tier3Spells;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private SpellPickUp _spellPickUp;
    // Start is called before the first frame update
    public GameObject getScroll(Ability i)
    {
        foreach(GameObject gameObject in Tier1Spells)
        {
            SpellPickUp game = gameObject.GetComponent<SpellPickUp>();
            if (game.spell.Equals(i))
            {
                return gameObject;
            }
            
        }
        foreach(GameObject gameObject in Tier2Spells)
        {
            SpellPickUp game = gameObject.GetComponent<SpellPickUp>();
            if (game.spell.Equals(i))
            {
                return gameObject;
            }
            
        }
        foreach(GameObject gameObject in Tier3Spells)
        {
            SpellPickUp game = gameObject.GetComponent<SpellPickUp>();
            if (game.spell.Equals(i))
            {
                return gameObject;
            }
            
        }
        return null;
    }

    
    public void generateTier1Spell(Transform spawnLocation)
    {
        Instantiate(Tier1Spells[Random.Range(0,Tier1Spells.Length)],spawnLocation);
    }
    public void generateTier2Spell(Transform spawnLocation)
    {
        Instantiate(Tier1Spells[Random.Range(0,Tier2Spells.Length)], spawnLocation);
    }
    public void generateTier3Spell(Transform spawnLocation)
    {
        Instantiate(Tier1Spells[Random.Range(0,Tier3Spells.Length)], spawnLocation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
