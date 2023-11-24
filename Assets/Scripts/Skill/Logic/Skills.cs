using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public List<SkillData_SO> skillList;
    [SerializeField]private List<SkillData_SO> templateSkillList;
    public GameObject attackTarget;
    public GameObject skiller;

    protected virtual void Awake()
    {
        if (CompareTag("NPC") || CompareTag("Enemy"))
        {
            skiller = GetComponent<EnemyController>().gameObject;
            
        }
        else if (CompareTag("Player"))
            skiller = GetComponent<Player>().gameObject;
        if (templateSkillList != null)
            skillList = templateSkillList;
        
    }
    protected virtual void Update()
    {
        foreach(SkillData_SO skillData in skillList)
        {
            if (skillData.currentCoolDown > 0 && !skillData.canSpell)
                skillData.currentCoolDown -= Time.deltaTime;
            else
            {
                skillData.canSpell = true;
            }
        }
    }
    #region GetSkill
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
    #endregion

    public void callSkill(string name)
    {
        SkillData_SO calledSkill = getSkillDataByName(name);
        calledSkill.currentCoolDown = calledSkill.CoolDown;
        calledSkill.canSpell = false;
    }

    public void skillEffects(SkillData_SO currentSkill, CharacterInformation defender)
    {
        switch (currentSkill.skillEffect)
        {
            case EffectType.单次造成伤害:
                attackTarget.AddComponent<O_Health>().SetUp(defender, BuffStateType.Once, currentSkill.effectValue, false, currentSkill.effectStackable);
                break;
            case EffectType.持续造成伤害:
                attackTarget.AddComponent<S_Health>().SetUp(defender, currentSkill.durationTime, currentSkill.effectTurn,BuffStateType.Sustainable,currentSkill.effectValue,false,currentSkill.effectStackable);
                break;
        }
    }
}
