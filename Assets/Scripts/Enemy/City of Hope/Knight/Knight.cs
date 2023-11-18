using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : EnemyController
{
    protected override void hit()
    {
        base.hit();
        if (attackTarget != null)
            if (Random.value < enemyInformation.Continuous_AttackRate)
            {
                conAttackTime = enemyInformation.AttackCooling;
                Attack = true;
            }
    }
}
