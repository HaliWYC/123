using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : SkillSpellManager
{
    private Animator anim;
    public SkillDetails_SO  tentacleData;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GenerateTentacle();
    }
    public void GenerateTentacle()
    {
        anim.SetTrigger("isGenerated");
    }

    public override void CallTheSkill(SkillDetails_SO currentSkill,GameObject Target, GameObject Skiller)
    {
        
        if (currentSkill.skillID != tentacleData.skillID)
            return;
        
        if (Target.GetComponent<CharacterInformation>().isUndefeated)
        {
            Debug.Log("Undefeated");
            return;
        }
        
        if (Target != null)
        {
            
            if (Vector3.Distance(Target.transform.position,Skiller.transform.position) <= tentacleData.skillRange)
            {
                switch (tentacleData.skillRangeType)
                {
                    //TODO:后期加上不同距离的限制或buff
                    case SkillRangeType.近战:                     
                        break;
                    case SkillRangeType.远程:
                        break;
                }
                Vector3 targetPos = new Vector3(Target.transform.position.x + 0.2f, Target.transform.position.y + 1f, 0);
                Instantiate(tentacleData.skillPrefab, targetPos, Quaternion.identity);
                Skiller.GetComponent<Skills>().SkillEvaluation(tentacleData,Skiller.GetComponent<CharacterInformation>(),Target.GetComponent<CharacterInformation>());
            }
        }
    }
}
