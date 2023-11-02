using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                if (slotUI.itemDetails != null)
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                        slotUI.inventoryUI.upDateSlotHighLight(slotUI.slotIndex);
                    else
                        slotUI.inventoryUI.upDateSlotHighLight(-1);

                    EventHandler.callItemSelectedEvent(slotUI.itemDetails, slotUI.isSelected);
                }
            }
        }
    }
}

