using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class AbilityData : ScriptableObject
{
    public string id;
    public Sprite icon;
    public bool isHealing;
    public int baseDamage;
    public int baseHealing;
    public bool hasHealing;
    public int elementalDamage;
    [Range(0f, 100f)]
    public float critChance;
    public int damageMultiplier;
    public int manaCost;
    public int cooldown;
    public float spellRadius = 0.5f;
    public float lifeTime;
    public float projectileSpeed;
    public GameObject castEffectPrefab;
    public GameObject hitEffectPrefab;
    public GameObject projectilePrefab;
    public GameObject critEffect;
    public GameObject damageText;
    public GameObject damageTextCrit;
    public enum ElementalDamageType { Lightning, Fire, Ice, Bludgeoning, Slashing }
    public ElementalDamageType myElementalDamageType;
}
