using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW FightingData",menuName ="CharacterData/FightingData")]
public class CharacterFightingData_SO : ScriptableObject
{
    [Header("阴阳五行")]
    public int Metal;
    public int Wood;
    public int Water;
    public int Fire;
    public int Ground;
    public int Lunar_Solar;

    [Header("基本属性")]
    public int maxHealth;
    public int currentHealth;
    public int maxVigor;
    public int currentVigor;
    public int maxWound;
    public int currentWound;
    public int maxQi;
    public int currentQi;
    public int maxMorale;
    public int currentMorale;
    public int Argility;
    public int Resilience;
    public int speed;
    public int fatalLevel;
    public float woundRecovery;
    public float skillCooling;


    [Header("攻击属性")]
    public int Attack;
    public int attackAccuracy;
    public int createWound;
    public int Penetrate;
    public int criticalRate;
    public float AttackCooling;
    public float meleeRange;
    public float rangedRange;
    public float criticalMutiple;
    public float fatal_Enhancement;
    public float continuous_DamageRate;
    public float continuous_AttackRate;

    [Header("防御属性")]
    public int Defense;
    public int penetrateDefense;
    public float criticalDefense;
    public float fatalDefense;
    public float parryCoolDown;
}
