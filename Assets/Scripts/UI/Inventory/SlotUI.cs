using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ShanHai_IsolatedCity.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("GetComponent")]
        
        [SerializeField] private Image Quality;
        [SerializeField] private Image itemIcon;
        [SerializeField] private Text amountText;
        [SerializeField] private Text Name;
        [SerializeField] private Text QualityText;
        [SerializeField] private Text itemTypeText;
        [SerializeField] private Text subType;
        [SerializeField] private GameObject Action;
        [Header("SlotType")]
        public SlotType slotType;
        public int slotIndex;


        public ItemDetails itemDetails;
        public int itemAmount;
        /// <summary>
        /// Update Slot UI and information
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">HoldingNumber </param>
        public void UpDateSlot(ItemDetails item, ItemType itemType, int amount)
        {
            itemDetails = item;
            itemAmount = amount;
            if (itemDetails != null)
            {
                if(Name!=null)
                    Name.text = itemDetails.itemName;
                itemIcon.sprite = item.itemIcon;
                switch (itemType)
                {
                    case ItemType.Equip:
                        EquipItemDetails equip = InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID);
                        if (Quality != null)
                            Quality.color = InventoryManager.Instance.GetEquipQualityColor(equip.EquipItemQuality);
                        if (QualityText != null)
                            QualityText.text = equip.EquipItemQuality.ToString();
                        if (itemTypeText != null)
                            itemTypeText.text = equip.itemType.ToString();
                        if (subType != null)
                            subType.text = equip.equipItemType.ToString();
                        break;
                    case ItemType.Consume:
                        ConsumeItemDetails consume = InventoryManager.Instance.GetConsumeItemDetails(itemDetails.itemID);
                        Quality.color = InventoryManager.Instance.GetQualityColor(consume.ConsumeItemQuality);
                        amountText.text = "X " + itemAmount.ToString();
                        QualityText.text = consume.ConsumeItemQuality.ToString();
                        if (itemTypeText != null)
                            itemTypeText.text = consume.itemType.ToString();
                        subType.text = consume.consumeItemType.ToString();
                        break;
                    case ItemType.Task:
                        TaskItemDetails task = InventoryManager.Instance.GetTaskItemDetails(itemDetails.itemID);
                        Quality.color = InventoryManager.Instance.GetQualityColor(task.TaskItemQuality);
                        amountText.text = itemAmount.ToString();
                        QualityText.text = task.TaskItemQuality.ToString();
                        if (itemTypeText != null)
                            itemTypeText.text = task.itemType.ToString();
                        subType.text = task.taskItemType.ToString();
                        break;
                    case ItemType.Other:
                        OtherItemDetails other = InventoryManager.Instance.GetOtherItemDetails(itemDetails.itemID);
                        Quality.color = InventoryManager.Instance.GetQualityColor(other.OtherItemQuality);
                        amountText.text = itemAmount.ToString();
                        QualityText.text = other.OtherItemQuality.ToString();
                        if (itemTypeText != null)
                            itemTypeText.text = other.itemType.ToString();
                        subType.text = other.otherItemType.ToString();
                        break;
                }
            }
            //button.interactable = true;
        }

        /// <summary>
        /// Make the Slot to become empty
        /// </summary>
        public void UpDateEmptySlot()
        {
            itemDetails = null;
            itemAmount = 0;
            Quality.color = Color.white;
            itemIcon.sprite = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(itemDetails!=null)
            {
                switch (itemDetails.itemType)
                {
                    case ItemType.Equip:
                        if (slotType == SlotType.PlayerBag)
                        {
                            EquipItemTips equipItemTips = InventoryManager.Instance.inventoryUI.equipItemTip;
                            equipItemTips.gameObject.SetActive(!equipItemTips.gameObject.activeInHierarchy);
                            equipItemTips.SetUpItemToolTip(InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID));
                        }

                        if (eventData.clickCount % 2 == 0 && slotType == SlotType.Player)
                        {
                            InventoryManager.Instance.TakeOffItem(InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID));
                        }
                        else if (eventData.clickCount % 2 == 0 && slotType == SlotType.PlayerBag)
                        {
                            InventoryManager.Instance.EquipItem(InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID));
                        }
                        
                        break;
                    case ItemType.Consume:
                        ConsumeItemTip consumeItemTip = InventoryManager.Instance.inventoryUI.consumeItemTip;
                        consumeItemTip.gameObject.SetActive(!consumeItemTip.gameObject.activeInHierarchy);
                        consumeItemTip.SetupToolTip(InventoryManager.Instance.GetConsumeItemDetails(itemDetails.itemID));
                        if (eventData.clickCount % 2 == 0)
                        {
                            InventoryManager.Instance.inventoryUI.ConsumeItemPanel.SetActive(true);
                            InventoryManager.Instance.inventoryUI.ConsumeItemPanel.GetComponent<UseConsumeItemPanel>().SetupConsumePanel(InventoryManager.Instance.GetConsumeItemDetails(itemDetails.itemID));
                        }
                        break;
                    case ItemType.Task:
                        break;
                    case ItemType.Other:
                        break;
                }
            }
            
            //TODO:播放声音
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            //TODO:播放声音
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(Action!=null)
                Action.SetActive(false);
        }

        public void EquipItem()
        {
            EquipItemDetails equip = InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID);
            InventoryManager.Instance.EquipItem(equip);
        }

        public void ShowItemTip()
        {
            EquipItemDetails equip = InventoryManager.Instance.GetEquipItemDetails(itemDetails.itemID);
            InventoryManager.Instance.inventoryUI.equipItemTip.SetUpItemToolTip(equip);
            InventoryManager.Instance.inventoryUI.equipItemTip.gameObject.SetActive(true);
        }

        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    if (itemAmount != 0)
        //    {
        //        //Debug.Log(itemAmount);
        //        inventoryUI.dragItem.enabled = true;
        //        inventoryUI.dragItem.sprite = slotImage.sprite;
        //        inventoryUI.dragItem.SetNativeSize();
        //        isSelected = true;
        //        inventoryUI.UpDateSlotHighLight(slotIndex);
        //    }
        //}

        //public void OnDrag(PointerEventData eventData)
        //{
        //    inventoryUI.dragItem.transform.position = Input.mousePosition;
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    inventoryUI.dragItem.enabled = false;

        //    if(eventData.pointerCurrentRaycast.gameObject != null)
        //    {
        //        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
        //        {
        //            return;
        //        }

        //        var targetSlotUI = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();

        //        int targetIndex = targetSlotUI.slotIndex;

        //        //Swap item in player's bag
        //        if(slotType==SlotType.人物背包 && targetSlotUI.slotType == SlotType.人物背包)
        //        {
        //            InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
        //        }
        //        else if(slotType == SlotType.NPC背包 && targetSlotUI.slotType == SlotType.人物背包)//Buy
        //        {
        //            EventHandler.CallShowTradeUI(itemDetails, false, itemDetails.itemType);
        //        }
        //        else if(slotType == SlotType.人物背包 && targetSlotUI.slotType == SlotType.NPC背包)//Sell
        //        {
        //            EventHandler.CallShowTradeUI(itemDetails, true, itemDetails.itemType);
        //        }

        //        //Clean all the highlight
        //        inventoryUI.UpDateSlotHighLight(-1);
        //    }
        //    /*else//Test for drop in the world
        //    {
        //        if (itemDetails.canDrop)
        //        {
        //            var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        //            EventHandler.callInstantiateItemInScene(itemDetails.itemID, pos);
        //        }
        //    }*/
        //}
    }
}
 