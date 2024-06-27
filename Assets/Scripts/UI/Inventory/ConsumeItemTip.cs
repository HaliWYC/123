using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShanHai_IsolatedCity.Inventory;
using System;

public class ConsumeItemTip : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image quality;
    [SerializeField] private Text itemName;
    [SerializeField] private Text typeText;
    [SerializeField] private Text Description;
    

    public void SetupToolTip(ConsumeItemDetails consume)
    {
        itemIcon.sprite = consume.itemIcon;
        quality.color = InventoryManager.Instance.GetBasicQualityColor(consume.ConsumeItemQuality);
        typeText.text = consume.itemType.ToString() + "-" + consume.consumeItemType.ToString();
        Description.text = consume.itemDescription;
    }

    
}
