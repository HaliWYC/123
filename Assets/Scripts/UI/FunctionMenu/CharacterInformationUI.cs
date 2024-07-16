using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Buff;
namespace ShanHai_IsolatedCity.Inventory
{
    public class CharacterInformationUI : MonoBehaviour
    {
        public CharacterInformation_SO characterInformation;
        public CharacterFightingData_SO characterFightingData;
        public CharacterFightingData_SO characterEquipData;
        public CharacterFightingData_SO characterBuffData;

        [Header("Character")]
        public Text Appellation;
        public Text Name;
        public Image charIcon;
        public Text characterData;

        [Header("6Dimensions")]
        public Text Fitness;
        public Text Eloquence;
        public Text Wisdom;
        public Text Luck;
        public Text Strength;
        public Text Perception;

        [Header("Property")]
        public Text Metal;
        public Text Wood;
        public Text Water;
        public Text Fire;
        public Text Ground;
        public Text Lunar_Solar;

        [Header("Buff")]
        public GameObject buffBase;
        public GameObject bufflist;
        public List<BuffSlotUI> buffSlots;



        private void OnEnable()
        {
            EventHandler.UpdateCharacterInformationUIEvent += OnUpdateCharacterInformationEvent;
            EventHandler.UpdateBuffListEvent += OnUpdateBuffListEvent;
        }



        private void OnDisable()
        {
            EventHandler.UpdateCharacterInformationUIEvent -= OnUpdateCharacterInformationEvent;
            EventHandler.UpdateBuffListEvent -= OnUpdateBuffListEvent;
        }

        private void Start()
        {
            characterInformation = GameManager.Instance.playerInformation.characterInformation;
            characterFightingData = GameManager.Instance.playerInformation.fightingData;
            characterEquipData = GameManager.Instance.playerInformation.equipFightingData;
            characterBuffData = GameManager.Instance.playerInformation.buffFightingData;
        }

        private void OnUpdateCharacterInformationEvent()
        {
            Appellation.text = characterInformation.Appellation;
            Name.text = characterInformation.Name;
            charIcon.sprite = characterInformation.charIcon;
            characterData.text = GetCharacterData();
            Fitness.text = characterInformation.Fitness.ToString();
            Eloquence.text = characterInformation.Eloquence.ToString();
            Wisdom.text = characterInformation.Wisdom.ToString();
            Luck.text = characterInformation.Luck.ToString();
            Strength.text = characterInformation.Strength.ToString();
            Perception.text = characterInformation.Perception.ToString();
            Metal.text = Mathf.Max(characterFightingData.Metal + characterEquipData.Metal + characterBuffData.Metal, 0).ToString();
            Wood.text = Mathf.Max(characterFightingData.Wood + characterEquipData.Wood + characterBuffData.Wood, 0).ToString();
            Water.text = Mathf.Max(characterFightingData.Water + characterEquipData.Water + characterBuffData.Water, 0).ToString();
            Fire.text = Mathf.Max(characterFightingData.Fire + characterEquipData.Fire + characterBuffData.Fire, 0).ToString();
            Ground.text = Mathf.Max(characterFightingData.Ground + characterEquipData.Ground + characterBuffData.Ground, 0).ToString();
            Lunar_Solar.text = Mathf.Max(characterFightingData.Lunar_Solar + characterEquipData.Lunar_Solar + characterBuffData.Lunar_Solar, 0).ToString();
            OnUpdateBuffListEvent();
        }

        private void OnUpdateBuffListEvent()
        {
            foreach (var buff in buffSlots)
            {
                Destroy(buff.gameObject);
            }
            buffSlots.Clear();

            for (int index = 0; index < characterFightingData.buffList.Count; index++)
            {
                var slot = Instantiate(buffBase, bufflist.transform).GetComponent<BuffSlotUI>();
                buffSlots.Add(slot);
                slot.UpdateBuffSlot(characterFightingData.buffList[index]);
            }
            for (int index = 0; index < characterEquipData.buffList.Count; index++)
            {
                var slot = Instantiate(buffBase, bufflist.transform).GetComponent<BuffSlotUI>();
                buffSlots.Add(slot);
                slot.UpdateBuffSlot(characterEquipData.buffList[index]);
            }

            for (int index = 0; index < characterBuffData.buffList.Count; index++)
            {
                var slot = Instantiate(buffBase, bufflist.transform).GetComponent<BuffSlotUI>();
                buffSlots.Add(slot);
                slot.UpdateBuffSlot(characterBuffData.buffList[index]);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(bufflist.GetComponent<RectTransform>());
        }

        private string GetCharacterData()
        {
            string CD = string.Empty;
            //TODO:后期用Tooltip解释每一个词条
            if (characterInformation.Personality.Count > 0)
            {
                foreach(var personality in characterInformation.Personality)
                {
                    CD += "Personality: " + personality.Name + "\n";
                }
            }
            if (characterInformation.commandStyle != string.Empty)
                CD += "CommandStyle: " + characterInformation.commandStyle + "\n";
            if (characterInformation.Spirit != string.Empty)
                CD += "Spirit: " + characterInformation.Spirit + "\n";
            CD += "PrestigeLevel: " + characterInformation.prestigeLevel.ToString() + "\n";
            CD += "Prestige: " + characterInformation.currentPrestige + "/ " + characterInformation.nextPrestige;
            return CD;
        }
    }
}

