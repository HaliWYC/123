using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_Health : BuffBase
{
    public override void launch()
    {
        if (isPro)
            buffTarget.CurrentHealth = Mathf.Min((int)(buffTarget.CurrentHealth + currentBuffValue), buffTarget.MaxHealth);
        else
            buffTarget.CurrentHealth = Mathf.Max((int)(buffTarget.CurrentHealth - currentBuffValue), 0);
        buffTarget.GetComponent<CharacterInformation>().healthChange?.Invoke(buffTarget.GetComponent<CharacterInformation>());
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
