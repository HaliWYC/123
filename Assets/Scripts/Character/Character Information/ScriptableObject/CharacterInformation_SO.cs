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
    //后期做成效果
    public string Personality;
    public string commandStyle;
    public string Spirit;
    public int prestigeIndex;
    public Prestige prestigeLevel;
    public int currentPrestige;
    public int nextPrestige;

    [Header("六维")]
    public int Fitness;
    public int Eloquence;
    public int Wisedom;
    public int Luck;
    public int Strength;
    public int Perception;
}
