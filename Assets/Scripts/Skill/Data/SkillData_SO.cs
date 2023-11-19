using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW Skill",menuName ="CharacterData/Skill")]
public class SkillData_SO : ScriptableObject
{
    [Header("基础信息")]
    public string SkillName;
    public QualityType skillQuality;

    [Header("技能属性")]
    public SkillTargetType skillTargetType;
    public SkillRangeType skillRangeType;
    public EffectType skillEffect;
    public int QiComsume;
    public float skillRange;
    public float skillProbability;
}
