using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public GameObject attackTarget;
    private Animator anim;
    public SkillData_SO skillData;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void generateTentacle()
    {
        anim.SetTrigger("isGenerated");
    }
}
