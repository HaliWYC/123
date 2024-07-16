using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShanHai_IsolatedCity.Skill
{
    public class Skills : MonoBehaviour
    {
        public List<SkillDetails_SO> skillList;//SkillList on the character
        [SerializeField] private List<SkillDetails_SO> templateSkillList;//SkillList on the character used to clone
        public GameObject skiller;

        protected virtual void Awake()
        {
            if (templateSkillList != null)
            {
                foreach (SkillDetails_SO skillData in templateSkillList)
                {
                    if (skillData != null)
                        skillList.Add(Instantiate(skillData));
                }
            }
            skiller = transform.gameObject;
        }
        protected virtual void Update()
        {
            if (skillList != null)
                SkillCooling(skillList);
        }

        #region GetSkill
        public SkillDetails_SO GetSkillDataByName(string skillName)
        {
            foreach (SkillDetails_SO skillData in skillList)
            {
                return skillList.Find(i => i.SkillName == skillName);
            }
            return null;
        }

        public SkillDetails_SO GetSkillDataByProbability(float probability)
        {
            foreach (SkillDetails_SO skillData in skillList)
            {
                return skillList.Find(i => i.skillProbability <= probability);
            }
            return null;
        }

        public SkillDetails_SO GetSkillDataBySkillRange(float range)
        {
            foreach (SkillDetails_SO skillData in skillList)
            {
                return skillList.Find(i => i.skillRange >= range);
            }
            return null;
        }

        public SkillDetails_SO GetSkillDataByID(int skillID)
        {
            foreach (SkillDetails_SO skillData in skillList)
            {
                return skillList.Find(i => i.skillID == skillID);
            }
            return null;
        }
        #endregion


        public void SkillEvaluation(SkillDetails_SO currentSkill, CharacterInformation skiller, CharacterInformation defender)
        {
            SkillDetails_SO evaluateSkill = GetSkillDataByID(currentSkill.skillID);
            evaluateSkill.currentCoolDown = evaluateSkill.CoolDown;
            skiller.CurrentQi -= evaluateSkill.QiComsume;

            if (CheckDodged(skiller, defender, evaluateSkill.perfectAccurate))
            {
                EventHandler.CallDamageTextPopEvent(defender.transform, 0, AttackEffectType.Dodged);
                return;
            }
            BuffEvaluation(evaluateSkill, SkillExpIncrementCalculation(evaluateSkill), skiller, defender);
        }

        /// <summary>
        /// Check whether target can dodge the skill or not
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <param name="perfectAccurate"></param>
        /// <returns></returns>
        public bool CheckDodged(CharacterInformation attacker, CharacterInformation defender, bool perfectAccurate)
        {
            if (perfectAccurate)
                return false;
            if ((defender.Argility - attacker.AttackAccuracy) / Settings.skillDodgeConstant >= Random.Range(0, 1) || defender.isDodged)
            {

                return true;
            }
            return false;
        }

        

        public void SkillCooling(List<SkillDetails_SO> skillList)
        {
            foreach (SkillDetails_SO skillDetails in skillList)
            {
                if (skillDetails != null)
                    skillDetails.currentCoolDown -= Time.deltaTime;
            }
        }


        #region Proficiency
        public float SkillExpIncrementCalculation(SkillDetails_SO evaluateSkill)
        {
            float correctionValue = Random.Range(-0.01f, 0.01f);
            int proficiencyValue =
                evaluateSkill.skillProficiency switch
                {
                    Proficiency.一窍不通 => 1,
                    Proficiency.初窥门径 => 2,
                    Proficiency.一知半解 => 3,
                    Proficiency.半生不熟 => 4,
                    Proficiency.融会贯通 => 5,
                    Proficiency.游刃有余 => 6,
                    Proficiency.炉火纯青 => 8,
                    Proficiency.得心应手 => 10,
                    Proficiency.登峰造极 => 12,
                    _ => 15
                };
            return (0.2f + correctionValue) * proficiencyValue;
        }

        public void SkillExperienceChange(SkillDetails_SO skill, int exp)
        {
            if (skill.currentExp + exp >= skill.nextExp)
            {
                skill.currentExp = skill.currentExp + exp - skill.nextExp;
                skill.skillProficiency++;
                //Debug.Log(skill.skillProficiency);
                skill.nextExp = NextProficencyExp(skill.skillProficiency);

            }
            else
                skill.currentExp += exp;
        }
        

        private int NextProficencyExp(Proficiency proficiency)
        {
            switch (proficiency)
            {
                case Proficiency.一窍不通:
                    return 10;
                case Proficiency.初窥门径:
                    return 50;
                case Proficiency.一知半解:
                    return 100;
                case Proficiency.半生不熟:
                    return 200;
                case Proficiency.融会贯通:
                    return 500;
                case Proficiency.游刃有余:
                    return 1000;
                case Proficiency.炉火纯青:
                    return 1500;
                case Proficiency.得心应手:
                    return 2000;
                case Proficiency.登峰造极:
                    return 3000;
            }
            return -1;
        }
        #endregion

        #region BuffEvaluation
        /// <summary>
        /// This method is used to sort the bufflist of the skill considering the priority
        /// </summary>
        /// <param name="spellSkill"></param>
        /// <param name="SkillExpIncrement"></param>
        /// <param name="skiller"></param>
        /// <param name="defender"></param>
        public void BuffEvaluation(SkillDetails_SO spellSkill, float SkillExpIncrement, CharacterInformation skiller, CharacterInformation defender)
        {
            //TODO:后期加上如果Priority相同的情况
            spellSkill.buffList.Sort((x, y) => x.buffPriority.CompareTo(y.buffPriority));
            foreach (Buff_SO buff in spellSkill.buffList)
            {
                if (buff.target == BuffTarget.Self)
                    ClassifyBuff(buff, SkillExpIncrement, skiller);
                else if (buff.target == BuffTarget.Enemy)
                    ClassifyBuff(buff, SkillExpIncrement, defender);
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="SkillExpIncrement"></param>
        /// <param name="buffTarget"></param>
        public void ClassifyBuff(Buff_SO buff, float SkillExpIncrement, CharacterInformation buffTarget)
        {
            switch (buff.effectType)
            {
                case EffectType.Health:
                    buffTarget.gameObject.AddComponent<HealthBuff>().BuffSetUp(buff, SkillExpIncrement, buffTarget);
                    break;
                case EffectType.Wound:
                    break;
                case EffectType.Speed:
                    buffTarget.gameObject.AddComponent<SpeedBuff>().BuffSetUp(buff, SkillExpIncrement, buffTarget);
                    break;
                case EffectType.Attack:
                    break;
                case EffectType.Defense:
                    break;
                case EffectType.Dizzy:
                    break;
                case EffectType.Undeated:
                    break;
                case EffectType.Dodged:
                    break;
            }
            buffTarget.BuffList.Add(buff);
        }


        #endregion
    }
}
