using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterInformation : MonoBehaviour
{
    public CharacterInformation_SO templateInformation;
    public CharacterInformation_SO characterInformation;
    public CharacterFightingData_SO templateFightingData;
    public CharacterFightingData_SO fightingData;

    private float lastWoundRecoverTime;

    private void Awake()
    {
        if (templateInformation != null)
            characterInformation = Instantiate(templateInformation);
        if (templateFightingData != null)
            fightingData = Instantiate(templateFightingData);
    }
    private void Update()
    {
        woundRecovery();
    }
    #region basicInfor
    public int prestigeLevel
    {
        get { if (characterInformation != null) return characterInformation.prestigeLevel; else return 0; }
        set { characterInformation.prestigeLevel = value; }
    }
    public int currentPrestige
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

    public bool checkIsFatal(int currentWound, int maxWound)
    {
        return currentWound >= maxWound;
    }

    #endregion

    #region Combat
    public void finalDamage(CharacterInformation attacker, CharacterInformation defender)
    {
        int attack = attacker.Attack;
        //Debug.Log(attack);
        int defense = defender.Defense;
        // Calculate the penetrate diffence;
        int penetrateRate = (int)(Mathf.Max((attacker.Penetrate - defender.PenetrateDefense), 0)/Settings.penetrateConstant);

        float damage = Mathf.Max((attack * (1 + Random.Range(-0.05f, 0.05f))) - defense * (1 - penetrateRate),0);
        //Debug.Log(characterInformation);
        if (attacker.isCritical)
        {
            damage *=Mathf.Max((1 + attacker.Criticalmutiple - defender.CriticalDefense),1);
            defender.GetComponent<Animator>().SetTrigger("Hurt");
            //Debug.Log("Critical"+(int)damage);
        }
        int finalDamage = (int)damage;
        
        int ConDamage = continuousDamage(attacker, damage);
        CurrentHealth = Mathf.Max(CurrentHealth - finalDamage, 0);
        Debug.Log(CurrentHealth);
        defender.CurrentWound += attacker.CreateWound;
        StartCoroutine(calculateFatal(attacker.Fatal_Enhancement,defender.FatalDefense));
        //Instantiate the damage each time
        if (ConDamage > 0)
            CurrentHealth = Mathf.Max(CurrentHealth - ConDamage, 0);
        //TODO:UpdateUI/UpdateXP
    }
    private int continuousDamage(CharacterInformation attacker, float damage)
    {
        
        if (attacker.isConDamage)
        {
            float finalConDamage = (1 + Random.Range(-0.1f, 0.1f)) * damage;

            //Debug.Log("ConDamage" + damage + finalConDamage);
            return (int)finalConDamage;
        }
        return 0;
    }
    
    private IEnumerator calculateFatal(float fatalEnhance, float fatalDefense)
    {
        while(checkIsFatal(CurrentWound, MaxWound))
        {
            updateFatal(fatalEnhance, fatalDefense);
            yield return null;
            CurrentWound -= MaxWound;
            FatalLevel = (int)((MaxHealth / (float)templateFightingData.maxHealth) * 5);
        }
        
        
    }

    private void updateFatal(float fatalEnhance, float fatalDefense)
    {
        float finalFatal = Mathf.Max((fatalEnhance-fatalDefense + 0.2f), 0.2f);
        MaxHealth -= (int)(templateFightingData.maxHealth * finalFatal);
        MaxVigor -= (int)(templateFightingData.maxVigor * finalFatal);
        MaxQi -= (int)(templateFightingData.maxQi * finalFatal);
        MaxMorale -= (int)(templateFightingData.maxMorale * finalFatal);
        Speed -= (int)(templateFightingData.speed * finalFatal);
        Argility -= (int)(templateFightingData.Argility * finalFatal);
        Resilience -= (int)(templateFightingData.Resilience * finalFatal);
        SkillCooling -= templateFightingData.skillCooling * finalFatal;
        Attack -= (int)(templateFightingData.Attack * finalFatal);
        CreateWound -= (int)(templateFightingData.createWound * finalFatal);
        Penetrate -= (int)(templateFightingData.Penetrate * finalFatal);
        CriticalPoint -= (int)(templateFightingData.criticalPoint * finalFatal);
        //AttackCooling -= templateFightingData.AttackCooling * finalFatal;
        Criticalmutiple -= templateFightingData.criticalmutiple * finalFatal;
        Fatal_Enhancement -= templateFightingData.fatal_Enhancement * finalFatal;
        Continuous_DamageRate -= templateFightingData.continuous_DamageRate * finalFatal;
        Continuous_AttackRate -= templateFightingData.continuous_AttackRate * finalFatal;
        Defense -= (int)(templateFightingData.Defense * finalFatal);
        PenetrateDefense -= (int)(templateFightingData.penetrateDefense * finalFatal);
        CriticalDefense -= templateFightingData.criticalDefense * finalFatal;
        FatalDefense -= templateFightingData.fatalDefense * finalFatal;
        checkExceedLimit();
    }

    private void checkExceedLimit()
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
    private void woundRecovery()
    {
        lastWoundRecoverTime -= Time.deltaTime * WoundRecovery;
        if (lastWoundRecoverTime < 0)
        {
            lastWoundRecoverTime = Settings.WoundRecoveryTime;
            CurrentWound--;
        }
    }
    public void recoverFatal()
    {
        float recover = FatalLevel * 0.2f;
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
        //AttackCooling = (int)(templateFightingData.AttackCooling * recover);
        Criticalmutiple = (int)(templateFightingData.criticalmutiple * recover);
        Fatal_Enhancement = (int)(templateFightingData.fatal_Enhancement * recover);
        Continuous_DamageRate = (int)(templateFightingData.continuous_DamageRate * recover);
        Continuous_AttackRate = (int)(templateFightingData.continuous_AttackRate * recover);
        Defense = (int)(templateFightingData.Defense * recover);
        PenetrateDefense = (int)(templateFightingData.penetrateDefense * recover);
        CriticalDefense = (int)(templateFightingData.criticalDefense * recover);
        FatalDefense = (int)(templateFightingData.fatalDefense * recover);
    }
    #endregion
}
