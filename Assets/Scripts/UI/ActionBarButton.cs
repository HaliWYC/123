using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode key;
        private SlotUI slotUI;
        private bool canUse;

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        private void OnEnable()
        {
            EventHandler.updateGameStateEvent += onUpdateGameStateEvent;
        }

        private void OnDisable()
        {
            EventHandler.updateGameStateEvent -= onUpdateGameStateEvent;
        }

        private void onUpdateGameStateEvent(GameState gameState)
        {
            canUse = gameState == GameState.GamePlay;
        }

        private void Update()
        {
            if (Input.GetKeyDown(key) && canUse)
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

