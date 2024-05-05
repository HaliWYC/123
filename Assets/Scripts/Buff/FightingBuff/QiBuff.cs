using UnityEngine;
using ShanHai_IsolatedCity.Buff;
public class QiBuff : BuffBase
{
    protected override void BuffLaunch()
    {
        if (isPro)
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentQi = Mathf.Max(targetInfor.CurrentQi + (int)(targetInfor.CurrentQi * (1 + buffValue) * ExpIncrement), targetInfor.MaxHealth);
                else
                    targetInfor.CurrentQi = Mathf.Max(targetInfor.CurrentQi + (int)(targetInfor.MaxQi * (1 + buffValue) * ExpIncrement), targetInfor.MaxHealth);
            }
            else
                targetInfor.CurrentQi = Mathf.Max(targetInfor.CurrentQi + (int)(buffValue * ExpIncrement), targetInfor.MaxQi);
        }
        else
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentQi = Mathf.Max(targetInfor.CurrentQi - (int)(targetInfor.CurrentQi * (1 + buffValue) * ExpIncrement), 0);
                else
                    targetInfor.CurrentQi = Mathf.Max(targetInfor.CurrentQi - (int)(targetInfor.MaxQi * (1 + buffValue) * ExpIncrement), 0);
            }
            else
                targetInfor.CurrentQi = Mathf.Max(targetInfor.CurrentQi - (int)(buffValue * ExpIncrement), 0);
        }
        targetInfor.qiChange.Invoke(targetInfor);

    }
}
