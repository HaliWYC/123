using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public GameObject attackTarget;
    private Animator anim;
    public SkillData_SO skillData;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void generateTentacle(GameObject tentacle)
    {
        Vector3 targetPos = new Vector3(attackTarget.transform.position.x + 0.2f, attackTarget.transform.position.y + 1f, 0);
        transform.position = targetPos;
        Instantiate(tentacle, targetPos, Quaternion.identity);
        anim.SetTrigger("isGenerated");
    }
}
