using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Health : BuffBase
{
    public override void launch()
    {
        if (isPro)
            buffTarget.CurrentHealth = (int)(buffTarget.CurrentHealth + currentBuffValue);
        else
            buffTarget.CurrentHealth = (int)(buffTarget.CurrentHealth - currentBuffValue);
        stateFinished += onStateFinished;
    }

    private void onStateFinished()
    {
        Destroy(this);
    }

    private void OnDisable()
    {
        stateFinished -= onStateFinished;
    }
}
