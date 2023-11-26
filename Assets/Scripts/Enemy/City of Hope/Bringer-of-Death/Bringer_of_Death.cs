using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_of_Death : EnemyController
{
    public GameObject tentaclePrefab;
    //TODO:后期加上随机生成其中一个技能
    private float timeAfterLastSkill;
    private SkillData_SO skill;
    protected override void Start()
    {
        base.Start();
        skillRange = npcDetails.sightRadius;
        skill = GetComponent<Skills>().getSkillDataBySkillRange(npcDetails.sightRadius);
        //hasSkill = false;
    }

    protected override void Update()
    {
        base.Update();
        if (timeAfterLastSkill > 0)
            timeAfterLastSkill -= Time.deltaTime;
        if(skill!=null && Random.value <= skill.skillProbability && timeAfterLastSkill<=0 && skill.canSpell)
        {
            isSkillRange = true;
            canSkill = true;
        }
    }

    //Skill Death Tentacle
    public void callTheTentacle()
    {
        //TODO:后期变成一个泛用技能
        if (attackTarget != null )
        {
            Skills currentSkill = GetComponent<Skills>();
            SkillData_SO skillData = currentSkill.getSkillDataByName("Kiss_of_Death");
            if (targetInSkillRange(skillData.skillRange))
            {
                bool isInRange = false;
                switch (skillData.skillRangeType)
                {
                    case SkillRangeType.近战:
                        isInRange = targetInMeleeRange();
                        break;
                    case SkillRangeType.远程:
                        isInRange = targetInRangedRange();
                        break;
                }
                if (isInRange &&skillData.canSpell)
                {
                    Vector3 targetPos = new Vector3(attackTarget.transform.position.x + 0.2f, attackTarget.transform.position.y + 1f, 0);
                    var tentacle = Instantiate(tentaclePrefab, targetPos, Quaternion.identity);
                    tentacle.GetComponent<Tentacle>().attackTarget = attackTarget;
                    skillRange = skillData.skillRange;
                    tentacle.GetComponent<Tentacle>().generateTentacle();
                    tentacle.GetComponent<Tentacle>().updateData(skillData, attackTarget.GetComponent<CharacterInformation>());
                    skillRange = npcDetails.sightRadius;
                    canSkill = false;
                    currentSkill.callSkill("Kiss_of_Death");
                }
                //TODO: 后期加上技能效果    
            }
        }
    }
}
