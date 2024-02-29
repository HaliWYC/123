using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BuffBase : Singleton<BuffBase>
{
    public delegate void finishedState();
    public event finishedState stateFinished;
    public float durationTime { get; set; } = 0;
    public float currentDurationTime { get; set; } = 0;
    public float buffValue { get; set; } = 0;
    public float currentBuffValue { get; set; } = 0;
    public int durationTurn { get; set; } = 0;
    public float currentDurationTurn { get; set; } = 0;
    public CharacterInformation buffTarget { get; set; }


    public BuffDurationType buffState;
    public BuffStackType buffStack;

    public bool isPermanent;
    public bool isPro;
    public bool isStackable;

    /// <summary>
    /// Initialize the buff with buff target, how long implements a effect, how many times effect will be implement, the buffstate type and the effect value for each time
    /// </summary>
    /// <param name="target">buffTarget</param>
    /// <param name="time">durationTime</param>
    /// <param name="turn">durationTurn</param>
    /// <param name="state">buffStateType</param>
    /// <param name="value">buffValue</param>
    /// <returns></returns>
    public BuffBase SetUp(CharacterInformation target, float time, int turn, BuffDurationType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        durationTime = time;
        durationTurn = turn;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        //initializeData();
        return this;
    }
    /// <summary>
    /// Initialize the buff with buff target, how many times effect will be implement, the buffstate type and the effect value for each time without durationTime
    /// </summary>
    /// <param name="target">buffTarget</param>
    /// <param name="turn">durationTurn</param>
    /// <param name="state">buffStateType</param>
    /// <param name="value">buffValue</param>
    /// <returns></returns>
    public BuffBase SetUp(CharacterInformation target, int turn, BuffDurationType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        durationTurn = turn;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        //initializeData();
        return this;
    }

    /// <summary>
    /// Initialize the buff with buff target, how long implements a effect, the buffstate type and the effect value for each time without the times
    /// </summary>
    /// <param name="target">buffTarget</param>
    /// <param name="time">durationTime</param>
    /// <param name="state">buffStateType</param>
    /// <param name="value">buffValue</param>
    /// <returns></returns>
    public BuffBase SetUp(CharacterInformation target, float time, BuffDurationType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        durationTime = time;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        //initializeData();
        return this;
    }
    /// <summary>
    /// Initialize the buff only with buff target, the buffstate type and the effect value for each time
    /// </summary>
    /// <param name="target">buffTarget</param>
    /// <param name="state">buffStateType</param>
    /// <param name="value">buffValue</param>
    /// <returns></returns>
    public BuffBase SetUp(CharacterInformation target, BuffDurationType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        //initializeData();
        return this;
    }

    //private void initializeData()
    //{
    //    currentBuffValue = buffValue;
    //}

    private void Update()
    {
        SwitchType();
    }

    protected void SwitchType()
    {
        switch (buffState)
        {
            case BuffDurationType.Once:
                Launch();
                stateFinished.Invoke();
                break;
            case BuffDurationType.Sustainable:
                currentDurationTime += Time.deltaTime;
                if (currentDurationTime >= durationTime)
                {
                    currentDurationTime = 0;
                    if (isStackable)
                    {
                        switch (buffStack)
                        {
                            case BuffStackType.StackTurn:
                                currentDurationTurn += durationTurn;
                                break;
                            case BuffStackType.StackValue:
                                currentBuffValue += buffValue;
                                break;
                        }
                    }
                    Launch();
                    currentDurationTurn++;
                    if (currentDurationTurn >= durationTurn)
                        stateFinished.Invoke();
                }
                break;
            case BuffDurationType.Permanent:
                isPermanent = true;
                break;
        }
    }

    public virtual void Launch()
    {

    }


    public void BuffLaunch(SkillDetails_SO  launchSkill, float ExpIncrement)
    {
        launchSkill.buffList.Sort((x, y) => x.buffPriority.CompareTo(y.buffPriority));
        foreach(Buff_SO buff in launchSkill.buffList)
        {
            Debug.Log(buff.buffPriority);
        }
    }
}
