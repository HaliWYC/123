using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Skills
{
    private Animator anim;
    private SkillData_SO tentacleData;
    private CharacterInformation targetInfor;
    public void updateData(SkillData_SO tentacle,CharacterInformation target)
    {
        tentacleData = tentacle;
        targetInfor = target;
    }


    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    public void generateTentacle()
    {
        anim.SetTrigger("isGenerated");
    }

    public void tentacleEffect()
    {
        skillEffects(tentacleData, targetInfor);
    }
}
