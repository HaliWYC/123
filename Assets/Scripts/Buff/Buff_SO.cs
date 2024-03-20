using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW Buff",menuName ="Skill/BuffData")]
public class Buff_SO : ScriptableObject
{
    [Header("基础属性")]
    public string buffName;
    public int buffID;
    public Sprite buffIcon;
    [TextArea]
    public string buffDescription;

    [Header("Buff属性")]
    public BuffDurationType buffState;//Once/Sustain/Permanent
    public EffectType buffType;//Buff effect like:Damage,Cure,Undefeated
    public bool isForSelf;//Is this buff target itself
    public bool canReverse;//Can this buff reverse(Only for Once buff)
    public int buffPriority;
    public bool isPro;//Is this buff positive or negative
    public int buffValue;//Value evaluate in each turn
    public bool isPercentage;//Is the Value percentage or not
    public bool isCurrentValue;
    public int effectTurn;//Number of turn
    public int durationTime;//The timespan between each turn evaluate
    public bool canStack;//Whether the buff can stack or not
    public BuffStackType stackType;//If buff can stack, how it stack
}
