using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShanHai_IsolatedCity.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public SlotUI slotUI;
        
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemDetails!=null)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                switch (slotUI.itemDetails.itemType)
                {
                    case ItemType.装备:
                        inventoryUI.itemToolTip.SetUpToolTip(InventoryManager.Instance.GetEquipItemDetails(slotUI.itemDetails.itemID), slotUI.slotType);
                        break;
                    case ItemType.消耗品:
                        inventoryUI.itemToolTip.SetUpToolTip(InventoryManager.Instance.GetConsumeItemDetails(slotUI.itemDetails.itemID), slotUI.slotType);
                        break;
                    case ItemType.任务物品:
                        inventoryUI.itemToolTip.SetUpToolTip(InventoryManager.Instance.GetTaskItemDetails(slotUI.itemDetails.itemID), slotUI.slotType);
                        break;
                    case ItemType.其他物品:
                        inventoryUI.itemToolTip.SetUpToolTip(InventoryManager.Instance.GetOtherItemDetails(slotUI.itemDetails.itemID), slotUI.slotType);
                        break;
                }
                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(-0.1f, 0.5f);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 1;
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
    }
}
