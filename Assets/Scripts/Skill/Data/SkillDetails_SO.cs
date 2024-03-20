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
    public BasicQualityType skillQuality;
    [TextArea]
    public string skillInformation;//The area where the introduction of the skill is.
    public int currentExp;//Current experience
    public int nextExp;//The experience limit for reaching next level
    public Proficiency skillProficiency;
    public bool isWeaponSkill;//Check whether this skill is special for weapon
    public bool isCharacterFirst;//Check whether this skill is the first character special skill
    public bool isCharacterSecond;//Check whether this skill is the second character special skill

    [Header("技能属性")]
    public SkillTargetType skillTargetType;
    public SkillRangeType skillRangeType;
    public float skillRange;//Skill actual range value
    public float currentCoolDown;
    public float CoolDown;
    public int QiComsume;
    public float skillProbability;//This only for random calling 
    public bool perfectAccurate;//Check whether this skill can be dodged
    public float timeAfterSpell;//The time after spell the skill and next action

    [Header("技能模型")]
    //TODO:后面判断一下是否需要List来储存prefab
    public GameObject skillPrefab;

    [Header("附加效果")]
    public List<Buff_SO> buffList;
    //TODO:后期根据等级不同增加不同的bufflist
}
