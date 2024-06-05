#region Inventory
public enum ItemType
{
    Equip,Consume,Task,Other
}

public enum SlotType
{
    PlayerBag,Player,WareHouse,NPCBag
}

public enum InventoryLocation
{
    Player,NPC
}

public enum EquipItemType
{
    Head,Neck,UpperBody,LowerBody,LeftHand,RightHand,Shoe,Accessory,Mount,Special
}

public enum ConsumeItemType
{

}

public enum TaskItemType
{

}

public enum OtherItemType
{

}

public enum WeaponType
{
    单手剑,双手剑,长刀,短刀,斧头,匕首,盾牌,琴,长枪,长矛,戟,笛,拳套,非武器
}

public enum EquipQualityType
{
    神器,传说,史诗,卓越,精良,优秀,普通,残品
}

public enum CharacterFightingDataType
{
    Character, Equip, Buff
}
#endregion

#region GameProperty
public enum BasicQualityType
{
    赤,橙,黄,绿,青,蓝,紫,灰
}

public enum Seasons
{
    春,夏,秋,冬
}

public enum LightShift
{
    破晓, 清晨, 黄昏, 夜晚
}

public enum GameState
{
    GamePlay, Pause
}
public enum Proficiency
{
    一窍不通, 初窥门径, 一知半解, 半生不熟, 融会贯通, 游刃有余, 炉火纯青, 得心应手, 登峰造极, 出神入化
}

public enum Prestige
{
    遗臭万年, 身败名裂, 臭名昭著, 声名狼藉, 名誉扫地, 默默无闻, 小有名气, 远近闻名, 声名远扬, 举世闻名, 名扬四海, 扬名天下, 震烁古今
}
#endregion

#region NPC&Enemy
public enum EnemyState
{
    和平, 警惕, 攻击, 巡逻, 死亡
}

public enum EnemyLevelType
{
    入门, 普通, 精英, 史诗, 王者, 神话, 传说, 上古, 秩序, 法则
}
#endregion

#region SKil&Buff
public enum SkillTargetType
{
    单体,范围,群体
}

public enum EffectType
{
    Health, Wound, Speed, Attack, Defense, Dizzy, Undeated, Dodged
}

public enum BuffDurationType
{
    Once,Sustainable,Permanent
}

public enum AttackEffectType
{
    Normal,Critical,continuousDamage,continuouseAttack,Skill,Undefeated,Dodged
}

public enum BuffTarget
{
    Self,Enemy
}

public enum BuffStackType
{
    StackTurn,StackValue
}
#endregion
#region Dialogue&Task
public enum DialoguePieceType
{
    InCG, OnlyText
}

public enum DialogueOptionType
{
    Talk, Task, Trade, GivePresent, Leave
}

public enum TaskType
{
    Mission, SideQuest, DailyQuest
}

public enum TaskRequirementsType
{
    BringItem, PickItem, KillEnemy, Travel
}
public enum TaskRewardType
{
    Item
}
#endregion


public enum ParticleEffectType
{
    None, LeaveFalling01, LeaveFalling02
}

public enum GridType
{
    可投掷区, 近战区, 远程区, NPC障碍
}


