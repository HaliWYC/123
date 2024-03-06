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
        BuffEvaluation(evaluateSkill,SkillExpIncrementCalculation(evaluateSkill),skiller,defender);
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
        if ((defender.Argility - attacker.AttackAccuracy) / Settings.skillDodgeConstant >= Random.Range(0, 1) || defender.isDodged)
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
    #region PlayerSkill
    public void PlayerSkilldetection()
    {
        if (skiller.CompareTag("Player"))
        {
            
        }
    }
    #endregion

    #region BuffEvaluation
    /// <summary>
    /// This method is used to sort the bufflist of the skill considering the priority
    /// </summary>
    /// <param name="spellSkill"></param>
    /// <param name="BuffExpIncrement"></param>
    /// <param name="skiller"></param>
    /// <param name="defender"></param>
    public void BuffEvaluation(SkillDetails_SO spellSkill,float BuffExpIncrement, CharacterInformation skiller, CharacterInformation defender)
    {
        //FIXME:后期加上如果Priority相同的情况
        spellSkill.buffList.Sort((x, y) => x.buffPriority.CompareTo(y.buffPriority));
        foreach(Buff_SO buff in spellSkill.buffList)
        {
            //Check the buff target
            if (buff.isForSelf)
                SwitchBuff(buff, BuffExpIncrement, skiller);
            else
                SwitchBuff(buff, BuffExpIncrement, defender);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="BuffExpIncrement"></param>
    /// <param name="buffTarget"></param>
    public void SwitchBuff(Buff_SO buff, float BuffExpIncrement, CharacterInformation buffTarget)
    {
        switch (buff.buffType)
        {
            case EffectType.生命值:
                buffTarget.gameObject.AddComponent<HealthBuff>().BuffSetUp(buff,BuffExpIncrement,buffTarget);
                break;
            case EffectType.伤口:
                break;
            case EffectType.速度 :
                buffTarget.gameObject.AddComponent<SpeedBuff>().BuffSetUp(buff, BuffExpIncrement, buffTarget);
                break;
            case EffectType.攻击:
                break;
            case EffectType.防御:
                break;
            case EffectType.造成眩晕:
                break;
            case EffectType.免疫:
                break;
            case EffectType.霸体:
                break;
        }
    }


    #endregion
}