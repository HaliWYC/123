using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Health : BuffBase
{
    public override void Launch()
    {
        if (isPro)
        {
            buffTarget.CurrentHealth = Mathf.Min((int)(buffTarget.CurrentHealth + currentBuffValue), buffTarget.MaxHealth);
            EventHandler.CallDamageTextPopEvent(buffTarget.transform, (int)currentBuffValue, AttackEffectType.Skill);
        }
        else
        {
            buffTarget.CurrentHealth = Mathf.Max((int)(buffTarget.CurrentHealth - currentBuffValue), 0);
            EventHandler.CallDamageTextPopEvent(buffTarget.transform, -(int)currentBuffValue, AttackEffectType.Skill);
        }   
        buffTarget.GetComponent<CharacterInformation>().healthChange?.Invoke(buffTarget.GetComponent<CharacterInformation>());
        
        stateFinished += OnStateFinished;
    }

    private void OnStateFinished()
    {
        Destroy(this);
    }

    private void OnDisable()
    {
        stateFinished -= OnStateFinished;
    }
}
