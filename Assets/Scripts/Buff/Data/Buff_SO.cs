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
    public EffectType effectType;//Buff effect like:Damage,Cure,Undefeated
    public BasicQualityType buffQuality;
    public BuffTarget target;//Is this buff target itself
    public bool canReverse;//Can this buff reverse(Only for Once buff)
    public int buffPriority;
    public bool isPro;//Is this buff positive or negative
    public int buffValue;//Value evaluate in each turn
    public bool isPercentage;//Is the Value percentage or not
    public bool isCurrentValue;//Is base on current value
    public int effectTurn;//Number of turn
    public int durationTime;//The timespan between each turn evaluate
    public bool CanStack;//Whether the buff can stack or not
    public BuffStackType stackType;//If buff can stack, how it stack
}

//Once：自己生命值单次增加30点，不可叠加
//Sustainable：敌人生命值每30秒减少40点，持续3轮，可叠加（仅叠加数值）