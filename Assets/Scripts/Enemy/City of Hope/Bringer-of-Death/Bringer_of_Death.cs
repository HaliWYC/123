using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_of_Death : EnemyController
{
    public GameObject tentaclePrefab;
    private bool hasSkill;
    //TODO:后期加上随机生成其中一个技能
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
        
        //if (skill != null && Random.value <= skill.skillProbability && !hasSkill&& enemyInformation.CurrentQi >= skill.QiComsume)
        //{
        //    enemyInformation.CurrentQi -= skill.QiComsume;
        //    isSkillRange = true;
        //    canSkill = true;
        //    hasSkill = true;
        //}
        if(skill!=null && Random.value <= skill.skillProbability && enemyInformation.CurrentQi>=skill.QiComsume)
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
            SkillData_SO skill = GetComponent<Skills>().getSkillDataByName("Kiss of Death");
            
            if (targetInSkillRange(skill.skillRange))
            {
                bool isInRange=false;
                switch (skill.skillRangeType)
                {
                    case SkillRangeType.近战:
                        isInRange=targetInMeleeRange();
                        break;
                    case SkillRangeType.远程:
                        isInRange = targetInRangedRange();
                        break;
                }
                if (isInRange)
                {
                    Vector3 targetPos = new Vector3(attackTarget.transform.position.x + 0.2f, attackTarget.transform.position.y + 1f, 0);
                    transform.position = targetPos;
                    var tentacle = Instantiate(tentaclePrefab, targetPos, Quaternion.identity);
                    tentacle.GetComponent<Tentacle>().attackTarget = attackTarget;
                    isSkilling = true;
                    skillRange = skill.skillRange;
                    tentacle.GetComponent<Tentacle>().generateTentacle();
                    enemyInformation.CurrentQi -= skill.QiComsume;
                    skillRange = npcDetails.sightRadius;
                    canSkill = false;
                    isSkilling = false;
                }
                //TODO: 后期加上技能效果    
            }
        }
    }
}
