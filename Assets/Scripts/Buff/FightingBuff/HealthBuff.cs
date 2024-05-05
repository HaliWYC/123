using ShanHai_IsolatedCity.Buff;
using UnityEngine;
public class HealthBuff : BuffBase
{
    protected override  void BuffLaunch()
    {
        if (isPro)
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentHealth = Mathf.Max(targetInfor.CurrentHealth + (int)(targetInfor.CurrentHealth * (1 + buffValue) * ExpIncrement), targetInfor.MaxHealth);
                else
                    targetInfor.CurrentHealth = Mathf.Max(targetInfor.CurrentHealth + (int)(targetInfor.MaxHealth * (1 + buffValue) * ExpIncrement), targetInfor.MaxHealth);
            }
            else
                targetInfor.CurrentHealth = Mathf.Max(targetInfor.CurrentHealth + (int)(buffValue * ExpIncrement), targetInfor.MaxHealth);
        }
        else
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentHealth = Mathf.Max(targetInfor.CurrentHealth - (int)(targetInfor.CurrentHealth * (1 + buffValue) * ExpIncrement), 0);
                else
                    targetInfor.CurrentHealth = Mathf.Max(targetInfor.CurrentHealth - (int)(targetInfor.MaxHealth * (1 + buffValue) * ExpIncrement), 0);
            }
            else
                targetInfor.CurrentHealth = Mathf.Max(targetInfor.CurrentHealth - (int)(buffValue * ExpIncrement), 0);
        }
        targetInfor.healthChange.Invoke(targetInfor);
        EventHandler.CallDamageTextPopEvent(targetInfor.transform, buffValue, AttackEffectType.Skill);
    }
}
