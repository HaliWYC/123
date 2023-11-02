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
            //Debug.Log(slotUI.itemAmount);
            if (slotUI.itemDetails!=null)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                inventoryUI.itemToolTip.setUpToolTip(slotUI.itemDetails, slotUI.slotType);
                
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
