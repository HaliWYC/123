using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public List<SkillDetails_SO > skillList;//SkillList on the character
    [SerializeField]private List<SkillDetails_SO > templateSkillList;//SkillList on the character used to clone
    public GameObject skiller;

    protected virtual void Awake()
    {
        if (templateSkillList != null)
        {
            foreach(SkillDetails_SO  skillData in templateSkillList)
            {
                if(skillData!=null)
                skillList.Add(Instantiate(skillData));
            }
        }
        skiller = transform.gameObject;
    }
    protected virtual void Update()
    {
        if(skillList!=null)
        SkillCooling(skillList);
    }
    #region GetSkill
    public SkillDetails_SO  GetSkillDataByName(string skillName)
    {
        foreach(SkillDetails_SO  skillData in skillList)
        {
            return skillList.Find(i => i.SkillName == skillName);
        }
        return null;
    }

    public SkillDetails_SO  GetSkillDataByProbability(float probability)
    {
        foreach (SkillDetails_SO  skillData in skillList)
        {
            return skillList.Find(i => i.skillProbability <= probability);
        }
        return null;
    }

    public SkillDetails_SO  GetSkillDataBySkillRange(float range)
    {
        foreach (SkillDetails_SO  skillData in skillList)
        {
            return skillList.Find(i => i.skillRange >= range);
        }
        return null;
    }

    public SkillDetails_SO GetSkillDataByID(int skillID)
    {
        foreach (SkillDetails_SO skillData in skillList)
        {
            return skillList.Find(i => i.skillID==skillID);
        }
        return null;
    }
    #endregion


    public void SkillEvaluation(SkillDetails_SO currentSkill, CharacterInformation skiller, CharacterInformation defender)
    {
        SkillDetails_SO evaluateSkill =  GetSkillDataByID(currentSkill.skillID);
        evaluateSkill.currentCoolDown = evaluateSkill.CoolDown;
        skiller.CurrentQi -= evaluateSkill.QiComsume;

        if (CheckDodged(skiller, defender, evaluateSkill.perfectAccurate))
        {
            EventHandler.CallDamageTextPopEvent(defender.transform, 0, AttackEffectType.Dodged);
            return;
        }

        BuffBase.Instance.BuffLaunch(evaluateSkill,SkillExpIncrementCalculation(evaluateSkill));

        //TODO:把技能的所有buff以顺序结算
        
    }

    /// <summary>
    /// Check whether target can dodge the skill or not
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="defender"></param>
    /// <param name="perfectAccurate"></param>
    /// <returns></returns>
    public bool CheckDodged(CharacterInformation attacker, CharacterInformation defender, bool perfectAccurate)
    {
        if (perfectAccurate)
            return false;
        if ((defender.Argility - attacker.AttackAccuracy) / Settings.skillDodgeConstant >= Random.Range(0, 1))
        {
            
            return true;
        }
        return false;
    }

    public float SkillExpIncrementCalculation(SkillDetails_SO evaluateSkill)
    {
        float correctionValue = Random.Range(-0.1f, 0.1f);
        int proficiencyValue =
            evaluateSkill.skillProficiency switch
            {
                Proficiency.一窍不通 => 1,
                Proficiency.初窥门径 => 2,
                Proficiency.一知半解 => 3,
                Proficiency.半生不熟 => 4,
                Proficiency.融会贯通 => 5,
                Proficiency.游刃有余 => 6,
                Proficiency.炉火纯青 => 8,
                Proficiency.得心应手 => 10,
                Proficiency.登峰造极 => 12,
                _ => 15
            };

        return (0.2f + correctionValue) * proficiencyValue;
    }

    public void SkillCooling(List<SkillDetails_SO> skillList)
    {
        foreach(SkillDetails_SO skillDetails in skillList)
        {
            if(skillDetails!=null)
            skillDetails.currentCoolDown -= Time.deltaTime;
        }
    }
}
