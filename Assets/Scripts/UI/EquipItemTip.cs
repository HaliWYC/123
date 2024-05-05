using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using ShanHai_IsolatedCity.Skill;
using UnityEditor.Experimental.GraphView;
using System;

namespace ShanHai_IsolatedCity.Inventory
{
    public class EquipItemTips : MonoBehaviour,IPointerExitHandler
    {
        [Header("Item")]
        [SerializeField] private Image qualityIcon;

        [SerializeField] private Image itemIcon;

        [SerializeField] private Text nameText;

        [SerializeField] private Text typeText;

        [SerializeField] private Text qualityText;

        [SerializeField] private Text descriptionText;

        public Color Yellow;
        public Color Green;
        public Color Blue;
        public Color Red;
        public Color Brown;
        public Color White;

        public Text Metal;
        public Text Wood;
        public Text Water;
        public Text Fire;
        public Text Ground;
        public Text Lunar_Solar;

        public Text Health;
        public Text Vigor;
        public Text Wound;
        public Text Qi;
        public Text Morale;
        public Text Argility;
        public Text Resilience;
        public Text Speed;

        public Text MinRange;
        public Text MaxRange;
        public Text Attack;
        public Text AttackCooling;
        public Text AttackAccuracy;
        public Text Penetrate;
        public Text WoundCreate;
        public Text CriticalPoint;
        public Text CriticalMultiple;
        public Text Fatal_Enhancement;

        public Text Defense;
        public Text PenetrateDefense;
        public Text CriticalDefense;
        public Text FatalDefense;

        [Header("Skill")]
        public GameObject skillList;
        public GameObject skillSlot;
        public GameObject skillDetailsTip;
        public Text skillInfor;
        public Text qiConsume;
        public Text coolDown;
        public Text targetType;
        public Text SkillRange;
        public Text buffEffect;

        [SerializeField] List<SkillSlotUI> skillSlots;

        public List<SkillDetails_SO> skillDetailList;


        private void Awake()
        {
            Metal.color = Yellow;
            Attack.color = Yellow;
            Penetrate.color = Yellow;
            AttackAccuracy.color = Yellow;
            Wood.color = Green;
            Vigor.color = Green;
            Argility.color = Green;
            Speed.color = Green;
            Water.color = Blue;
            FatalDefense.color = Blue;
            CriticalDefense.color = Blue;
            Qi.color = Blue;
            Fire.color = Red;
            CriticalPoint.color = Red;
            CriticalMultiple.color = Red;
            Fatal_Enhancement.color = Red;
            Ground.color = Brown;
            Health.color = Brown;
            Defense.color = Brown;
            PenetrateDefense.color = Brown;
            Lunar_Solar.color = White;
            Resilience.color = White;
            Wound.color = White;
            WoundCreate.color = White;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.GetChild(0).GetComponent<Toggle>().isOn = true;
            gameObject.SetActive(false);
            ReGenerateSkillSlot();
            skillDetailsTip.SetActive(false);
        }
        #region SetUpToolTips
        public void SetUpItemToolTip(EquipItemDetails equipItem)
        {
            qualityIcon.color = InventoryManager.Instance.GetEquipQualityColor(equipItem.EquipItemQuality);
            itemIcon.sprite = equipItem.itemIcon;
            nameText.text = equipItem.itemName;
            typeText.text = equipItem.itemType.ToString() + "-" + equipItem.equipItemType.ToString();
            skillDetailList = equipItem.equipSkills;
            descriptionText.text = equipItem.itemDescription;

            Metal.text = "Metal: " + equipItem.EquipData.Metal.ToString();
            Wood.text = "Wood: " + equipItem.EquipData.Wood.ToString();
            Water.text = "Water: " + equipItem.EquipData.Water.ToString();
            Fire.text = "Fire: " + equipItem.EquipData.Fire.ToString();
            Ground.text = "Ground: " + equipItem.EquipData.Ground.ToString();
            Lunar_Solar.text = "Lunar/Solar: " + equipItem.EquipData.Lunar_Solar.ToString();
            Health.text = "Health: " + equipItem.EquipData.maxHealth.ToString();
            Vigor.text = "Vigor: " + equipItem.EquipData.maxVigor.ToString();
            Wound.text = "Wound: " + equipItem.EquipData.maxWound.ToString();
            Qi.text = "Qi: " + equipItem.EquipData.maxQi.ToString();
            Morale.text = "Morale: " + equipItem.EquipData.maxMorale.ToString();
            Argility.text = "Argility: " + equipItem.EquipData.Argility.ToString();
            Resilience.text = "Resilience: " + equipItem.EquipData.Resilience.ToString();
            Speed.text = "Speed: " + equipItem.EquipData.speed.ToString();
            MinRange.text = "Minimum Range: " + equipItem.EquipData.minimumRange.ToString();
            MaxRange.text = "Maximum Range: " + equipItem.EquipData.maximumRange.ToString();
            Attack.text = "Attack: " + equipItem.EquipData.Attack.ToString();
            AttackCooling.text = "Attack Cooling: " + equipItem.EquipData.attackCooling.ToString();
            AttackAccuracy.text = "Attack Accuracy: " + equipItem.EquipData.attackAccuracy.ToString();
            Penetrate.text = "Penetrate: " + equipItem.EquipData.Penetrate.ToString();
            WoundCreate.text = "Wound Create: " + equipItem.EquipData.createWound.ToString();
            CriticalPoint.text = "Critical Point: " + equipItem.EquipData.criticalPoint.ToString();
            CriticalMultiple.text = "Critical Multiple: " + equipItem.EquipData.criticalMultiple.ToString();
            Fatal_Enhancement.text = "Fatal Enhancement: " + equipItem.EquipData.fatal_Enhancement.ToString();
            Defense.text = "Defense: " + equipItem.EquipData.Defense.ToString();
            PenetrateDefense.text = "Penetrate Defense: " + equipItem.EquipData.penetrateDefense.ToString();
            CriticalDefense.text = "Critical Defense: " + equipItem.EquipData.criticalDefense.ToString();
            Fatal_Enhancement.text = "Fatal Enhancement: " + equipItem.EquipData.fatal_Enhancement.ToString();
        }

        public void SetUpSkillToolTip(SkillDetails_SO skill)
        {
            skillInfor.text = skill.skillInformation;
            qiConsume.text = "Qi Consume: " + skill.QiComsume;
            coolDown.text = "Cool Down: " + skill.CoolDown;
            targetType.text = "Target: " + skill.skillTargetType;
            SkillRange.text = "Skill Range: " + skill.skillRange;
            buffEffect.text = GetBuffEffect(skill);

            for (int index = 0; index < skillSlots.Count; index++)
            {
                if (skillSlots[index].skillDetails !=skill)
                    skillList.transform.GetChild(index).gameObject.SetActive(false);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(skillList.GetComponent<RectTransform>());
            skillDetailsTip.SetActive(true);
        }

        private string GetBuffEffect(SkillDetails_SO skill)
        {
            string effects = string.Empty;
            foreach(Buff_SO buff in skill.buffList)
            {
                effects += buff.buffDescription+"\n";
            }
            return effects;
        }
        #endregion

        public void SetSkillDetailsTipFalse()
        {
            
            skillDetailsTip.SetActive(false);
        }


        public void GenerateSkillSlots()
        {
            foreach(var slot in skillSlots)
            {
                Destroy(slot.gameObject);
            }
            skillSlots.Clear();

            if (skillDetailList != null)
            {
                foreach(SkillDetails_SO skill in skillDetailList)
                {
                    var Slot = Instantiate(skillSlot, skillList.transform).GetComponent<SkillSlotUI>();
                    skillSlots.Add(Slot);
                    Slot.UpdateSlot(skill);
                }
            }
        }

        public void ReGenerateSkillSlot()
        {
            
            for(int index =0; index < skillSlots.Count; index++)
            {
                skillList.transform.GetChild(index).gameObject.SetActive(true);
                
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(skillList.GetComponent<RectTransform>());
        }

       
    }
}

