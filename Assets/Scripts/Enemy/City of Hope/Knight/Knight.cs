using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : EnemyController
{
    private int maxConAttackTime = 1;
    protected override void Hit()
    {
        base.Hit();
        if (attackTarget != null)
        {
            float comfirmValue = Random.value;

            if (comfirmValue < enemyInformation.Continuous_AttackRate)
            {
                if (maxConAttackTime < 3)
                {
                    anim.SetTrigger("isAttack");
                    maxConAttackTime += 1;
                }
                else
                    maxConAttackTime = 1;

            }
        }
    }
}
