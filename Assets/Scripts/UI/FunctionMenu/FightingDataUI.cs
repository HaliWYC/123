using UnityEngine;
using UnityEngine.UI;

namespace ShanHai_IsolatedCity.Inventory
{
    public class FightingDataDetailsUI : Singleton<FightingDataDetailsUI>
    {
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
        public Text WoundRecovery;
        public Text SkillCooling;

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

        private float[,] FightingData = new float[3, 30];
        private string[] Text = new string[30];


        protected override void Awake()
        {
            base.Awake();
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

        private void Start()
        {
            UpdateFightingDataUI(GameManager.Instance.playerInformation.fightingData, CharacterFightingDataType.Character);
        }

        public void UpdateFightingDataUI(CharacterFightingData_SO Data, CharacterFightingDataType type)
        {

            switch (type)
            {
                case CharacterFightingDataType.Character:
                    FillFightingDataArray(Data, 0);
                    break;
                case CharacterFightingDataType.Equip:
                    FillFightingDataArray(Data, 1);
                    break;
                case CharacterFightingDataType.Buff:
                    FillFightingDataArray(Data, 2);
                    break;
            }
            UpdateCharacterFightingDataUI();
        }

        private void FillFightingDataArray(CharacterFightingData_SO Data, int index)
        {
            FightingData[index, 0] = Data.Metal;
            FightingData[index, 1] = Data.Wood;
            FightingData[index, 2] = Data.Water;
            FightingData[index, 3] = Data.Fire;
            FightingData[index, 4] = Data.Ground;
            FightingData[index, 5] = Data.Lunar_Solar;
            FightingData[index, 6] = Data.maxHealth;
            FightingData[index, 7] = Data.maxVigor;
            FightingData[index, 8] = Data.maxWound;
            FightingData[index, 9] = Data.maxQi;
            FightingData[index, 10] = Data.maxMorale;
            FightingData[index, 11] = Data.Argility;
            FightingData[index, 12] = Data.Resilience;
            FightingData[index, 13] = Data.speed;
            FightingData[index, 14] = Data.woundRecovery;
            FightingData[index, 15] = Data.skillCooling;
            FightingData[index, 16] = Data.minimumRange;
            FightingData[index, 17] = Data.maximumRange;
            FightingData[index, 18] = Data.Attack;
            FightingData[index, 19] = Data.attackCooling;
            FightingData[index, 20] = Data.attackAccuracy;
            FightingData[index, 21] = Data.Penetrate;
            FightingData[index, 22] = Data.createWound;
            FightingData[index, 23] = Data.criticalPoint;
            FightingData[index, 24] = Data.criticalMultiple;
            FightingData[index, 25] = Data.fatal_Enhancement;
            FightingData[index, 26] = Data.Defense;
            FightingData[index, 27] = Data.penetrateDefense;
            FightingData[index, 28] = Data.criticalDefense;
            FightingData[index, 29] = Data.fatalDefense;
        }

        private void UpdateCharacterFightingDataUI()
        {
            GenerateText();
            Metal.text = "Metal: " + Text[0];
            Wood.text = "Wood: " + Text[1];
            Water.text = "Water: " + Text[2];
            Fire.text = "Fire: " + Text[3];
            Ground.text = "Ground: " + Text[4];
            Lunar_Solar.text = "Lunar/Solar: " + Text[5];
            Health.text = "Health: " + Text[6];
            Vigor.text = "Vigor: " + Text[7];
            Wound.text = "Wound: " + Text[8];
            Qi.text = "Qi: " + Text[9];
            Morale.text = "Morale: " + Text[10];
            Argility.text = "Argility: " + Text[11];
            Resilience.text = "Resilience: " + Text[12];
            Speed.text = "Speed: " + Text[13];
            WoundRecovery.text = "Wound Recovery" + Text[14];
            SkillCooling.text = "Skill Cooling" + Text[15];
            MinRange.text = "Minimum Range: " + Text[16];
            MaxRange.text = "Maximum Range: " + Text[17];
            Attack.text = "Attack: " + Text[18];
            AttackCooling.text = "Attack Cooling: " + Text[19];
            AttackAccuracy.text = "Attack Accuracy: " + Text[20];
            Penetrate.text = "Penetrate: " + Text[21];
            WoundCreate.text = "Wound Create: " + Text[22];
            CriticalPoint.text = "Critical Point: " + Text[23];
            CriticalMultiple.text = "Critical Multiple: " + Text[24];
            Fatal_Enhancement.text = "Fatal Enhancement: " + Text[25];
            Defense.text = "Defense: " + Text[26];
            PenetrateDefense.text = "Penetrate Defense: " + Text[27];
            CriticalDefense.text = "Critical Defense: " + Text[28];
            FatalDefense.text = "Fatal Defense: " + Text[29];
        }
        private void GenerateText()
        {
            for (int index = 0; index < FightingData.GetLength(1); index++)
            {
                Text[index] = Mathf.Max(FightingData[0, index] + FightingData[1, index] + FightingData[2, index], 0).ToString();
                if (FightingData[0, index] != 0)
                    Text[index] += "<color=#FF0000>( Basic: </color>" + FightingData[0, index] + "<color=#FF0000> )</color>";
                if (FightingData[1, index] != 0)
                    Text[index] += "<color=#00A0FF>( Equipment: </color>" + FightingData[1, index] + "<color=#00A0FF> )</color>";
                if (FightingData[2, index] != 0)
                    Text[index] += "<color=#06B917>( Buff: </color>" + FightingData[2, index] + "<color=#06B917> )</color>";
            }
        }


    }
}

