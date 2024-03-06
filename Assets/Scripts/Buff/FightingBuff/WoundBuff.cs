using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoundBuff : BuffBase
{
    protected override void BuffLaunch()
    {
        if (isPro)
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentWound = Mathf.Max(targetInfor.CurrentWound - (int)(targetInfor.CurrentWound * (1 + buffValue) * ExpIncrement), 0);
                else
                    targetInfor.CurrentWound = Mathf.Max(targetInfor.CurrentWound - (int)(targetInfor.MaxWound * (1 + buffValue) * ExpIncrement), 0);
            }
            else
                targetInfor.CurrentWound = Mathf.Max(targetInfor.CurrentWound - (int)(buffValue * ExpIncrement), 0);
        }
        else
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.CurrentWound += (int)(targetInfor.CurrentWound * (1 + buffValue) * ExpIncrement);
                else
                    targetInfor.CurrentWound += (int)(targetInfor.MaxWound * (1 + buffValue) * ExpIncrement);
            }
            else
                targetInfor.CurrentWound += (int)(buffValue * ExpIncrement);

            if (targetInfor.CheckIsFatal(targetInfor.CurrentWound, targetInfor.MaxWound))
                StartCoroutine(targetInfor.CalculateFatal(1, 1, targetInfor));
        }
        targetInfor.woundChange.Invoke(targetInfor);

    }
}
