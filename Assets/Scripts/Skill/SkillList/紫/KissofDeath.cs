using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Skill;

public class KissofDeath : SkillSpeller
{
    private Animator anim;
    public SkillDetails_SO  KissofDeathData;

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
        
        if (currentSkill.skillID != KissofDeathData.skillID)
            return;
        
        if (Target.GetComponent<CharacterInformation>().isUndefeated)
        {
            Debug.Log("Undefeated");
            return;
        }
        
        if (Target != null)
        {
            
            if (Vector3.Distance(Target.transform.position,Skiller.transform.position) <= KissofDeathData.skillRange)
            {
                Vector3 targetPos = new Vector3(Target.transform.position.x + 0.2f, Target.transform.position.y + 1f, 0);
                Instantiate(KissofDeathData.skillPrefab, targetPos, Quaternion.identity);
                Skiller.GetComponent<Skills>().SkillEvaluation(KissofDeathData,Skiller.GetComponent<CharacterInformation>(),Target.GetComponent<CharacterInformation>());
            }
        }
    }
}
