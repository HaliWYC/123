using UnityEngine;
using ShanHai_IsolatedCity.Buff;

public class VigorBuff : BuffBase
{
    protected override void BuffLaunch()
    {
        if (isPro)
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentVigor = Mathf.Max(targetInfor.CurrentVigor + (int)(targetInfor.CurrentVigor * (1 + buffValue) * ExpIncrement), targetInfor.MaxHealth);
                else
                    targetInfor.CurrentVigor = Mathf.Max(targetInfor.CurrentVigor + (int)(targetInfor.MaxVigor * (1 + buffValue) * ExpIncrement), targetInfor.MaxHealth);
            }
            else
                targetInfor.CurrentVigor = Mathf.Max(targetInfor.CurrentVigor + (int)(buffValue * ExpIncrement), targetInfor.MaxVigor);
        }
        else
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentVigor = Mathf.Max(targetInfor.CurrentVigor - (int)(targetInfor.CurrentVigor * (1 + buffValue) * ExpIncrement), 0);
                else
                    targetInfor.CurrentVigor = Mathf.Max(targetInfor.CurrentVigor - (int)(targetInfor.MaxVigor * (1 + buffValue) * ExpIncrement), 0);
            }
            else
                targetInfor.CurrentVigor = Mathf.Max(targetInfor.CurrentVigor - (int)(buffValue * ExpIncrement), 0);
        }
        targetInfor.healthChange.Invoke(targetInfor);

    }
}
