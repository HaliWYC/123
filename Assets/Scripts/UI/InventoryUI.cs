using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShanHai_IsolatedCity.Inventory
{


    public class InventoryUI : MonoBehaviour
    {
        public ItemToolTip itemToolTip;

        [Header("Drag Icon")]
        public Image dragItem;

        [Header("Player Bag UI")]

        [SerializeField] private GameObject bagUI;

        private bool bagOpened;

        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            EventHandler.updateInventoryUI += onUpdateInventoryUI;
            EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
        }

        private void onUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.角色:
                    for(int i =0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            
                            var Item = InventoryManager.Instance.getItemDetails(list[i].itemID);
                            playerSlots[i].upDateSlot(Item, list[i].itemAmount);
                        }
                        else
                        {
                            //Debug.Log(list[i].itemAmount);
                            playerSlots[i].upDateEmptySlot();
                        }
                    }

                    break;
            }
        }

        private void OnDisable()
        {
            EventHandler.updateInventoryUI -= onUpdateInventoryUI;
            EventHandler.beforeSceneUnloadEvent += onBeforeSceneUnloadEvent;
        }

        private void onBeforeSceneUnloadEvent()
        {
            upDateSlotHighLight(-1);
        }

        private void Start()
        {
            //Give each slot a index 
            for(int i=0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i; 
            }
            bagOpened = bagUI.activeInHierarchy; 
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                openBagUI();
            }
        }

        /// <summary>
        /// Open/Close BagUI 
        /// </summary>

        public void openBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// Update Slot HighLight
        /// </summary>
        /// <param name="index">Index</param>

        public void upDateSlotHighLight(int index)
        {
            foreach(var slot in playerSlots)
            {
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHighLight.gameObject.SetActive(true); 
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHighLight.gameObject.SetActive(false );
                }
            }
        }
    }
}
