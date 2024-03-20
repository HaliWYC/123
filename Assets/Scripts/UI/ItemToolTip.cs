using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI propertyText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Text valueText;

    [SerializeField] private GameObject buttomPart;

    #region ShowToolTips
    public void SetUpToolTip(EquipItemDetails equipItem,SlotType slotType)
    {
        nameText.text = equipItem.itemName;
        typeText.text = equipItem.itemType.ToString() + "-" + equipItem.equipItemType.ToString();

        propertyText.text = GetEquipItemPropertyUI(equipItem);

        descriptionText.text = "\n" + equipItem.itemDescription;

        buttomPart.SetActive(true);

        var price = equipItem.itemPrice;


        if (slotType == SlotType.人物背包)
        {
            price = (int)(price * equipItem.sellPercentage);

        }

        valueText.text = price.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    public void SetUpToolTip(ConsumeItemDetails consumeItem, SlotType slotType)
    {
        nameText.text = consumeItem.itemName;
        typeText.text = consumeItem.itemType.ToString() + " " + consumeItem.consumeItemType.ToString();

        propertyText.text = GetConsumeItemPropertyUI(consumeItem);
        descriptionText.text = consumeItem.itemDescription;

        buttomPart.SetActive(true);

        var price = consumeItem.itemPrice;


        if (slotType == SlotType.人物背包)
        {
            price = (int)(price * consumeItem.sellPercentage);

        }

        valueText.text = price.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    public void SetUpToolTip(TaskItemDetails taskItem, SlotType slotType)
    {
        nameText.text = taskItem.itemName;
        typeText.text = taskItem.itemType.ToString() + " " + taskItem.taskItemType.ToString();

        descriptionText.text = taskItem.itemDescription;

        buttomPart.SetActive(false);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    public void SetUpToolTip(OtherItemDetails otherItem, SlotType slotType)
    {
        nameText.text = otherItem.itemName;
        typeText.text = otherItem.itemType.ToString() + " " + otherItem.otherItemType.ToString();

        descriptionText.text = otherItem.itemDescription;

        buttomPart.SetActive(true);

        var price = otherItem.itemPrice;


        if (slotType == SlotType.人物背包)
        {
            price = (int)(price * otherItem.sellPercentage);

        }

        valueText.text = price.ToString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    #endregion
    public string GetEquipItemPropertyUI(EquipItemDetails equip)
    {
        string[] property = ArrangeEquipValue(equip);
        bool basicEmpty = true;
        bool AttackEmpty = true;
        bool DefenseEmpty = true;
        bool SpecialEmpty = true;

        string propertyString = string.Empty;
        for(int index = 0; index < 14; index++)
        {
            if (property[index] != string.Empty)
            {
                basicEmpty = false;
                break;
            }
        }
        for (int index = 14; index < 25; index++)
        {
            if (property[index] != string.Empty)
            {
                AttackEmpty = false;
                break;
            }
        }

        for (int index = 25; index < 29; index++)
        {
            if (property[index] != string.Empty)
            {
                DefenseEmpty = false;
                break;
            }
        }
        if (property[29] != string.Empty)
            SpecialEmpty = false;

        for(int index = 0; index < property.Length; index++)
        {
            if (index == 0 && !basicEmpty)
                propertyString += "<size=7><color=#FFFFFF>Basic</color> Information</size>" + "\n";
            if (index == 14 && !AttackEmpty)
                propertyString += "<size=7><color=#FF0000>Attack</color> Information</size>" + "\n";
            if (index == 24 && !DefenseEmpty)
                propertyString += "<size=7><color=#0099FF>Defense</color> Information</size>" + "\n";
            if (index == 28 && !SpecialEmpty)
                propertyString += "<size=7><color=#FFAF00>Special</color> Information</size>" + "\n";
            propertyString += property[index];
        }

        return propertyString;
    }

    public string[] ArrangeEquipValue(EquipItemDetails equip)
    {
        CharacterFightingData_SO equipData = equip.EquipData;
        string[] property = new string[30];
        //BasicInfor
        if (equipData.Metal != 0)
            property[0] = "<color=#FFAF00>Metal</color>: " + equipData.Metal + "\n";
        if (equipData.Wood != 0)
            property[1] = "<color=#0D8C0D>Wood</color>: " + equipData.Wood + "\n";
        if (equipData.Water != 0)
            property[2] = "<color=#0099FF>Water</color>: " + equipData.Water + "\n";
        if (equipData.Fire != 0)
            property[3] = "<color=#FF0000>Fire</color>: " + equipData.Fire + "\n";
        if (equipData.Ground != 0)
            property[4] = "<color=#823E00>Ground</color>: " + equipData.Ground + "\n";
        if (equipData.Lunar_Solar != 0)
            property[5] = "<color=#000000>Lunar</color> <color=#FFFFFF>Solar</color>: " + equipData.Lunar_Solar + "\n";
        if (equipData.speed != 0)
            property[6] = "Speed: " + equipData.speed + "\n";
        if (equipData.Resilience != 0)
            property[7] = "<color=#B7B7B7>Resilience</color>: " + equipData.Resilience + "\n";
        if (equipData.Argility != 0)
            property[8] = "Argility: " + equipData.Argility + "\n";
        if (equipData.maxHealth != 0)
            property[9] = "<color=#FF0000>MaxHealth</color>: " + equipData.maxHealth + "\n";
        if (equipData.maxQi != 0)
            property[10] = "<color=#4873A3>MaxQi</color>: " + equipData.maxQi + "\n";
        if (equipData.maxVigor != 0)
            property[11] = "<color=#0D8C0D>MaxVigor</color>: " + equipData.maxVigor + "\n";
        if (equipData.maxWound != 0)
            property[12] = "<color=#390082>MaxWound</color>: " + equipData.maxWound + "\n";
        if (equipData.maxMorale != 0)
            property[13] = "<color=#FFAF00>Morale</color>: " + equipData.maxMorale + "\n" + "\n";
        //AttackInfor
        if (equipData.meleeRange != 0)
            property[14] = "MeleeRange: " + equipData.meleeRange + "\n";
        if (equipData.rangedRange != 0)
            property[15] = "RangedRange: " + equipData.rangedRange + "\n";
        if (equipData.Attack != 0)
            property[16] = "<color=#FF0000>Attack</color>: " + equipData.Attack + "\n";
        if (equipData.attackAccuracy != 0)
            property[17] = "Attack Accuracy: " + equipData.attackAccuracy + "\n";
        if (equipData.createWound != 0)
            property[18] = "Wound Create: " + equipData.createWound + "\n";
        if (equipData.criticalRate != 0)
            property[19] = "Critical Rate: " + equipData.criticalRate + "\n";
        if (equipData.criticalMutiple != 0)
            property[20] = "Critical Mutiple: " + equipData.criticalMutiple + "\n";
        if (equipData.Penetrate != 0)
            property[21] = "Penetrate: " + equipData.Penetrate + "\n";
        if (equipData.fatal_Enhancement != 0)
            property[22] = "Fatal Enhancement: " + equipData.fatal_Enhancement + "\n";
        if (equipData.continuous_DamageRate != 0)
            property[23] = "Continuous Damage Rate: " + equipData.continuous_DamageRate + "\n" + "\n";
        //DefenseInfor
        if (equipData.Defense != 0)
            property[24] = "<color=#0099FF>Defense</color>: " + equipData.Defense + "\n";
        if (equipData.criticalDefense != 0)
            property[25] = "Critical Defense: " + equipData.criticalDefense + "\n";
        if (equipData.penetrateDefense != 0)
            property[26] = "Penetrate Defense: " + equipData.penetrateDefense + "\n";
        if (equipData.fatalDefense != 0)
            property[27] = "Fatal Defense: " + equipData.fatalDefense + "\n" + "\n";
        //SpecialValue
        if (equipData.woundRecovery != 0)
            property[28] = "Wound Recovery: " + equipData.woundRecovery + "\n";

        return property;
    }

    public string GetConsumeItemPropertyUI(ConsumeItemDetails consume)
    {
        string propertyString = string.Empty;
        foreach(Buff_SO buff in consume.BuffList)
        {
            if (buff.buffDescription != string.Empty)
                propertyString += buff.buffDescription + "\n";
        }
        return propertyString;
    }
}
