using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW BasicData",menuName ="CharacterData/CharacterInformation")]
public class CharacterInformation_SO : ScriptableObject
{
    [Header("基础信息")]
    public string Name;
    //后期做成效果
    public string Personality;
    public string commandStyle;
    public string Spirit;
    public int prestigeLevel;
    public int currentPrestige;

    [Header("六维")]
    public int Fitness;
    public int Eloquence;
    public int Wisedom;
    public int Luck;
    public int Strength;
    public int Understanding;
}
