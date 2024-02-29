using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : EnemyController
{
    protected override void Hit()
    {
        base.Hit();
        if (attackTarget != null)
            if (Random.value < enemyInformation.Continuous_AttackRate)
            {
                conAttackTime = enemyInformation.AttackCooling;
                canAttack = true;
            }
    }
}
