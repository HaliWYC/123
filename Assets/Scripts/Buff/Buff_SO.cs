using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW Buff",menuName ="CharacterData/BuffData")]
public class Buff_SO : ScriptableObject
{
    [Header("基础属性")]
    public string buffName;
    public int buffID;
    public Sprite buffIcon;

    [Header("Buff属性")]
    public BuffDurationType buffState;
    public EffectType buffType;
    public int buffPriority;
    public int buffValue;
    public int effectTurn;
    public int durationTime;
    public BuffStackType stackType;
}
