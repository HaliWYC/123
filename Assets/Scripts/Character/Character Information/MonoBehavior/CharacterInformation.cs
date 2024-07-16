using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ShanHai_IsolatedCity.Inventory;
public class CharacterInformation : MonoBehaviour
{
    public CharacterInformation_SO templateInformation;
    public CharacterInformation_SO characterInformation;

    public CharacterFightingData_SO templateFightingData;
    public CharacterFightingData_SO fightingData;
    public CharacterFightingData_SO templateEquipFightingData;
    public CharacterFightingData_SO equipFightingData;
    public CharacterFightingData_SO templateBuffFightingData;
    public CharacterFightingData_SO buffFightingData;

    private float lastWoundRecoverTime=0;
    private float lastVigorRocoverTime=0;
    public bool isUndefeated;
    public bool isDodged;

    public UnityEvent<CharacterInformation> healthChange;
    public UnityEvent<CharacterInformation> qiChange;
    public UnityEvent<CharacterInformation> vigorChange;
    public UnityEvent<CharacterInformation> woundChange;
    public UnityEvent<CharacterInformation> moraleChange;
    public UnityEvent criticalShakeEvent;
    private void Awake()
    {
        if (templateInformation != null)
            characterInformation = Instantiate(templateInformation);
        if (templateFightingData != null)
            fightingData = Instantiate(templateFightingData);
        if (templateEquipFightingData != null)
            equipFightingData = Instantiate(templateEquipFightingData);
        if (templateBuffFightingData != null)
            buffFightingData = Instantiate(templateBuffFightingData);
    }

    public void UseConsumeItem(ConsumeItem_SO Consume)
    {
        CurrentPrestige += Consume.currentPrestige;
        Fitness += Consume.Fitness;
        Eloquence += Consume.Eloquence;
        Wisedom += Consume.Wisdom;
        Luck += Consume.Luck;
        Strength += Consume.Strength;
        Perception += Consume.Perception;
        Metal += Consume.Metal;
        Wood += Consume.Wood;
        Water += Consume.Water;
        Fire += Consume.Fire;
        Ground += Consume.Ground;
        Lunar_Solar += Consume.Lunar_Solar;
        MaxHealth += Consume.MaxHealth;
        CurrentHealth += Consume.CurrentHealth;
        MaxVigor += Consume.MaxVigor;
        CurrentVigor += Consume.CurrentVigor;
        MaxWound += Consume.MaxWound;
        CurrentWound += Consume.CurrentWound;
        MaxQi += Consume.MaxQi;
        CurrentQi += Consume.CurrentQi;
        MaxMorale += Consume.MaxMorale;
        CurrentMorale += Consume.CurrentMorale;
        Argility += Consume.Argility;
        Resilience += Consume.Resilience;
        Speed += Consume.Speed;
        WoundRecovery += Consume.WoundRecovery;
        SkillCooling += Consume.SkillCooling;
        MinimumRange += Consume.MinimumRange;
        MaximumRange += Consume.MaximumRange;
        Attack += Consume.Attack;
        AttackCooling += Consume.AttackCooling;
        AttackAccuracy += Consume.AttackAccuracy;
        Penetrate += Consume.Penetrate;
        CreateWound += Consume.WoundCreate;
        CriticalPoint += Consume.CriticalPoint;
        Criticalmutiple += Consume.CriticalMultiple;
        Fatal_Enhancement += Consume.Fatal_Enhancement;
        Defense += Consume.Defense;
        PenetrateDefense += Consume.PenetrateDefense;
        CriticalDefense += Consume.CriticalDefense;
        FatalDefense += Consume.FatalDefense;
    }

    private void Start()
    {
        isUndefeated = false;
        isDodged = false;
    }

    private void Update()
    {
        WoundRecoveryProcess();
        VigorRecovery();
    }

    public void ExecuteBenefits()
    {
        //TODO:不断的修改效果找到一个数值平衡
        this.CurrentHealth = Mathf.Min(this.CurrentHealth + (int)(MaxHealth * 0.05), this.MaxHealth);
        this.CurrentQi = Mathf.Min(this.CurrentQi + (int)(MaxQi * 0.05), this.MaxQi);
        this.CurrentVigor = Mathf.Min(this.CurrentVigor + (int)(MaxVigor * 0.5), this.MaxVigor);
        this.CurrentWound = Mathf.Max(this.CurrentWound - 2, 0);
        this.CurrentMorale = Mathf.Min(this.CurrentMorale + (int)(MaxMorale * 0.25), this.MaxMorale);
        healthChange.Invoke(this);
        qiChange.Invoke(this);
        vigorChange.Invoke(this);
        woundChange.Invoke(this);
        moraleChange.Invoke(this);
    }

    #region basicInfor
    public int PrestigeIndex
    {
        get { if (characterInformation != null) return characterInformation.prestigeIndex; else return 0; }
        set { characterInformation.prestigeIndex = value; }
    }
    public int CurrentPrestige
    {
        get { if (characterInformation != null) return characterInformation.currentPrestige; else return 0; }
        set { characterInformation.currentPrestige = value; }
    }
    public int NextPrestige
    {
        get { if (characterInformation != null) return characterInformation.nextPrestige; else return 0; }
        set { characterInformation.nextPrestige = value; }
    }
    public int Fitness
    {
        get { if (characterInformation != null) return characterInformation.Fitness; else return 0; }
        set { characterInformation.Fitness = value; }
    }
    public int Eloquence
    {
        get { if (characterInformation != null) return characterInformation.Eloquence; else return 0; }
        set { characterInformation.Eloquence = value; }
    }
    public int Wisedom
    {
        get { if (characterInformation != null) return characterInformation.Wisdom; else return 0; }
        set { characterInformation.Wisdom = value; }
    }
    public int Luck
    {
        get { if (characterInformation != null) return characterInformation.Luck; else return 0; }
        set { characterInformation.Luck = value; }
    }
    public int Perception
    {
        get { if (characterInformation != null) return characterInformation.Perception; else return 0; }
        set { characterInformation.Perception = value; }
    }
    public int Strength
    {
        get { if (characterInformation != null) return characterInformation.Strength; else return 0; }
        set { characterInformation.Strength = value; }
    }
    #endregion

    #region fightingData
    public int Metal
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Metal + equipFightingData.Metal + buffFightingData.Metal, 0); else return 0; }
        set { fightingData.Metal = value; }
    }
    public int Wood
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Wood + equipFightingData.Wood + buffFightingData.Wood, 0); else return 0; }
        set { fightingData.Wood = value; }
    }
    public int Water
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Water + equipFightingData.Water + buffFightingData.Water, 0); else return 0; }
        set { fightingData.Water = value; }
    }
    public int Fire
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Fire + equipFightingData.Fire + buffFightingData.Fire, 0); else return 0; }
        set { fightingData.Fire = value; }
    }
    public int Ground
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Ground + equipFightingData.Ground + buffFightingData.Ground, 0); else return 0; }
        set { fightingData.Ground = value; }
    }
    public int Lunar_Solar
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Lunar_Solar + equipFightingData.Lunar_Solar + buffFightingData.Lunar_Solar, 0); else return 0; }
        set { fightingData.Lunar_Solar = value; }
    }
    public int MaxHealth
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.maxHealth + equipFightingData.maxHealth + buffFightingData.maxHealth, 0); else return 0; }
        set { fightingData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (fightingData != null) return fightingData.currentHealth; else return 0; }
        set { fightingData.currentHealth = value; }
    }
    public int MaxVigor
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.maxVigor + equipFightingData.maxVigor + buffFightingData.maxVigor, 0); else return 0; }
        set { fightingData.maxVigor = value; }
    }
    public int CurrentVigor
    {
        get { if (fightingData != null) return fightingData.currentVigor; else return 0; }
        set { fightingData.currentVigor = value; }
    }
    public int MaxWound
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.maxWound + equipFightingData.maxWound + buffFightingData.maxWound, 0); else return 0; }
        set { fightingData.maxWound = value; }
    }
    public int CurrentWound
    {
        get { if (fightingData != null) return fightingData.currentWound; else return 0; }
        set { fightingData.currentWound = value; }
    }
    public int MaxQi
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.maxQi + equipFightingData.maxQi + buffFightingData.maxQi, 0); else return 0; }
        set { fightingData.maxQi = value; }
    }
    public int CurrentQi
    {
        get { if (fightingData != null) return fightingData.currentQi; else return 0; }
        set { fightingData.currentQi = value; }
    }
    public int MaxMorale
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.maxMorale + equipFightingData.maxMorale + buffFightingData.maxMorale, 0); else return 0; }
        set { fightingData.maxMorale = value; }
    }
    public int CurrentMorale
    {
        get { if (fightingData != null) return fightingData.currentMorale; else return 0; }
        set { fightingData.currentMorale = value; }
    }
    public int Argility
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Argility + equipFightingData.Argility + buffFightingData.Argility, 0); else return 0; }
        set { fightingData.Argility = value; }
    }
    public int Resilience
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Resilience + equipFightingData.Resilience + buffFightingData.Resilience, 0); else return 0; }
        set { fightingData.Resilience = value; }
    }
    public int Speed
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.speed + equipFightingData.speed + buffFightingData.speed, 0); else return 0; }
        set { fightingData.speed = value; }
    }
    public int Attack
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Attack + equipFightingData.Attack + buffFightingData.Attack, 0); else return 0; }
        set { fightingData.Attack = value; }
    }
    public int AttackAccuracy
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.attackAccuracy + equipFightingData.attackAccuracy + buffFightingData.attackAccuracy, 0); else return 0; }
        set { fightingData.attackAccuracy = value; }
    }
    public int CreateWound
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.createWound + equipFightingData.createWound + buffFightingData.createWound, 0); else return 0; }
        set { fightingData.createWound = value; }
    }
    public int Penetrate
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Penetrate + equipFightingData.Penetrate + buffFightingData.Penetrate, 0); else return 0; }
        set { fightingData.Penetrate = value; }
    }
    public float AttackCooling
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.attackCooling + equipFightingData.attackCooling + buffFightingData.attackCooling, 0); else return 0; }
        set { fightingData.attackCooling = value; }
    }
    
    public float MinimumRange
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.minimumRange + equipFightingData.minimumRange + buffFightingData.minimumRange, 0); else return 0; }
        set { fightingData.minimumRange = value; }
    }
    public float MaximumRange
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.maximumRange + equipFightingData.maximumRange + buffFightingData.maximumRange, 0); else return 0; }
        set { fightingData.maximumRange = value; }
    }
    public int CriticalPoint
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.criticalPoint + equipFightingData.criticalPoint + buffFightingData.criticalPoint, 0); else return 0; }
        set { fightingData.criticalPoint = value; }
    }
    public float Criticalmutiple
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.criticalMultiple + equipFightingData.criticalMultiple + buffFightingData.criticalMultiple, 0); else return 0; }
        set { fightingData.criticalMultiple = value; }
    }
    public float Fatal_Enhancement
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.fatal_Enhancement + equipFightingData.fatal_Enhancement + buffFightingData.fatal_Enhancement, 0); else return 0; }
        set { fightingData.fatal_Enhancement = value; }
    }

    public int Defense
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.Defense + equipFightingData.Defense + buffFightingData.Defense, 0); else return 0; }
        set { fightingData.Defense = value; }
    }
    public int PenetrateDefense
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.penetrateDefense + equipFightingData.penetrateDefense + buffFightingData.penetrateDefense, 0); else return 0; }
        set { fightingData.penetrateDefense = value; }
    }
    public float CriticalDefense
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.criticalDefense + equipFightingData.criticalDefense + buffFightingData.criticalDefense, 0); else return 0; }
        set { fightingData.criticalDefense = value; }
    }
    public float FatalDefense
    {
        get { if (fightingData != null) return Mathf.Max(fightingData.fatalDefense + equipFightingData.fatalDefense + buffFightingData.fatalDefense, 0); else return 0; }
        set { fightingData.fatalDefense = value; }
    }

    public float Continuous_DamageRate
    {
        get { if (fightingData != null) return fightingData.continuous_DamageRate + equipFightingData.continuous_DamageRate + buffFightingData.continuous_DamageRate; else return 0; }
        set { fightingData.continuous_DamageRate = value; }
    }
    public float Continuous_AttackRate
    {
        get { if (fightingData != null) return fightingData.continuous_AttackRate + equipFightingData.continuous_AttackRate + buffFightingData.continuous_AttackRate; else return 0; }
        set { fightingData.continuous_AttackRate = value; }
    }
    public float WoundRecovery
    {
        get { if (fightingData != null) return fightingData.woundRecovery + equipFightingData.woundRecovery + buffFightingData.woundRecovery; else return 0; }
        set { fightingData.woundRecovery = value; }
    }
    public float SkillCooling
    {
        get { if (fightingData != null) return fightingData.skillCooling; else return 0; }
        set { fightingData.skillCooling = value; }
    }
    public float ParryCoolDown
    {
        get { if (fightingData != null) return fightingData.parryCoolDown; else return 0; }
        set { fightingData.parryCoolDown = value; }
    }

    public List<Buff_SO> BuffList
    {
        get { if (fightingData != null) return fightingData.buffList; else return null; }
    }
    #endregion

    #region EffectDetection
    [HideInInspector]
    public bool isCritical;
    [HideInInspector]
    public bool isFatal;
    [HideInInspector]
    public bool isConDamage;
    [HideInInspector]
    public bool isConAttack;
    [HideInInspector]
    public int attackCount;

    public bool CheckIsFatal(int currentWound, int maxWound)
    {
        return currentWound >= maxWound;
    }

    public void SetUndefeated(bool undefeated)
    {
        isUndefeated = undefeated;
    }

    public void SetComfirmDodged(bool dodge)
    {
        isDodged = dodge;
    }

    #endregion

    #region Combat
    public void FinalDamage(CharacterInformation attacker, CharacterInformation defender)
    {
        if (CheckDodged(attacker, defender))
        {
            EventHandler.CallDamageTextPopEvent(defender.transform, 0, AttackEffectType.Dodged);
            return;
        }

        // Calculate the penetrate diffence;
        int penetrateRate = (int)(Mathf.Max((attacker.Penetrate - defender.PenetrateDefense), 0)/Settings.penetrateConstant);

        float damage = Mathf.Max((attacker.Attack * (1 + Random.Range(-0.05f, 0.05f))) - defender.Defense * (1 - penetrateRate),0);

        if (CriticalDamage(attacker, defender, damage))
        {
            EventHandler.CallDamageTextPopEvent(defender.transform, (int)damage, AttackEffectType.Critical);
        }
        else
        {
            EventHandler.CallDamageTextPopEvent(defender.transform, (int)damage, AttackEffectType.Normal);
        }
        CurrentHealth = Mathf.Max(CurrentHealth - (int)damage, 0);
        ContinuousDamage(attacker, defender, damage);
        
        //GameManager.Instance.initDamageText(defender.transform, (int)damage);
        
        //Debug.Log(CurrentHealth);
        defender.CurrentWound += attacker.CreateWound;
        defender.woundChange?.Invoke(defender);
        if(CheckIsFatal(CurrentWound, MaxWound))
        StartCoroutine(CalculateFatal(attacker.Fatal_Enhancement,defender.FatalDefense,defender));
        //Instantiate the damage each time
    }
    private void ContinuousDamage(CharacterInformation attacker, CharacterInformation defender, float damage)
    {
        
        if (attacker.isConDamage)
        {
            float finalConDamage = (1 + Random.Range(-0.1f, 0.1f)) * damage;
            defender.CurrentHealth = Mathf.Max(defender.CurrentHealth - (int)finalConDamage, 0);
            //Instantiate Damage;
            //GameManager.Instance.initDamageText(defender.transform, (int)damage);
            EventHandler.CallDamageTextPopEvent(defender.transform, (int)damage, AttackEffectType.continuousDamage);
        }
        return;
    }
    
    private bool CriticalDamage(CharacterInformation attacker, CharacterInformation defender, float damage)
    {
        if (attacker.isCritical)
        {
            damage *= Mathf.Max((1 + attacker.Criticalmutiple - defender.CriticalDefense), 1);
            defender.GetComponent<Animator>().SetTrigger("Hurt");
            defender.criticalShakeEvent?.Invoke();
            return true;
        }
        return false;
    }

    private bool CheckDodged(CharacterInformation attacker, CharacterInformation defender)
    {
        if ((defender.Argility - attacker.AttackAccuracy) / Settings.dodgeConstant >= Random.Range(0, 1) || defender.isDodged)
        {
            return true;
        }
        return false;
    }

    public IEnumerator CalculateFatal(float fatalEnhance, float fatalDefense,CharacterInformation defender)
    {
        while(CheckIsFatal(CurrentWound, MaxWound))
        {
            UpdateFatal(fatalEnhance, fatalDefense);
            yield return null;
            CurrentWound -= MaxWound;
            defender.healthChange?.Invoke(defender);
            defender.qiChange?.Invoke(defender);
            defender.woundChange?.Invoke(defender);
            defender.moraleChange?.Invoke(defender);
            yield return null;
        }
    }

    private void UpdateFatal(float fatalEnhance, float fatalDefense)
    {
        float finalFatal = Mathf.Max((fatalEnhance-fatalDefense + 0.2f), 0.2f);
        MaxHealth = Mathf.Max(MaxHealth - (int)(fightingData.maxHealth * finalFatal), 0);
        MaxVigor = Mathf.Max(MaxVigor - (int)(fightingData.maxVigor * finalFatal), 0);
        MaxQi = Mathf.Max(MaxQi - (int)(fightingData.maxQi * finalFatal), 0);
        MaxMorale = Mathf.Max(MaxMorale - (int)(fightingData.maxMorale * finalFatal), 0);
        Speed = Mathf.Max(Speed - (int)(fightingData.speed * finalFatal), 0);
        Argility = Mathf.Max(Argility - (int)(fightingData.Argility * finalFatal), 0);
        Resilience = Mathf.Max(Resilience - (int)(fightingData.Resilience * finalFatal), 0);
        SkillCooling = Mathf.Max(SkillCooling - fightingData.skillCooling * finalFatal, 0);
        Attack = Mathf.Max(Attack - (int)(fightingData.Attack * finalFatal), 0);
        CreateWound = Mathf.Max(CreateWound - (int)(fightingData.createWound * finalFatal), 0);
        Penetrate = Mathf.Max(Penetrate - (int)(fightingData.Penetrate * finalFatal), 0);
        CriticalPoint = Mathf.Max(CriticalPoint - (int)(fightingData.criticalPoint * finalFatal), 0);
        Criticalmutiple = Mathf.Max(Criticalmutiple - fightingData.criticalMultiple * finalFatal, 0);
        Fatal_Enhancement = Mathf.Max(Fatal_Enhancement - fightingData.fatal_Enhancement * finalFatal, 0);
        Continuous_DamageRate = Mathf.Max(Continuous_DamageRate - fightingData.continuous_DamageRate * finalFatal, 0);
        Continuous_AttackRate = Mathf.Max(Continuous_AttackRate - fightingData.continuous_AttackRate * finalFatal, 0);
        Defense = Mathf.Max(Defense - (int)(fightingData.Defense * finalFatal), 0);
        PenetrateDefense = Mathf.Max(PenetrateDefense - (int)(fightingData.penetrateDefense * finalFatal), 0);
        CriticalDefense = Mathf.Max(CriticalDefense - fightingData.criticalDefense * finalFatal, 0);
        FatalDefense = Mathf.Max(FatalDefense - fightingData.fatalDefense * finalFatal, 0);
        CheckExceedLimit();
    }

    public void CheckExceedLimit()
    {
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        if (CurrentVigor > MaxVigor)
            CurrentVigor = MaxVigor;
        if (CurrentQi > MaxQi)
            CurrentQi = MaxQi;
        if (CurrentMorale > MaxMorale)
            CurrentMorale = MaxMorale;
    }
    #endregion
    #region Inventory
    public void EquipItem(EquipItemDetails equipment)
    {
        equipFightingData.Metal += equipment.EquipData.Metal;
        equipFightingData.Wood += equipment.EquipData.Wood;
        equipFightingData.Water += equipment.EquipData.Water;
        equipFightingData.Fire += equipment.EquipData.Fire;
        equipFightingData.Ground += equipment.EquipData.Ground;
        equipFightingData.Lunar_Solar += equipment.EquipData.Lunar_Solar;

        equipFightingData.maxHealth += equipment.EquipData.maxHealth;
        equipFightingData.maxVigor += equipment.EquipData.maxVigor;
        equipFightingData.maxWound += equipment.EquipData.maxWound;
        equipFightingData.maxQi += equipment.EquipData.maxQi;
        equipFightingData.maxMorale += equipment.EquipData.maxMorale;
        equipFightingData.speed += equipment.EquipData.speed;
        equipFightingData.Argility += equipment.EquipData.Argility;
        equipFightingData.Resilience += equipment.EquipData.Resilience;

        equipFightingData.minimumRange += equipment.EquipData.minimumRange;
        equipFightingData.maximumRange += equipment.EquipData.maximumRange;
        equipFightingData.Attack += equipment.EquipData.Attack;
        equipFightingData.attackCooling += equipment.EquipData.attackCooling;
        equipFightingData.attackAccuracy += equipment.EquipData.attackAccuracy;
        equipFightingData.Penetrate += equipment.EquipData.Penetrate;
        equipFightingData.createWound += equipment.EquipData.createWound;
        equipFightingData.criticalPoint += equipment.EquipData.criticalPoint;
        equipFightingData.criticalMultiple += equipment.EquipData.criticalMultiple;
        equipFightingData.fatal_Enhancement += equipment.EquipData.fatal_Enhancement;

        equipFightingData.Defense += equipment.EquipData.Defense;
        equipFightingData.penetrateDefense += equipment.EquipData.penetrateDefense;
        equipFightingData.criticalDefense += equipment.EquipData.criticalDefense;
        equipFightingData.fatalDefense += equipment.EquipData.fatalDefense;

        EventHandler.CallUpdateCharacterInformationUIEvent();
        FightingDataDetailsUI.Instance.UpdateFightingDataUI(equipFightingData, CharacterFightingDataType.Equip);
    }

    public void TakeOffItem(EquipItemDetails equipment)
    {
        equipFightingData.Metal -= equipment.EquipData.Metal;
        equipFightingData.Wood -= equipment.EquipData.Wood;
        equipFightingData.Water -= equipment.EquipData.Water;
        equipFightingData.Fire -= equipment.EquipData.Fire;
        equipFightingData.Ground -= equipment.EquipData.Ground;
        equipFightingData.Lunar_Solar -= equipment.EquipData.Lunar_Solar;

        equipFightingData.maxHealth -= equipment.EquipData.maxHealth;
        equipFightingData.maxVigor -= equipment.EquipData.maxVigor;
        equipFightingData.maxWound -= equipment.EquipData.maxWound;
        equipFightingData.maxQi -= equipment.EquipData.maxQi;
        equipFightingData.maxMorale -= equipment.EquipData.maxMorale;
        equipFightingData.speed -= equipment.EquipData.speed;
        equipFightingData.Argility -= equipment.EquipData.Argility;
        equipFightingData.Resilience -= equipment.EquipData.Resilience;

        equipFightingData.minimumRange -= equipment.EquipData.minimumRange;
        equipFightingData.maximumRange -= equipment.EquipData.maximumRange;
        equipFightingData.Attack -= equipment.EquipData.Attack;
        equipFightingData.attackCooling -= equipment.EquipData.attackCooling;
        equipFightingData.attackAccuracy -= equipment.EquipData.attackAccuracy;
        equipFightingData.Penetrate -= equipment.EquipData.Penetrate;
        equipFightingData.createWound -= equipment.EquipData.createWound;
        equipFightingData.criticalPoint -= equipment.EquipData.criticalPoint;
        equipFightingData.criticalMultiple -= equipment.EquipData.criticalMultiple;
        equipFightingData.fatal_Enhancement -= equipment.EquipData.fatal_Enhancement;

        equipFightingData.Defense -= equipment.EquipData.Defense;
        equipFightingData.penetrateDefense -= equipment.EquipData.penetrateDefense;
        equipFightingData.criticalDefense -= equipment.EquipData.criticalDefense;
        equipFightingData.fatalDefense -= equipment.EquipData.fatalDefense;

        EventHandler.CallUpdateCharacterInformationUIEvent();
        FightingDataDetailsUI.Instance.UpdateFightingDataUI(equipFightingData, CharacterFightingDataType.Equip);
        StartCoroutine(CalculateFatal(0, 0, this));
    }

    public void ChangeItem(CharacterFightingData_SO currentEquip, CharacterFightingData_SO newEquip)
    {
        equipFightingData.Metal = equipFightingData.Metal + newEquip.Metal - currentEquip.Metal;
        equipFightingData.Wood = equipFightingData.Wood + newEquip.Wood - currentEquip.Wood;
        equipFightingData.Water = equipFightingData.Water + newEquip.Water - currentEquip.Water;
        equipFightingData.Fire = equipFightingData.Fire + newEquip.Fire - currentEquip.Fire;
        equipFightingData.Ground = equipFightingData.Ground + newEquip.Ground - currentEquip.Ground;
        equipFightingData.Lunar_Solar = equipFightingData.Lunar_Solar + newEquip.Lunar_Solar - currentEquip.Lunar_Solar;

        equipFightingData.maxHealth = equipFightingData.maxHealth + newEquip.maxHealth - currentEquip.maxHealth;
        equipFightingData.maxVigor = equipFightingData.maxVigor + newEquip.maxVigor - currentEquip.maxVigor;
        equipFightingData.maxWound = equipFightingData.maxWound + newEquip.maxWound - currentEquip.maxWound;
        equipFightingData.maxQi = equipFightingData.maxQi + newEquip.maxQi - currentEquip.maxQi;
        equipFightingData.maxMorale = equipFightingData.maxMorale + newEquip.maxMorale - currentEquip.maxMorale;
        equipFightingData.speed = equipFightingData.speed + newEquip.speed - currentEquip.speed;
        equipFightingData.Argility = equipFightingData.Argility + newEquip.Argility - currentEquip.Argility;
        equipFightingData.Resilience = equipFightingData.Resilience + newEquip.Resilience - currentEquip.Resilience;

        equipFightingData.minimumRange = equipFightingData.minimumRange + newEquip.minimumRange - currentEquip.minimumRange;
        equipFightingData.maximumRange = equipFightingData.maximumRange + newEquip.maximumRange - currentEquip.maximumRange;
        equipFightingData.Attack = equipFightingData.Attack + newEquip.Attack - currentEquip.Attack;
        equipFightingData.attackCooling = equipFightingData.attackCooling + newEquip.attackCooling - currentEquip.attackCooling;
        equipFightingData.attackAccuracy = equipFightingData.attackAccuracy + newEquip.attackAccuracy - currentEquip.attackAccuracy;
        equipFightingData.Penetrate = equipFightingData.Penetrate + newEquip.Penetrate - currentEquip.Penetrate;
        equipFightingData.createWound = equipFightingData.createWound + newEquip.createWound - currentEquip.createWound;
        equipFightingData.criticalPoint = equipFightingData.criticalPoint + newEquip.criticalPoint - currentEquip.criticalPoint;
        equipFightingData.criticalMultiple = equipFightingData.criticalMultiple + newEquip.criticalMultiple - currentEquip.criticalMultiple;
        equipFightingData.fatal_Enhancement = equipFightingData.fatal_Enhancement + newEquip.fatal_Enhancement - currentEquip.fatal_Enhancement;

        equipFightingData.Defense = equipFightingData.Defense + newEquip.Defense - currentEquip.Defense;
        equipFightingData.penetrateDefense = equipFightingData.penetrateDefense + newEquip.penetrateDefense - currentEquip.penetrateDefense;
        equipFightingData.criticalDefense = equipFightingData.criticalDefense + newEquip.criticalDefense - currentEquip.criticalDefense;
        equipFightingData.fatalDefense = equipFightingData.fatalDefense + newEquip.fatalDefense - currentEquip.fatalDefense;

        EventHandler.CallUpdateCharacterInformationUIEvent();
        StartCoroutine(CalculateFatal(0, 0, this));
        FightingDataDetailsUI.Instance.UpdateFightingDataUI(equipFightingData, CharacterFightingDataType.Equip);
    }
    #endregion


    #region Recovery
    private void WoundRecoveryProcess()
    {
        lastWoundRecoverTime -= Time.deltaTime * WoundRecovery;
        if (lastWoundRecoverTime < 0)
        {
            lastWoundRecoverTime = Settings.WoundRecoveryTime;
            CurrentWound = Mathf.Max(CurrentWound - 1, 0);
            woundChange.Invoke(this);
        }
    }
    public void RecoverFatal(float recover)
    {
        MaxHealth += (int)(fightingData.maxHealth * recover);
        MaxVigor += (int)(fightingData.maxVigor * recover);
        MaxQi += (int)(fightingData.maxQi * recover);
        MaxMorale += (int)(fightingData.maxMorale * recover);
        Speed += (int)(fightingData.speed * recover);
        Argility += (int)(fightingData.Argility * recover);
        Resilience += (int)(fightingData.Resilience * recover);
        SkillCooling += (int)(fightingData.skillCooling * recover);
        Attack += (int)(fightingData.Attack * recover);
        CreateWound += (int)(fightingData.createWound * recover);
        Penetrate += (int)(fightingData.Penetrate * recover);
        CriticalPoint += (int)(fightingData.criticalPoint * recover);
        Criticalmutiple += (int)(fightingData.criticalMultiple * recover);
        Fatal_Enhancement += (int)(fightingData.fatal_Enhancement * recover);
        Continuous_DamageRate += (int)(fightingData.continuous_DamageRate * recover);
        Continuous_AttackRate += (int)(fightingData.continuous_AttackRate * recover);
        Defense += (int)(fightingData.Defense * recover);
        PenetrateDefense += (int)(fightingData.penetrateDefense * recover);
        CriticalDefense += (int)(fightingData.criticalDefense * recover);
        FatalDefense += (int)(fightingData.fatalDefense * recover);
    }

    public void VigorRecovery()
    {
        lastVigorRocoverTime -= Time.deltaTime;
        if (lastVigorRocoverTime <= 0)
        {
            lastVigorRocoverTime = Settings.VigorRecoverTime;
            float correction = Random.Range(-0.05f,0.05f);
            CurrentVigor = Mathf.Min(CurrentVigor + (int)(MaxVigor / 10 * (1 + correction)), MaxVigor);
            vigorChange.Invoke(this);
        }
        
    }
    #endregion


}
