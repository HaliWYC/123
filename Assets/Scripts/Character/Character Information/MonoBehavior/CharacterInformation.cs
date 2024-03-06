using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInformation : MonoBehaviour
{
    public CharacterInformation_SO templateInformation;
    public CharacterInformation_SO characterInformation;
    public CharacterFightingData_SO templateFightingData;
    public CharacterFightingData_SO fightingData;

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
    #region basicInfor
    public int PrestigeLevel
    {
        get { if (characterInformation != null) return characterInformation.prestigeLevel; else return 0; }
        set { characterInformation.prestigeLevel = value; }
    }
    public int CurrentPrestige
    {
        get { if (characterInformation != null) return characterInformation.currentPrestige; else return 0; }
        set { characterInformation.currentPrestige = value; }
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
        get { if (characterInformation != null) return characterInformation.Wisedom; else return 0; }
        set { characterInformation.Wisedom = value; }
    }
    public int Luck
    {
        get { if (characterInformation != null) return characterInformation.Luck; else return 0; }
        set { characterInformation.Luck = value; }
    }
    public int Understanding
    {
        get { if (characterInformation != null) return characterInformation.Understanding; else return 0; }
        set { characterInformation.Understanding = value; }
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
        get { if (fightingData != null) return fightingData.Metal; else return 0; }
        set { fightingData.Metal = value; }
    }
    public int Wood
    {
        get { if (fightingData != null) return fightingData.Wood; else return 0; }
        set { fightingData.Wood = value; }
    }
    public int Water
    {
        get { if (fightingData != null) return fightingData.Water; else return 0; }
        set { fightingData.Water = value; }
    }
    public int Fire
    {
        get { if (fightingData != null) return fightingData.Fire; else return 0; }
        set { fightingData.Fire = value; }
    }
    public int Ground
    {
        get { if (fightingData != null) return fightingData.Ground; else return 0; }
        set { fightingData.Ground = value; }
    }
    public int Lunar_Solar
    {
        get { if (fightingData != null) return fightingData.Lunar_Solar; else return 0; }
        set { fightingData.Lunar_Solar = value; }
    }
    public int MaxHealth
    {
        get { if (fightingData != null) return fightingData.maxHealth; else return 0; }
        set { fightingData.maxHealth = value; }
    }
    public int CurrentHealth
    {
        get { if (fightingData != null) return fightingData.currentHealth; else return 0; }
        set { fightingData.currentHealth = value; }
    }
    public int MaxVigor
    {
        get { if (fightingData != null) return fightingData.maxVigor; else return 0; }
        set { fightingData.maxVigor = value; }
    }
    public int CurrentVigor
    {
        get { if (fightingData != null) return fightingData.currentVigor; else return 0; }
        set { fightingData.currentVigor = value; }
    }
    public int MaxWound
    {
        get { if (fightingData != null) return fightingData.maxWound; else return 0; }
        set { fightingData.maxWound = value; }
    }
    public int CurrentWound
    {
        get { if (fightingData != null) return fightingData.currentWound; else return 0; }
        set { fightingData.currentWound = value; }
    }
    public int MaxQi
    {
        get { if (fightingData != null) return fightingData.maxQi; else return 0; }
        set { fightingData.maxQi = value; }
    }
    public int CurrentQi
    {
        get { if (fightingData != null) return fightingData.currentQi; else return 0; }
        set { fightingData.currentQi = value; }
    }
    public int MaxMorale
    {
        get { if (fightingData != null) return fightingData.maxMorale; else return 0; }
        set { fightingData.maxMorale = value; }
    }
    public int CurrentMorale
    {
        get { if (fightingData != null) return fightingData.currentMorale; else return 0; }
        set { fightingData.currentMorale = value; }
    }
    public int Argility
    {
        get { if (fightingData != null) return fightingData.Argility; else return 0; }
        set { fightingData.Argility = value; }
    }
    public int Resilience
    {
        get { if (fightingData != null) return fightingData.Resilience; else return 0; }
        set { fightingData.Resilience = value; }
    }
    public int Speed
    {
        get { if (fightingData != null) return fightingData.speed; else return 0; }
        set { fightingData.speed = value; }
    }
    public int FatalLevel
    {
        get { if (fightingData != null) return fightingData.fatalLevel; else return 0; }
        set { fightingData.fatalLevel = value; }
    }
    public float WoundRecovery
    {
        get { if (fightingData != null) return fightingData.woundRecovery; else return 0; }
        set { fightingData.woundRecovery = value; }
    }
    public float SkillCooling
    {
        get { if (fightingData != null) return fightingData.skillCooling; else return 0; }
        set { fightingData.skillCooling = value; }
    }
    
    public int Attack
    {
        get { if (fightingData != null) return fightingData.Attack; else return 0; }
        set { fightingData.Attack = value; }
    }
    public int AttackAccuracy
    {
        get { if (fightingData != null) return fightingData.attackAccuracy; else return 0; }
        set { fightingData.attackAccuracy = value; }
    }
    public int CreateWound
    {
        get { if (fightingData != null) return fightingData.createWound; else return 0; }
        set { fightingData.createWound = value; }
    }
    public int Penetrate
    {
        get { if (fightingData != null) return fightingData.Penetrate; else return 0; }
        set { fightingData.Penetrate = value; }
    }
    public float AttackCooling
    {
        get { if (fightingData != null) return fightingData.AttackCooling; else return 0; }
        set { fightingData.AttackCooling = value; }
    }
    public float Cooling
    {
        get { if (fightingData != null) return fightingData.skillCooling; else return 0; }
        set { fightingData.skillCooling = value; }
    }
    public float MeleeRange
    {
        get { if (fightingData != null) return fightingData.meleeRange; else return 0; }
        set { fightingData.meleeRange = value; }
    }
    public float RangedRange
    {
        get { if (fightingData != null) return fightingData.rangedRange; else return 0; }
        set { fightingData.rangedRange = value; }
    }
    public int CriticalPoint
    {
        get { if (fightingData != null) return fightingData.criticalPoint; else return 0; }
        set { fightingData.criticalPoint = value; }
    }
    public float Criticalmutiple
    {
        get { if (fightingData != null) return fightingData.criticalmutiple; else return 0; }
        set { fightingData.criticalmutiple = value; }
    }
    public float Fatal_Enhancement
    {
        get { if (fightingData != null) return fightingData.fatal_Enhancement; else return 0; }
        set { fightingData.fatal_Enhancement = value; }
    }
    public float Continuous_DamageRate
    {
        get { if (fightingData != null) return fightingData.continuous_DamageRate; else return 0; }
        set { fightingData.continuous_DamageRate = value; }
    }
    public float Continuous_AttackRate
    {
        get { if (fightingData != null) return fightingData.continuous_AttackRate; else return 0; }
        set { fightingData.continuous_AttackRate = value; }
    }
    public int Defense
    {
        get { if (fightingData != null) return fightingData.Defense; else return 0; }
        set { fightingData.Defense = value; }
    }
    public int PenetrateDefense
    {
        get { if (fightingData != null) return fightingData.penetrateDefense; else return 0; }
        set { fightingData.penetrateDefense = value; }
    }
    public float CriticalDefense
    {
        get { if (fightingData != null) return fightingData.criticalDefense; else return 0; }
        set { fightingData.criticalDefense = value; }
    }
    public float FatalDefense
    {
        get { if (fightingData != null) return fightingData.fatalDefense; else return 0; }
        set { fightingData.fatalDefense = value; }
    }
    public float ParryCoolDown
    {
        get { if (fightingData != null) return fightingData.parryCoolDown; else return 0; }
        set { fightingData.parryCoolDown = value; }
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
            //Debug.Log("Critical"+(int)damage);
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
        MaxHealth = Mathf.Max(MaxHealth - (int)(templateFightingData.maxHealth * finalFatal), 0);
        MaxVigor = Mathf.Max(MaxVigor - (int)(templateFightingData.maxVigor * finalFatal), 0);
        MaxQi = Mathf.Max(MaxQi - (int)(templateFightingData.maxQi * finalFatal), 0);
        MaxMorale = Mathf.Max(MaxMorale - (int)(templateFightingData.maxMorale * finalFatal), 0);
        Speed = Mathf.Max(Speed - (int)(templateFightingData.speed * finalFatal), 0);
        Argility = Mathf.Max(Argility - (int)(templateFightingData.Argility * finalFatal), 0);
        Resilience = Mathf.Max(Resilience - (int)(templateFightingData.Resilience * finalFatal), 0);
        SkillCooling = Mathf.Max(SkillCooling - templateFightingData.skillCooling * finalFatal, 0);
        Attack = Mathf.Max(Attack - (int)(templateFightingData.Attack * finalFatal), 0);
        CreateWound = Mathf.Max(CreateWound - (int)(templateFightingData.createWound * finalFatal), 0);
        Penetrate = Mathf.Max(Penetrate - (int)(templateFightingData.Penetrate * finalFatal), 0);
        CriticalPoint = Mathf.Max(CriticalPoint - (int)(templateFightingData.criticalPoint * finalFatal), 0);
        //AttackCooling -= templateFightingData.AttackCooling * finalFatal;
        Criticalmutiple = Mathf.Max(Criticalmutiple - templateFightingData.criticalmutiple * finalFatal, 0);
        Fatal_Enhancement = Mathf.Max(Fatal_Enhancement - templateFightingData.fatal_Enhancement * finalFatal, 0);
        Continuous_DamageRate = Mathf.Max(Continuous_DamageRate - templateFightingData.continuous_DamageRate * finalFatal, 0);
        Continuous_AttackRate = Mathf.Max(Continuous_AttackRate - templateFightingData.continuous_AttackRate * finalFatal, 0);
        Defense = Mathf.Max(Defense - (int)(templateFightingData.Defense * finalFatal), 0);
        PenetrateDefense = Mathf.Max(PenetrateDefense - (int)(templateFightingData.penetrateDefense * finalFatal), 0);
        CriticalDefense = Mathf.Max(CriticalDefense - templateFightingData.criticalDefense * finalFatal, 0);
        FatalDefense = Mathf.Max(FatalDefense - templateFightingData.fatalDefense * finalFatal, 0);
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
        MaxHealth = (int)(templateFightingData.maxHealth * recover);
        MaxVigor = (int)(templateFightingData.maxVigor * recover);
        MaxQi = (int)(templateFightingData.maxQi * recover);
        MaxMorale = (int)(templateFightingData.maxMorale * recover);
        Speed = (int)(templateFightingData.speed * recover);
        Argility = (int)(templateFightingData.Argility * recover);
        Resilience = (int)(templateFightingData.Resilience * recover);
        SkillCooling = (int)(templateFightingData.skillCooling * recover);
        Attack = (int)(templateFightingData.Attack * recover);
        CreateWound = (int)(templateFightingData.createWound * recover);
        Penetrate = (int)(templateFightingData.Penetrate * recover);
        CriticalPoint = (int)(templateFightingData.criticalPoint * recover);
        Criticalmutiple = (int)(templateFightingData.criticalmutiple * recover);
        Fatal_Enhancement = (int)(templateFightingData.fatal_Enhancement * recover);
        Continuous_DamageRate = (int)(templateFightingData.continuous_DamageRate * recover);
        Continuous_AttackRate = (int)(templateFightingData.continuous_AttackRate * recover);
        Defense = (int)(templateFightingData.Defense * recover);
        PenetrateDefense = (int)(templateFightingData.penetrateDefense * recover);
        CriticalDefense = (int)(templateFightingData.criticalDefense * recover);
        FatalDefense = (int)(templateFightingData.fatalDefense * recover);
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
