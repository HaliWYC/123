using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public List<SkillData_SO> skillList;


    public SkillData_SO getSkillDataByName(string skillName)
    {
        foreach(SkillData_SO skillData in skillList)
        {
            return skillList.Find(i => i.SkillName == skillName);
        }
        return null;
    }

    public SkillData_SO getSkillDataByProbability(float probability)
    {
        foreach (SkillData_SO skillData in skillList)
        {
            return skillList.Find(i => i.skillProbability <= probability);
        }
        return null;
    }

    public SkillData_SO getSkillDataBySkillRange(float range)
    {
        foreach (SkillData_SO skillData in skillList)
        {
            return skillList.Find(i => i.skillRange >= range);
        }
        return null;
    }
}
