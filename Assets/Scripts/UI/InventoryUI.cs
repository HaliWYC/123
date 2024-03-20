using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.U2D.Aseprite;

namespace ShanHai_IsolatedCity.Inventory
{
    public class InventoryUI : Singleton<InventoryUI>
    {
        public ItemToolTip itemToolTip;

        [Header("Drag Icon")]
        public Image dragItem;

        [Header("Player Bag UI")]

        [SerializeField] private GameObject bagUI;

        private bool bagOpened;
        private bool isFirstTime;

        [Header("Trade UI")]
        public TradeUI tradeUI;
        private string NPCName;
        public TextMeshProUGUI playerMoneyText;
        public TextMeshProUGUI NPCMoneyText;
        

        [Header("General Bag")]
        public GameObject baseBag;
        public GameObject slotPrefab;

        [SerializeField] private SlotUI[] playerSlots;
        public List<SlotUI> bagSlots;

        protected override void Awake()
        {
            isFirstTime = true;
        }

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            EventHandler.BaseBagCloseEvent += OnBaseBagCloseEvent;
            EventHandler.ShowTradeUI += OnShowTradeUI;
        }

        

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            EventHandler.BaseBagCloseEvent -= OnBaseBagCloseEvent;
            EventHandler.ShowTradeUI -= OnShowTradeUI;
        }

        private void Start()
        {
            
            //Give each slot a index 
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagUI.activeInHierarchy;

            playerMoneyText.text = InventoryManager.Instance.playerBag.Money.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnShowTradeUI(ItemDetails item, bool isSell, ItemType itemType)
        {
            tradeUI.gameObject.SetActive(true);

            tradeUI.SetUPTradeUI(item, isSell, itemType);
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            NPCName = InventoryManager.Instance.returnNPCName();
            switch (location)
            {
                case InventoryLocation.Player:
                    for(int i =0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            var Item = InventoryManager.Instance.GetItemDetails(list[i].itemID, list[i].Type);
                            playerSlots[i].UpDateSlot(Item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpDateEmptySlot();
                        }
                    }

                    break;

                case InventoryLocation.NPC:
           
                    for (int i = 0; i < bagSlots.Count; i++)
                    {
                        if (list[i].itemAmount == 0 || list[i].itemID == 0)
                            bagSlots[i].gameObject.SetActive(false);

                        if (list[i].itemAmount > 0)
                        {
                            var Item = InventoryManager.Instance.GetItemDetails(list[i].itemID, list[i].Type);
                            bagSlots[i].UpDateSlot(Item, list[i].itemAmount);
                        }
                        else
                        {
                            bagSlots[i].UpDateEmptySlot();
                        }
                    }

                        LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());
                    break;

            }
            playerMoneyText.text = InventoryManager.Instance.playerBag.Money.ToString();
            if(!isFirstTime)
            NPCMoneyText.text = NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money.ToString();
        }

        

        private void OnBeforeSceneUnloadEvent()
        {
            UpDateSlotHighLight(-1);
        }

        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            isFirstTime = false;
            GameObject prefab = slotType switch
            {
                SlotType.NPC背包 => slotPrefab,
                _ => null
            };
            
            baseBag.GetComponent<RectTransform>().anchoredPosition = new Vector2(125, 50);
            baseBag.SetActive(true);

            bagSlots = new List<SlotUI>();

            //if(!InventoryManager.Instance.isSell)
            for (int i = 0; i < bagData.itemList.Count; i++)
            {
                    var slot = Instantiate(prefab, baseBag.transform.GetChild(2)).GetComponent<SlotUI>();
                    
                    slot.slotIndex = i;
                    bagSlots.Add(slot);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());

            if (slotType == SlotType.NPC背包)
            {
                bagUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(-125, 50);
                
                bagUI.SetActive(true);
                bagOpened = true;
            }

            OnUpdateInventoryUI(InventoryLocation.NPC, bagData.itemList);
        }

        private void OnBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            baseBag.SetActive(false);
            baseBag.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            itemToolTip.gameObject.SetActive(false);
            UpDateSlotHighLight(-1);

            foreach(var slot in bagSlots)
            {
                Destroy(slot.gameObject); 
            }
            bagSlots.Clear();

            for(int i = 0; i < bagData.itemList.Count; i++)
            {
                if (bagData.itemList[i].itemID == 0 || bagData.itemList[i].itemAmount == 0)
                    bagData.itemList.Remove(bagData.itemList[i]);
            }

            if (slotType == SlotType.NPC背包)
            {
                
                bagUI.SetActive(false);
                bagUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                bagOpened = false;
            }
        }

        /*public void addSlotUIInNPCBag()
        {
            var slot =  Instantiate(slotPrefab, baseBag.transform.GetChild(2)).GetComponent<SlotUI>();
            slot.slotIndex = bagSlots.Count;
            bagSlots.Add(slot);
        }*/

        /// <summary>
        /// Open/Close BagUI 
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// Update Slot HighLight
        /// </summary>
        /// <param name="index">Index</param>

        public void UpDateSlotHighLight(int index)
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
