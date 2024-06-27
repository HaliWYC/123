using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ShanHai_IsolatedCity.Inventory;
using static UnityEditor.FilePathAttribute;

public class TradeItemSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public Image qualityIcon;
    public Image itemIcon;
    public ItemDetails itemDetails;
    public Text amountText;
    public SlotType slotType;
    public int itemAmount;
    public bool inTrade = false;

    public void UpdateSlot(ItemDetails item, ItemType itemType, int amount, SlotType location)
    {
        itemDetails = item;
        itemAmount = amount;
        if (itemDetails != null)
        {
            itemIcon.sprite = item.itemIcon;
            amountText.text = itemAmount.ToString();
            qualityIcon.color = itemType switch
            {
                ItemType.Equip => InventoryManager.Instance.GetEquipQualityColor(InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID).EquipItemQuality),
                ItemType.Consume => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetConsumeItemDetails(itemDetails.itemID).ConsumeItemQuality),
                ItemType.Task => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetTaskItemDetails(itemDetails.itemID).TaskItemQuality),
                ItemType.Other => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetOtherItemDetails(itemDetails.itemID).OtherItemQuality),
                _ => Color.white
            };
        }
        slotType = location;
    }
    public void UpdateSlot(ItemDetails item, ItemType itemType, SlotType location)
    {
        itemDetails = item;
        if (itemDetails != null)
        {
            itemIcon.sprite = item.itemIcon;
            qualityIcon.color = itemType switch
            {
                ItemType.Equip => InventoryManager.Instance.GetEquipQualityColor(InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID).EquipItemQuality),
                ItemType.Consume => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetConsumeItemDetails(itemDetails.itemID).ConsumeItemQuality),
                ItemType.Task => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetTaskItemDetails(itemDetails.itemID).TaskItemQuality),
                ItemType.Other => InventoryManager.Instance.GetBasicQualityColor(InventoryManager.Instance.GetOtherItemDetails(itemDetails.itemID).OtherItemQuality),
                _ => Color.white
            };
        }
        slotType = location;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            TradeUI.Instance.isSelected = true;
            TradeUI.Instance.itemTip.SetUpItemTip(itemDetails);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TradeUI.Instance.isSelected = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            if (eventData.clickCount % 2 == 0)
            {
                TradeUI.Instance.UpdateSubmitionPanel(this);
            }
        }
        
    }
}
