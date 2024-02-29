using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW Skill",menuName ="Skill/SkillData")]
public class SkillDetails_SO : ScriptableObject
{
    [Header("基础信息")]
    public string SkillName;
    public int skillID;
    public Sprite skillIcon;
    public QualityType skillQuality;
    [TextArea]
    public string skillInformation;
    public int currentExp;
    public int nextExp;
    public Proficiency skillProficiency;
    //public bool canSpell;

    [Header("技能属性")]
    public SkillTargetType skillTargetType;
    public SkillRangeType skillRangeType;
    public float skillRange;
    public float currentCoolDown;
    public float CoolDown;
    public int QiComsume;
    public float skillProbability;
    public bool perfectAccurate;
    public float timeAfterSpell;

    [Header("技能模型")]
    //TODO:后面判断一下是否需要List来储存prefab
    public GameObject skillPrefab;

    [Header("附加效果")]
    public List<Buff_SO> buffList;
}
