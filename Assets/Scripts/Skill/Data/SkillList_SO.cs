using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW SkillList",menuName ="Skill/SkillList")]
public class SkillList_SO : ScriptableObject
{
    public List<SkillDetails_SO > skillList;
}
