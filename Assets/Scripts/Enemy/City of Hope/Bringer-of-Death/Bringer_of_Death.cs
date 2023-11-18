using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringer_of_Death : EnemyController
{
    public GameObject tentaclePrefab;

    //Skill Death Tentacle
    public void callTheTentacle()
    {
        if (attackTarget != null)
        {
            var Tentacle = GetComponent<Tentacle>();
            Tentacle.attackTarget = attackTarget;
            if(Random.value<= Tentacle.skillData.skillProbability)
            {
                isSkilling = true;
                Tentacle.generateTentacle(tentaclePrefab);
                isSkilling = false;
            }
        }
    }
}
