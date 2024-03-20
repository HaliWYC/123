using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;

namespace ShanHai_IsolatedCity.Inventory
{


    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public EquipItemDetailsList_SO equipItemDetailsList_SO;
        public ConsumeItemDetailsList_SO consumeItemDetailsList_SO;
        public TaskItemDetailsList_SO taskItemDetailsList_SO;
        public OtherItemDetailsList_SO otherItemDetailsList_SO;
        

        [Header("背包数据")]

        public InventoryBag_SO playerBag;
        


        [Header("交易数据")]
        private string NPCName;

        //public bool isSell =false;
        
        


        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void OnEnable()
        {
            EventHandler.FindNPCEvent += OnFindNPCEvent;
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        

        private void OnDisable()
        {
            EventHandler.FindNPCEvent -= OnFindNPCEvent;
            EventHandler.DropItemEvent -= OnDropItemEvent;
        }

        

        private void OnFindNPCEvent(string Name)
        {
            
            NPCName = Name;
        }

        private void OnDropItemEvent(int ID, Vector3 pos)
        {
            RemoveItem(ID, 1,SlotType.人物背包);
        }


        /// <summary>
        /// Use ID to return item
        /// </summary>
        /// <param name="ID">ItemID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID,ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.装备:
                    return equipItemDetailsList_SO.equipItemDetailsList.Find(i => i.itemID == ID);
                case ItemType.消耗品:
                    return consumeItemDetailsList_SO.consumeItemDetailsList.Find(i => i.itemID == ID);
                case ItemType.任务物品:
                    return taskItemDetailsList_SO.taskItemDetailsList.Find(i => i.itemID == ID);
                case ItemType.其他物品:
                    return otherItemDetailsList_SO.otherItemDetailsList.Find(i => i.itemID == ID);
            }
            return null;
        }


        public EquipItemDetails GetEquipItemDetails(int ID)
        {
            return equipItemDetailsList_SO.equipItemDetailsList.Find(i => i.itemID == ID);
        }

        public ConsumeItemDetails GetConsumeItemDetails(int ID)
        {
            return consumeItemDetailsList_SO.consumeItemDetailsList.Find(i => i.itemID == ID);
        }

        public TaskItemDetails GetTaskItemDetails(int ID)
        {
            return taskItemDetailsList_SO.taskItemDetailsList.Find(i => i.itemID == ID);
        }

        public OtherItemDetails GetOtherItemDetails(int ID)
        {
            return otherItemDetailsList_SO.otherItemDetailsList.Find(i => i.itemID == ID);
        }


        /// <summary>
        /// Add Item to Player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">Whether destroy object or not </param>
         public void AddItem(Item item, bool toDestory)
        {
          //Is Bag spare?

          var index = GetItemIndexInBag(item.itemID,SlotType.人物背包);

            AddItemAtIndex(item.itemID, index, item.itemType, 1,SlotType.人物背包);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }

            //Update UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// Is Bag spare?
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
                
            }
            return false;
        }
        /// <summary>
        /// Use ID to find the position of the object had in the bag
        /// </summary>
        /// <param name="ID">Item ID</param>
        /// <returns>-1 means not exist else return the index</returns>
        private int GetItemIndexInBag(int ID,SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.人物背包:
                    for (int i = 0; i < playerBag.itemList.Count; i++)
                    {
                        if (playerBag.itemList[i].itemID == ID)
                        {
                            return i;
                        }

                    }
                    return -1;
                case SlotType.NPC背包:
                    for (int i = 0; i < NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList.Count; i++)
                    {

                        if (NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[i].itemID == ID)
                        {
                            return i;
                        }

                    }
                    return -1;

            }
            return -1;
            
        }        

        /// <summary>
        /// Add the item at the certain index
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        private void AddItemAtIndex(int ID, int index, ItemType itemType, int amount,SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.人物背包:
                    if (index == -1 && CheckBagCapacity())//Does not have this item 
                    {
                        var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type=itemType };

                        for (int i = 0; i < playerBag.itemList.Count; i++)
                        {
                            if (playerBag.itemList[i].itemID == 0)
                            {
                                playerBag.itemList[i] = Item;
                                break;
                            }
                        }
                    }
                    else//Does have this item
                    {
                        if (GetItemDetails(ID, itemType).Stackable)
                        {
                            int currentAmount = playerBag.itemList[index].itemAmount + amount;

                            var Item = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type=itemType};

                            playerBag.itemList[index] = Item;
                        }

                        else if (!GetItemDetails(ID, itemType).Stackable && CheckBagCapacity())//Not Stackable
                        {
                            var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type=itemType };

                            for (int i = 0; i < playerBag.itemList.Count; i++)
                            {
                                if (playerBag.itemList[i].itemID == 0)
                                {
                                    playerBag.itemList[i] = Item;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                    break;
                case SlotType.NPC背包:
                    if (index == -1 )//Does not have this item 
                    {
                        //isSell = true;
                        var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                        NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList.Add(Item);
                        /*EventHandler.callBaseBagOpenEvent(SlotType.NPC背包, NPCManager.Instance.getNPCDetail(NPCName).NPCBag);
                        isSell = false;*/
                        EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC,NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList);
                        
                    }
                    else//Does have this item
                    {
                        if (GetItemDetails(ID, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index].Type).Stackable)
                        {
                            int currentAmount = NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index].itemAmount + amount;

                            var Item = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                            NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index] = Item;
                        }

                        else if (!GetItemDetails(ID, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index].Type).Stackable)//Not Stackable
                        {
                            //isSell = true;
                            var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                            NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList.Add(Item);
                            /*EventHandler.callBaseBagOpenEvent(SlotType.NPC背包, NPCManager.Instance.getNPCDetail(NPCName).NPCBag);
                            isSell = false;*/
                            EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList);
                        }
                        else
                        {
                            return;
                        }

                    }
                    break;
            }

            

        }
        /// <summary>
        /// Swap Item in Player's bag
        /// </summary>
        /// <param name="initialIndex">Initial Index</param>
        /// <param name="targetIndex">Target Index</param>
        public void SwapItem(int initialIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[initialIndex];

            InventoryItem targetItem = playerBag.itemList[targetIndex];

            if (targetItem.itemID != 0)
            {
                playerBag.itemList[initialIndex] = targetItem;
                playerBag.itemList[targetIndex] =  currentItem;
            }
            else
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[initialIndex] = new InventoryItem();
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// Remove certain amount of item in bag
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="removeAmount">Amount</param>
        private void RemoveItem(int ID, int removeAmount,SlotType slotType)
        {
            int index = GetItemIndexInBag(ID, slotType);
            switch (slotType)
            {
                case SlotType.人物背包:
                    if (playerBag.itemList[index].itemAmount > removeAmount)
                    {
                        var amount = playerBag.itemList[index].itemAmount - removeAmount;
                        var item = new InventoryItem { itemID = ID, itemAmount = amount };
                        playerBag.itemList[index] = item;
                    }
                    else if (playerBag.itemList[index].itemAmount == removeAmount)
                    {
                        var item = new InventoryItem();
                        playerBag.itemList[index] = item;
                    }

                    EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
                    break;
                case SlotType.NPC背包:
                    if (NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index].itemAmount > removeAmount)
                    {
                        var amount = NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index].itemAmount - removeAmount;
                        var item = new InventoryItem { itemID = ID, itemAmount = amount };
                        //InventoryUI.Instance.addSlotUIInNPCBag();
                        NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index] = item;
                    }
                    else if (NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index].itemAmount == removeAmount)
                    {
                        var NPCItem = new InventoryItem();
                        NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[index] = NPCItem;
                    }

                    EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList);
                    break;
                    
            }

            
        }

        public  void TradeItem(ItemDetails itemDetails,int amount,bool isSellTrade)
        {
            
            int cost = itemDetails.itemPrice * amount;
            

            //Gain the obeject in bag
            int index = GetItemIndexInBag(itemDetails.itemID,SlotType.人物背包);
            int NPCBagIndex = GetItemIndexInBag(itemDetails.itemID,SlotType.NPC背包);

            //Debug.Log(NPCManager.Instance.getNPCDetail(NPCName).NPCBag.itemList[NPCBagIndex].itemAmount);
            if (isSellTrade) //Sell 
            {
                
                if (playerBag.itemList[index].itemAmount >= amount && NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money > (int)(cost*itemDetails.sellPercentage))
                {
                    RemoveItem(itemDetails.itemID, amount,SlotType.人物背包);
                    AddItemAtIndex(itemDetails.itemID, NPCBagIndex, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[NPCBagIndex].Type, amount, SlotType.NPC背包);
                    //Total revenue
                    int presentCost = (int)(cost * itemDetails.sellPercentage);
                    playerBag.Money += presentCost;
                    NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money -= presentCost;
                }
                else if(playerBag.itemList[index].itemAmount < amount)
                {
                    Debug.Log("I do not have these amount of good");
                }
                else
                {
                    Debug.Log("NPC does not have these amount of money");
                }
                
            }
            else if (!isSellTrade)//Buy
            {
                
                if(playerBag.Money - cost >= 0 && amount <= NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[NPCBagIndex].itemAmount)
                {
                    if (CheckBagCapacity())
                    {
                        AddItemAtIndex(itemDetails.itemID, index, playerBag.itemList[index].Type, amount, SlotType.人物背包);
                        RemoveItem(itemDetails.itemID, amount, SlotType.NPC背包);
                    }
                    playerBag.Money -= cost;
                    NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money += cost;
                }
                else 
                {
                    Debug.Log("I do not have these much of item");
                    return; 
                }
                
            }
            //Refresh UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList);
        }

        public string returnNPCName()
        {
            return NPCName;
        }

    }


}
