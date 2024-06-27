using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW ConsumeItem",menuName ="Inventory/Item/ConsumeItem")]
public class ConsumeItem_SO : ScriptableObject
{
    [Header("Character Information")]
    public int currentPrestige;
    public int Fitness;
    public int Eloquence;
    public int Wisdom;
    public int Luck;
    public int Strength;
    public int Perception;

    [Header("Character FightingData")]
    public int Metal;
    public int Wood;
    public int Water;
    public int Fire;
    public int Ground;
    public int Lunar_Solar;
    public int MaxHealth;
    public int CurrentHealth;
    public int MaxVigor;
    public int CurrentVigor;
    public int MaxWound;
    public int CurrentWound;
    public int MaxQi;
    public int CurrentQi;
    public int MaxMorale;
    public int CurrentMorale;
    public int Argility;
    public int Resilience;
    public int Speed;
    public float WoundRecovery;
    public float SkillCooling;
    public float MinimumRange;
    public float MaximumRange;
    public int Attack;
    public float AttackCooling;
    public int AttackAccuracy;
    public int Penetrate;
    public int WoundCreate;
    public int CriticalPoint;
    public float CriticalMultiple;
    public float Fatal_Enhancement;
    public int Defense;
    public int PenetrateDefense;
    public float CriticalDefense;
    public float FatalDefense;

    [Header("Inventory")]
    public int Gold;
    public int ShanHaiGold;
}
