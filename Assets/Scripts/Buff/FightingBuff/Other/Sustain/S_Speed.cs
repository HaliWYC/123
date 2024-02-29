using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Speed : BuffBase
{
    public int originalSpeed;

    private void Start()
    {
        originalSpeed = buffTarget.Speed;
    }
    public override void Launch()
    {
        originalSpeed = buffTarget.Speed;
        if(isPro)
            buffTarget.Speed = (int)(buffTarget.Speed * currentBuffValue);
        else
            buffTarget.Speed =(int)(buffTarget.Speed/currentBuffValue);
        stateFinished += OnStateFinished;
    }

    private void OnStateFinished()
    {
        buffTarget.Speed = originalSpeed;
        Destroy(this);
    }

    private void OnDisable()
    {
        stateFinished -= OnStateFinished;
    }
}
