using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BuffBase : MonoBehaviour
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


    public BuffStateType buffState;
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
    public BuffBase SetUp(CharacterInformation target, float time, int turn,BuffStateType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        durationTime = time;
        durationTurn = turn;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        initializeData();
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
    public BuffBase SetUp(CharacterInformation target, int turn, BuffStateType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        durationTurn = turn;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        initializeData();
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
    public BuffBase SetUp(CharacterInformation target, float time,BuffStateType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        durationTime = time;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        initializeData();
        return this;
    }
    /// <summary>
    /// Initialize the buff only with buff target, the buffstate type and the effect value for each time
    /// </summary>
    /// <param name="target">buffTarget</param>
    /// <param name="state">buffStateType</param>
    /// <param name="value">buffValue</param>
    /// <returns></returns>
    public BuffBase SetUp(CharacterInformation target, BuffStateType state, float value, bool Pro, bool stackable)
    {
        buffTarget = target;
        buffState = state;
        buffValue = value;
        isPro = Pro;
        isStackable = stackable;
        initializeData();
        return this;
    }

    private void initializeData()
    {
        currentBuffValue = buffValue;
    }

    private void Update()
    {
        switchType();
    }

    protected void switchType()
    {
        switch (buffState)
        {
            case BuffStateType.Once:
                launch();
                stateFinished.Invoke();
                break;
            case BuffStateType.Sustainable:
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
                    launch();
                    currentDurationTurn++;
                    if (currentDurationTurn >= durationTurn)
                        stateFinished.Invoke();
                }
                break;
            case BuffStateType.Permanent:
                isPermanent = true;
                break;
        }
    }

    public virtual void launch()
    {

    }
}
