using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;
using TMPro;

public class ItemTip : MonoBehaviour
{
    [SerializeField] private Image qualityIcon;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI quality;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI ShanHaiGold;
    [SerializeField] private TextMeshProUGUI Gold;
    [SerializeField] private Text Data;
    private float[] equipData = new float[30];
    private string[] equipText = new string[30];
    [SerializeField] private Text Description;

    public void SetUpItemTip(ItemDetails itemDetails)
    {
        if (itemDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Equip:
                    EquipItemDetails equip = InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID);
                    qualityIcon.color = InventoryManager.Instance.GetEquipQualityColor(equip.EquipItemQuality);
                    quality.text = equip.EquipItemQuality.ToString();
                    itemType.text = itemDetails.itemType.ToString() + " - " + equip.equipItemType;
                    Data.text = GetItemData(equip.EquipData);
                    Data.gameObject.SetActive(true);
                    break;
                case ItemType.Consume:
                    ConsumeItemDetails consume = InventoryManager.Instance.GetConsumeItemDetails(itemDetails.itemID);
                    qualityIcon.color = InventoryManager.Instance.GetBasicQualityColor(consume.ConsumeItemQuality);
                    quality.text = consume.ConsumeItemQuality.ToString();
                    itemType.text = itemDetails.itemType.ToString() + " - " + consume.consumeItemType;
                    Data.gameObject.SetActive(false);
                    break;
                case ItemType.Task:
                    TaskItemDetails task = InventoryManager.Instance.GetTaskItemDetails(itemDetails.itemID);
                    qualityIcon.color = InventoryManager.Instance.GetBasicQualityColor(task.TaskItemQuality);
                    quality.text = task.TaskItemQuality.ToString();
                    itemType.text = itemDetails.itemType.ToString() + " - " + task.taskItemType;
                    break;
                case ItemType.Other:
                    OtherItemDetails other = InventoryManager.Instance.GetOtherItemDetails(itemDetails.itemID);
                    qualityIcon.color = InventoryManager.Instance.GetBasicQualityColor(other.OtherItemQuality);
                    quality.text = other.OtherItemQuality.ToString();
                    itemType.text = itemDetails.itemType.ToString() + " - " + other.otherItemType;
                    break;
            }
        }
        itemIcon.sprite = itemDetails.itemIcon;
        itemName.text = itemDetails.itemName;
        ShanHaiGold.text = itemDetails.shanHaiGold.ToString();
        Gold.text = itemDetails.gold.ToString();
        Description.text = itemDetails.itemDescription;
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemData(CharacterFightingData_SO equipData)
    {
        string property = string.Empty;
        if (equipData.Metal != 0)
            property += "Metal: " + equipData.Metal + "\n";
        if (equipData.Wood != 0)
            property += "Wood: " + equipData.Wood + "\n";
        if (equipData.Water != 0)
            property += "Water: " + equipData.Water + "\n";
        if (equipData.Fire != 0)
            property += "Fire: " + equipData.Fire + "\n";
        if (equipData.Ground != 0)
            property += "Ground: " + equipData.Ground + "\n";
        if (equipData.Lunar_Solar != 0)
            property += "Lunar/Solar: " + equipData.Lunar_Solar + "\n";
        if (equipData.maxHealth != 0)
            property += "Max Health: " + equipData.maxHealth + "\n";
        if (equipData.maxVigor != 0)
            property += "Max Vigor: " + equipData.maxVigor + "\n";
        if (equipData.maxWound != 0)
            property += "Max Wound: " + equipData.maxWound + "\n";
        if (equipData.maxQi != 0)
            property += "Max Qi: " + equipData.maxQi + "\n";
        if (equipData.maxMorale != 0)
            property += "Max Morale: " + equipData.maxMorale + "\n";
        if (equipData.Argility != 0)
            property += "Argility: " + equipData.Argility + "\n";
        if (equipData.Resilience != 0)
            property += "Resilience: " + equipData.Resilience + "\n";
        if (equipData.speed != 0)
            property += "Speed: " + equipData.speed + "\n";
        if (equipData.woundRecovery != 0)
            property += "Wound Recovery: " + equipData.woundRecovery + "\n";
        if (equipData.skillCooling != 0)
            property += "Skill Cooling: " + equipData.skillCooling + "\n";
        if (equipData.minimumRange != 0)
            property += "Minimum Range: " + equipData.minimumRange + "\n";
        if (equipData.maximumRange != 0)
            property += "Maximum Range: " + equipData.maximumRange + "\n";
        if (equipData.Attack != 0)
            property += "Attack: " + equipData.Attack + "\n";
        if (equipData.attackCooling != 0)
            property += "Attack Cooling: " + equipData.attackCooling + "\n";
        if (equipData.attackAccuracy != 0)
            property += "Attack Accuracy: " + equipData.attackAccuracy + "\n";
        if (equipData.Penetrate != 0)
            property += "Penetrate: " + equipData.Penetrate + "\n";
        if (equipData.createWound != 0)
            property += "Wound Create: " + equipData.createWound + "\n";
        if (equipData.criticalPoint != 0)
            property += "Critical Point: " + equipData.criticalPoint + "\n";
        if (equipData.criticalMultiple != 0)
            property += "Critical Multiple: " + equipData.criticalMultiple + "\n";
        if (equipData.fatal_Enhancement != 0)
            property += "Fatal Enhancement: " + equipData.fatal_Enhancement + "\n";
        if (equipData.Defense != 0)
            property += "Defense: " + equipData.Defense + "\n";
        if (equipData.penetrateDefense != 0)
            property += "Penetrate Defense: " + equipData.penetrateDefense + "\n";
        if (equipData.criticalDefense != 0)
            property += "Critical Defense: " + equipData.criticalDefense + "\n";
        if (equipData.fatalDefense != 0)
            property += "FatalDefense: " + equipData.fatalDefense + "\n";
        return property;
    }
}
