using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PoppingDamageText : MonoBehaviour
{
    public PoppingUpDamageText damageText;

    private void OnEnable()
    {
        EventHandler.damageTextPopEvent += onDamageTextPopEvent;
    }

    private void OnDisable()
    {
        EventHandler.damageTextPopEvent -= onDamageTextPopEvent;
    }

    private void onDamageTextPopEvent(Transform targetPos, int damage, AttackEffectType attackEffect)
    {
        
        PoppingUpDamageText Text =Instantiate(damageText, targetPos.position, Quaternion.identity);
        Text.setUp(damage, attackEffect);
        
    }

}
