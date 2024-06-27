using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShanHai_IsolatedCity.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public Text playerSHGText;
        public Text playerGoldText;

        public EquipItemTips equipItemTip;
        public ConsumeItemTip consumeItemTip;
        public GameObject ConsumeItemPanel;

        [Header("General Bag")]
        public SlotUI Head;
        public SlotUI Neck;
        public SlotUI UpperBody;
        public SlotUI LowerBody;
        public SlotUI LeftHand;
        public SlotUI RightHand;
        public SlotUI Accessory;
        public SlotUI Shoe;
        public SlotUI Special;
        public SlotUI Mount;

        public GameObject equipBag;
        public GameObject equipSlot;
        [SerializeField] private List<SlotUI> equipSlots;
        public GameObject consumeBag;
        public GameObject consumeSlot;
        [SerializeField] private List<SlotUI> consumeSlots;
        public GameObject taskBag;
        public GameObject taskSlot;
        [SerializeField] private List<SlotUI> taskSlots;
        public GameObject otherBag;
        public GameObject otherSlot;
        [SerializeField] private List<SlotUI> otherSlots;

        public GameObject slotPrefab;
        
        public List<SlotUI> bagSlots;

        private void Awake()
        {
            InventoryManager.Instance.inventoryUI = this;
        }

        private void OnEnable()
        {
            EventHandler.UpdatePlayerInventoryUI += OnUpdatePlayerInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        }

        

        private void OnDisable()
        {
            EventHandler.UpdatePlayerInventoryUI -= OnUpdatePlayerInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        }

        private void Start()
        {
            
            //Give each slot a index 
            //for (int i = 0; i < equipSlots.Count; i++)
            //{
            //    equipSlots[i].slotIndex = i;
            //}
            //for (int i = 0; i < consumeSlots.Count; i++)
            //{
            //    consumeSlots[i].slotIndex = i;
            //}
            //for (int i = 0; i < taskSlots.Count; i++)
            //{
            //    taskSlots[i].slotIndex = i;
            //}
            //for (int i = 0; i < otherSlots.Count; i++)
            //{
            //    otherSlots[i].slotIndex = i;
            //}
            playerSHGText.text = InventoryManager.Instance.playerBag.ShanHaiGold.ToString();
            playerGoldText.text = InventoryManager.Instance.playerBag.Gold.ToString();
        }

        private void OnUpdatePlayerInventoryUI(ItemType itemType, List<InventoryItem> list)
        {
            switch (itemType)
            {
                case ItemType.Equip:
                    foreach (var slot in equipSlots)
                    {
                        Destroy(slot.gameObject);
                    }
                    equipSlots.Clear();

                    for (int i = 0; i < InventoryManager.Instance.playerBag.equipList.Count; i++)
                    {
                        if (InventoryManager.Instance.playerBag.equipList[i].itemAmount > 0)
                        {
                            var Item = Instantiate(equipSlot, equipBag.transform).GetComponent<SlotUI>();
                            equipSlots.Add(Item);
                            Item.UpDateSlot(InventoryManager.Instance.GetEquipItemDetails(list[i].itemID), list[i].Type, list[i].itemAmount);
                        }
                    }
                    break;
                case ItemType.Consume:
                    foreach (var slot in consumeSlots)
                    {
                        Destroy(slot.gameObject);
                    }
                    consumeSlots.Clear();

                    for (int i = 0; i < InventoryManager.Instance.playerBag.consumeList.Count; i++)
                    {
                        if (InventoryManager.Instance.playerBag.consumeList[i].itemAmount > 0)
                        {
                            var Item = Instantiate(consumeSlot, consumeBag.transform).GetComponent<SlotUI>();
                            consumeSlots.Add(Item);
                            Item.UpDateSlot(InventoryManager.Instance.GetConsumeItemDetails(list[i].itemID), list[i].Type, list[i].itemAmount);
                        }
                    }
                    break;
                case ItemType.Task:
                    foreach (var slot in taskSlots)
                    {
                        Destroy(slot.gameObject);
                    }
                    taskSlots.Clear();

                    for (int i = 0; i < InventoryManager.Instance.playerBag.taskList.Count; i++)
                    {
                        if (InventoryManager.Instance.playerBag.taskList[i].itemAmount > 0)
                        {
                            var Item = Instantiate(taskSlot, taskBag.transform).GetComponent<SlotUI>();
                            taskSlots.Add(Item);
                            Item.UpDateSlot(InventoryManager.Instance.GetTaskItemDetails(list[i].itemID), list[i].Type, list[i].itemAmount);
                        }
                    }
                    break;
                case ItemType.Other:
                    foreach (var slot in otherSlots)
                    {
                        Destroy(slot.gameObject);
                    }
                    otherSlots.Clear();

                    for (int i = 0; i < InventoryManager.Instance.playerBag.otherList.Count; i++)
                    {
                        if (InventoryManager.Instance.playerBag.otherList[i].itemAmount > 0)
                        {
                            var Item = Instantiate(otherSlot, otherBag.transform).GetComponent<SlotUI>();
                            otherSlots.Add(Item);
                            Item.UpDateSlot(InventoryManager.Instance.GetOtherItemDetails(list[i].itemID), list[i].Type, list[i].itemAmount);
                        }
                    }
                    break;

            }
            playerSHGText.text = InventoryManager.Instance.playerBag.ShanHaiGold.ToString();
            playerGoldText.text = InventoryManager.Instance.playerBag.Gold.ToString();
            
        }

        private void OnBeforeSceneUnloadEvent()
        {
            //UpDateSlotHighLight(-1);
        }

        //private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        //{
        //    //isFirstTime = false;
        //    GameObject prefab = slotType switch
        //    {
        //        SlotType.NPC背包 => slotPrefab,
        //        _ => null
        //    };
        //    equipBag.SetActive(true);

        //    bagSlots = new List<SlotUI>();

        //    //if(!InventoryManager.Instance.isSell)
        //    for (int i = 0; i < bagData.equipList.Count; i++)
        //    {
        //            var slot = Instantiate(prefab, equipBag.transform.GetChild(2)).GetComponent<SlotUI>();
                    
        //            slot.slotIndex = i;
        //            bagSlots.Add(slot);
        //    }

        //    LayoutRebuilder.ForceRebuildLayoutImmediate(equipBag.GetComponent<RectTransform>());

        //    if (slotType == SlotType.NPC背包)
        //    {
        //        bagUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-125, 50);
                
        //        bagUI.SetActive(true);
        //        bagOpened = true;
        //    }

        //    OnUpdateInventoryUI(InventoryLocation.NPC, ItemType.装备, bagData.equipList);
        //}

        //private void OnBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bagData)
        //{
        //    equipBag.SetActive(false);
            
        //    //UpDateSlotHighLight(-1);

        //    foreach(var slot in bagSlots)
        //    {
        //        Destroy(slot.gameObject); 
        //    }
        //    bagSlots.Clear();

        //    for(int i = 0; i < bagData.equipList.Count; i++)
        //    {
        //        if (bagData.equipList[i].itemID == 0 || bagData.equipList[i].itemAmount == 0)
        //            bagData.equipList.Remove(bagData.equipList[i]);
        //    }

        //    if (slotType == SlotType.NPC背包)
        //    {
                
        //        bagUI.SetActive(false);
        //        bagUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //        bagOpened = false;
        //    }
        //}

        /// <summary>
        /// Open/Close BagUI 
        /// </summary>
        //public void OpenBagUI()
        //{
        //    bagOpened = !bagOpened;
        //    bagUI.SetActive(bagOpened);
        //}

        /// <summary>
        /// Update Slot HighLight
        /// </summary>
        /// <param name="index">Index</param>

        //public void UpDateSlotHighLight(int index)
        //{
        //    foreach(var slot in playerSlots)
        //    {
        //        if(slot.isSelected && slot.slotIndex == index)
        //        {
        //            slot.slotHighLight.gameObject.SetActive(true); 
        //        }
        //        else
        //        {
        //            slot.isSelected = false;
        //            slot.slotHighLight.gameObject.SetActive(false );
        //        }
        //    }
        //}


    }
}
