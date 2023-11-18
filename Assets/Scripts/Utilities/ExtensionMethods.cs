using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    private const float dotThrehold = 0.5f;
    //TODO Can change the dotThrehold of each Enemy
    public static bool isFacingTarget(this Transform transform,Transform target)
    {
        var vectorToTarget = target.position - transform.position;

        float dot = Vector3.Dot(transform.forward, vectorToTarget);

        return dot >= dotThrehold;
    }
}
