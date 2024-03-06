using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : BuffBase
{
    protected override void BuffLaunch()
    {
        if (canReverse)
            valueForReverse = targetInfor.Speed;

        if (isPro)
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.Speed = Mathf.Min(targetInfor.Speed + (int)(targetInfor.Speed * (1 + buffValue) * ExpIncrement), (int)Settings.maxSpeed);
            }
            else
                targetInfor.Speed = Mathf.Min(targetInfor.Speed + (int)(buffValue * ExpIncrement), (int)Settings.maxSpeed);
        }
        else
        {
            if (isPercentage)
            {
                if (isCurrentValue)
                    targetInfor.Speed = Mathf.Max(targetInfor.Speed - (int)(targetInfor.Speed * (1 + buffValue) * ExpIncrement), 0);
            }
            else
                targetInfor.Speed = Mathf.Max(targetInfor.Speed - (int)(buffValue * ExpIncrement), 0);
        }
    }

    protected override void BuffReverse()
    {
        targetInfor.Speed = (int)valueForReverse;
    }
}
