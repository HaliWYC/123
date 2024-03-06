using UnityEngine;

public class BuffBase : MonoBehaviour
{

    //Buff evalue order:
    //1.Priority less first. If priority same, look following
    //2.Negative > Positive
    //3.Health > Wound > Other
    //4.Percentage > Value
    Buff_SO buff;
    protected float valueForReverse;
    protected bool canReverse;
    protected bool isPro;
    protected int buffValue;
    protected bool isPercentage;
    protected bool isCurrentValue;
    protected int effectTurn;
    protected float currentTime;
    protected int durationTime;
    protected bool canStack;
    protected float ExpIncrement;
    protected CharacterInformation targetInfor;

    private bool OnceBuffFinished = false;

    public void BuffSetUp(Buff_SO Buff, float buffExpIncrement, CharacterInformation targetInformation)
    {
        buff = Buff;
        canReverse = buff.canReverse;
        isPro = buff.isPro;
        buffValue = buff.buffValue;
        isPercentage = buff.isPercentage;
        isCurrentValue = buff.isCurrentValue;
        effectTurn = buff.effectTurn;
        durationTime = buff.durationTime;
        canStack = buff.canStack;
        ExpIncrement = buffExpIncrement;
        targetInfor = targetInformation;
    }

    private void Update()
    {
        switch (buff.buffState)
        {
            case BuffDurationType.Once:
                if (!OnceBuffFinished)
                {
                    BuffLaunch();
                    OnceBuffFinished = true;
                    currentTime = durationTime;
                }
                currentTime -= Time.deltaTime;
                if (currentTime <= 0)
                {
                    if (canReverse)
                        BuffReverse();
                    Destroy(this);
                }
                break;
            case BuffDurationType.Sustainable:
                if (effectTurn >= 1)
                {
                    if (currentTime <= 0)
                    {
                        BuffLaunch();
                        effectTurn -= 1;
                        currentTime = durationTime;
                    }
                }
                else if(currentTime<=0)
                {
                    Destroy(this);
                }
                currentTime -= Time.deltaTime;
                break;
            case BuffDurationType.Permanent:
                break;
        }
    }

    protected virtual void BuffLaunch()
    {

    }

    protected virtual void BuffReverse()
    {

    }
}
