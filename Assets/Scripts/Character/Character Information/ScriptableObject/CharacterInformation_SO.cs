using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW BasicData",menuName ="CharacterData/CharacterInformation")]
public class CharacterInformation_SO : ScriptableObject
{
    [Header("基础信息")]
    public string Name;
    public string Appellation;
    public Sprite charIcon;
    public List<Personality> Personality;
    public string commandStyle;
    public string Spirit;
    public int prestigeIndex;
    public Prestige prestigeLevel;
    public int currentPrestige;
    public int nextPrestige;

    [Header("六维")]
    public int Fitness;
    public int Eloquence;
    public int Wisdom;
    public int Luck;
    public int Strength;
    public int Perception;
}

[System.Serializable]
public class Personality
{
    public string Name;
    public BasicQualityType personalityQuality;
    [TextArea]
    public string description;
}
