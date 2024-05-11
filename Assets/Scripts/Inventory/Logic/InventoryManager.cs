using UnityEngine;
using UnityEngine.UI;

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
        public InventoryUI inventoryUI;
        public FightingDataDetailsUI fightingDataUI;

        [Header("交易数据")]
        private string NPCName;

        //public bool isSell =false;

        [Header("基础品质颜色")]
        public Color Red;
        public Color Orange;
        public Color Yellow;
        public Color Green;
        public Color Cyan;
        public Color Blue;
        public Color Purple;
        public Color Grey;

        private void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Equip, playerBag.equipList);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Consume, playerBag.consumeList);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Task, playerBag.taskList);
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Other, playerBag.otherList);
        }

        private void OnEnable()
        {
            EventHandler.UseConsumeItemEvent += OnUseConsumeItemEvent;
            EventHandler.DropItemEvent += OnDropItemEvent;
        }

        

        private void OnDisable()
        {
            EventHandler.UseConsumeItemEvent -= OnUseConsumeItemEvent;
            EventHandler.DropItemEvent -= OnDropItemEvent;
        }

        private void OnUseConsumeItemEvent(ConsumeItem_SO consume)
        {
            playerBag.Gold += consume.Gold;
            playerBag.ShanHaiGold += consume.ShanHaiGold;
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Consume, playerBag.consumeList);
        }

        private void OnDropItemEvent(int ID, Vector3 pos)
        {
            //RemoveItem(ID, 1,SlotType.人物背包);
        }

        #region GetItemDetails

        /// <summary>
        /// Use ID to return item
        /// </summary>
        /// <param name="ID">ItemID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID,ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Equip:
                    return equipItemDetailsList_SO.equipItemDetailsList.Find(i => i.itemID == ID);
                case ItemType.Consume:
                    return consumeItemDetailsList_SO.consumeItemDetailsList.Find(i => i.itemID == ID);
                case ItemType.Task:
                    return taskItemDetailsList_SO.taskItemDetailsList.Find(i => i.itemID == ID);
                case ItemType.Other:
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

        #endregion

        /// <summary>
        /// Add Item to Player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">Whether destroy object or not </param>
        public void AddItem(Item item, bool toDestory)
        {
            //Is Bag spare?

            var index = GetItemIndexInBag(item.itemID, item.itemType, SlotType.PlayerBag);

            AddItemAtIndex(item.itemID, index, item.itemType, 1,SlotType.PlayerBag);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }

            //Update UI
            switch (item.itemType)
            {
                case ItemType.Equip:
                    EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, item.itemType, playerBag.equipList);
                    break;
                case ItemType.Consume:
                    EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, item.itemType, playerBag.consumeList);
                    break;
                case ItemType.Task:
                    EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, item.itemType, playerBag.taskList);
                    break;
                case ItemType.Other:
                    EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, item.itemType, playerBag.otherList);
                    break;
            }

        }

        /// <summary>
        /// Is Bag spare?
        /// </summary>
        /// <returns></returns>
        //private bool CheckBagCapacity()
        //{
        //    for(int i = 0; i < playerBag.itemList.Count; i++)
        //    {
        //        if (playerBag.itemList[i].itemID == 0)
        //        {
        //            return true;
        //        }
                
        //    }
        //    return false;
        //}
        /// <summary>
        /// Use ID to find the position of the object had in the bag
        /// </summary>
        /// <param name="ID">Item ID</param>
        /// <returns>-1 means not exist else return the index</returns>
        private int GetItemIndexInBag(int ID,ItemType itemType,SlotType slotType)
        {
            switch (slotType)
            {
                case SlotType.PlayerBag:
                    switch (itemType)
                    {
                        case ItemType.Equip:
                            for (int i = 0; i < playerBag.equipList.Count; i++)
                            {
                                if (playerBag.equipList[i].itemID == ID)
                                {
                                    return i;
                                }

                            }
                            break;
                        case ItemType.Consume:
                            for (int i = 0; i < playerBag.consumeList.Count; i++)
                            {
                                if (playerBag.consumeList[i].itemID == ID)
                                {
                                    return i;
                                }

                            }
                            break;
                        case ItemType.Task:
                            for (int i = 0; i < playerBag.taskList.Count; i++)
                            {
                                if (playerBag.taskList[i].itemID == ID)
                                {
                                    return i;
                                }

                            }
                            break;
                        case ItemType.Other:
                            for (int i = 0; i < playerBag.otherList.Count; i++)
                            {
                                if (playerBag.otherList[i].itemID == ID)
                                {
                                    return i;
                                }

                            }
                            break;
                    }
                    
                    return -1;
                //case SlotType.NPCBag:
                //    for (int i = 0; i < NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList.Count; i++)
                //    {

                //        if (NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[i].itemID == ID)
                //        {
                //            return i;
                //        }

                //    }
                //    return -1;

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
                case SlotType.PlayerBag:
                    if (index == -1)//Does not have this item 
                    {
                        var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type=itemType };
                        switch (itemType)
                        {
                            case ItemType.Equip:
                                playerBag.equipList.Add(Item);
                                break;
                            case ItemType.Consume:
                                playerBag.consumeList.Add(Item);
                                break;
                            case ItemType.Task:
                                playerBag.taskList.Add(Item);
                                break;
                            case ItemType.Other:
                                playerBag.otherList.Add(Item);
                                break;

                        }

                        //for (int i = 0; i < playerBag.itemList.Count; i++)
                        //{
                        //    if (playerBag.itemList[i].itemID == 0)
                        //    {
                        //        playerBag.itemList[i] = Item;
                        //        break;
                        //    }
                        //}
                    }
                    else//Does have this item
                    {
                        if (GetItemDetails(ID, itemType).Stackable)
                        {
                            int currentAmount;
                            switch (itemType)
                            {
                                case ItemType.Equip:
                                    currentAmount = playerBag.equipList[index].itemAmount + amount;

                                    var equipItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                                    playerBag.equipList[index] = equipItem;
                                    break;
                                case ItemType.Consume:
                                    currentAmount = playerBag.consumeList[index].itemAmount + amount;

                                    var consumeItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                                    playerBag.consumeList[index] = consumeItem;
                                    break;
                                case ItemType.Task:
                                    currentAmount = playerBag.taskList[index].itemAmount + amount;

                                    var taskItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                                    playerBag.taskList[index] = taskItem;
                                    break;
                                case ItemType.Other:
                                    currentAmount = playerBag.otherList[index].itemAmount + amount;

                                    var otherItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                                    playerBag.otherList[index] = otherItem;
                                    break;
                            }
                        }

                        else if (!GetItemDetails(ID, itemType).Stackable)//Not Stackable
                        {
                            switch (itemType)
                            {
                                case ItemType.Equip:
                                    var equipItem = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                                    playerBag.equipList.Add(equipItem);
                                    break;
                                case ItemType.Consume:
                                    var consumeItem = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                                    playerBag.consumeList.Add(consumeItem);
                                    break;
                                case ItemType.Task:
                                    var taskItem = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                                    playerBag.taskList.Add(taskItem);
                                    break;
                                case ItemType.Other:
                                    var otherItem = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                                    playerBag.otherList.Add(otherItem);
                                    break;
                            }
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                    break;
                //case SlotType.NPCBag:
                //    if (index == -1 )//Does not have this item 
                //    {
                //        //isSell = true;
                //        var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                //        NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList.Add(Item);
                //        /*EventHandler.callBaseBagOpenEvent(SlotType.NPC背包, NPCManager.Instance.getNPCDetail(NPCName).NPCBag);
                //        isSell = false;*/
                //        EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, itemType, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList);
                        
                //    }
                //    else//Does have this item
                //    {
                //        if (GetItemDetails(ID, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index].Type).Stackable)
                //        {
                //            int currentAmount = NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index].itemAmount + amount;

                //            var Item = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                //            NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index] = Item;
                //        }

                //        else if (!GetItemDetails(ID, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index].Type).Stackable)//Not Stackable
                //        {
                //            //isSell = true;
                //            var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type = itemType };
                //            NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList.Add(Item);
                //            /*EventHandler.callBaseBagOpenEvent(SlotType.NPC背包, NPCManager.Instance.getNPCDetail(NPCName).NPCBag);
                //            isSell = false;*/
                //            EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, itemType, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList);
                //        }
                //        else
                //        {
                //            return;
                //        }

                //    }
                //    break;
            }

            

        }
        

        /// <summary>
        /// Remove certain amount of item in bag
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="removeAmount">Amount</param>
        public void RemoveItem(int ID, ItemType itemType,int removeAmount,SlotType slotType)
        {
            int index = GetItemIndexInBag(ID,itemType,slotType);
            switch (slotType)
            {
                case SlotType.PlayerBag:
                    switch (itemType)
                    {
                        case ItemType.Equip:
                            if (playerBag.equipList[index].itemAmount > removeAmount)
                            {
                                var amount = playerBag.equipList[index].itemAmount - removeAmount;
                                var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = playerBag.equipList[index].Type };
                                playerBag.equipList[index] = item;
                            }
                            else if (playerBag.equipList[index].itemAmount == removeAmount)
                            {
                                playerBag.equipList.RemoveAt(index);
                            }
                            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, itemType, playerBag.equipList);
                            break;
                        case ItemType.Consume:
                            if (playerBag.consumeList[index].itemAmount > removeAmount)
                            {
                                var amount = playerBag.consumeList[index].itemAmount - removeAmount;
                                var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = playerBag.consumeList[index].Type };
                                playerBag.consumeList[index] = item;
                            }
                            else if (playerBag.consumeList[index].itemAmount == removeAmount)
                            {
                                playerBag.consumeList.RemoveAt(index);
                            }
                            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, itemType, playerBag.consumeList);
                            break;
                        case ItemType.Task:
                            if (playerBag.taskList[index].itemAmount > removeAmount)
                            {
                                var amount = playerBag.taskList[index].itemAmount - removeAmount;
                                var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = playerBag.taskList[index].Type };
                                playerBag.taskList[index] = item;
                            }
                            else if (playerBag.taskList[index].itemAmount == removeAmount)
                            {
                                playerBag.taskList.RemoveAt(index);
                            }
                            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, itemType, playerBag.taskList);
                            break;
                        case ItemType.Other:
                            if (playerBag.otherList[index].itemAmount > removeAmount)
                            {
                                var amount = playerBag.otherList[index].itemAmount - removeAmount;
                                var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = playerBag.otherList[index].Type };
                                playerBag.otherList[index] = item;
                            }
                            else if (playerBag.otherList[index].itemAmount == removeAmount)
                            {
                                playerBag.otherList.RemoveAt(index);
                            }
                            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, itemType, playerBag.otherList);
                            break;
                    }
                    break;
                //case SlotType.NPCBag:
                //    if (NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index].itemAmount > removeAmount)
                //    {
                //        var amount = NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index].itemAmount - removeAmount;
                //        var item = new InventoryItem { itemID = ID, itemAmount = amount };
                //        //InventoryUI.Instance.addSlotUIInNPCBag();
                //        NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index] = item;
                //    }
                //    else if (NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index].itemAmount == removeAmount)
                //    {
                //        var NPCItem = new InventoryItem();
                //        NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList[index] = NPCItem;
                //    }

                //    EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, itemType, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.equipList);
                //    break;
                    
            }

            
        }

        /// <summary>
        /// Equip item on character
        /// </summary>
        /// <param name="equip"></param>
        public void EquipItem(EquipItemDetails equip)
        {
            switch (equip.equipItemType)
            {
                case EquipItemType.Head:
                    if (inventoryUI.Head.itemDetails != null && inventoryUI.Head.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.Head.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.Head.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.Head.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.Neck:
                    if (inventoryUI.Neck.itemDetails != null && inventoryUI.Neck.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.Neck.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.Neck.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.Neck.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.UpperBody:
                    if (inventoryUI.UpperBody.itemDetails != null && inventoryUI.UpperBody.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.UpperBody.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.UpperBody.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.UpperBody.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.LowerBody:
                    if (inventoryUI.LowerBody.itemDetails != null && inventoryUI.LowerBody.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.LowerBody.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.LowerBody.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.LowerBody.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.LeftHand:
                    if (inventoryUI.LeftHand.itemDetails != null && inventoryUI.LeftHand.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.LeftHand.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.LeftHand.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.LeftHand.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.RightHand:
                    if (inventoryUI.RightHand.itemDetails != null && inventoryUI.RightHand.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.RightHand.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.RightHand.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.RightHand.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.Accessory:
                    if (inventoryUI.Accessory.itemDetails != null && inventoryUI.Accessory.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.Shoe.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.Accessory.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.Accessory.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.Shoe:
                    if (inventoryUI.Shoe.itemDetails != null && inventoryUI.Shoe.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.Shoe.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.Shoe.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.Shoe.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.Special:
                    if (inventoryUI.Special.itemDetails != null && inventoryUI.Special.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.Special.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.Special.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.Special.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
                case EquipItemType.Mount:
                    if (inventoryUI.Mount.itemDetails != null && inventoryUI.Mount.itemDetails.itemID != 0)
                    {
                        var Item = new InventoryItem { itemID = inventoryUI.Mount.itemDetails.itemID, itemAmount = 1, Type = ItemType.Equip };
                        playerBag.equipList.Add(Item);
                        GameManager.Instance.playerInformation.ChangeItem(GetEquipItemDetails(inventoryUI.Mount.itemDetails.itemID).EquipData, equip.EquipData);
                    }
                    else
                    {
                        GameManager.Instance.playerInformation.EquipItem(equip);
                    }
                    RemoveItem(equip.itemID, equip.itemType, 1, SlotType.PlayerBag);
                    inventoryUI.Mount.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Equip, playerBag.equipList);
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryUI.equipBag.GetComponent<RectTransform>());
        }
        /// <summary>
        /// Take off item from character
        /// </summary>
        /// <param name="equip"></param>
        public void TakeOffItem(EquipItemDetails equip)
        {
            var Item = new InventoryItem { itemID = equip.itemID, itemAmount = 1, Type = ItemType.Equip };
            playerBag.equipList.Add(Item);
            switch (equip.equipItemType)
            {
                case EquipItemType.Head:
                    inventoryUI.Head.UpDateEmptySlot();
                    break;
                case EquipItemType.Neck:
                    inventoryUI.Neck.UpDateEmptySlot();
                    break;
                case EquipItemType.UpperBody:
                    inventoryUI.UpperBody.UpDateEmptySlot();
                    break;
                case EquipItemType.LowerBody:
                    inventoryUI.LowerBody.UpDateEmptySlot();
                    break;
                case EquipItemType.LeftHand:
                    inventoryUI.LeftHand.UpDateEmptySlot();
                    break;
                case EquipItemType.RightHand:
                    inventoryUI.RightHand.UpDateEmptySlot();
                    break;
                case EquipItemType.Accessory:
                    inventoryUI.Accessory.UpDateEmptySlot();
                    break;
                case EquipItemType.Shoe:
                    inventoryUI.Shoe.UpDateEmptySlot();
                    break;
                case EquipItemType.Special:
                    inventoryUI.Special.UpDateEmptySlot();
                    break;
                case EquipItemType.Mount:
                    inventoryUI.Mount.UpDateEmptySlot();
                    break;
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, ItemType.Equip, playerBag.equipList);
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryUI.equipBag.GetComponent<RectTransform>());
            GameManager.Instance.playerInformation.TakeOffItem(equip);
        }


        public void UseConsumeItem(ConsumeItemDetails consumeItem, int amount)
        {
            int index = GetItemIndexInBag(consumeItem.itemID, ItemType.Consume, SlotType.PlayerBag);
            if (playerBag.consumeList[index].itemAmount >= amount)
            {
                RemoveItem(consumeItem.itemID, ItemType.Consume, amount, SlotType.PlayerBag);
                for (int itemIndex = 0; itemIndex < amount; itemIndex++)
                    EventHandler.CallUseConsumeItemEvent(consumeItem.consumeData);
            }
            else
                Debug.Log("I do not have that amount of item");
        }

        //public  void TradeItem(ItemDetails itemDetails,int amount,bool isSellTrade)
        //{

        //    int cost = itemDetails.itemPrice * amount;


        //    //Gain the obeject in bag
        //    int index = GetItemIndexInBag(itemDetails.itemID,SlotType.人物背包);
        //    int NPCBagIndex = GetItemIndexInBag(itemDetails.itemID,SlotType.NPC背包);

        //    //Debug.Log(NPCManager.Instance.getNPCDetail(NPCName).NPCBag.itemList[NPCBagIndex].itemAmount);
        //    if (isSellTrade) //Sell 
        //    {

        //        if (playerBag.itemList[index].itemAmount >= amount && NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money > (int)(cost*itemDetails.sellPercentage))
        //        {
        //            RemoveItem(itemDetails.itemID, amount,SlotType.人物背包);
        //            AddItemAtIndex(itemDetails.itemID, NPCBagIndex, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[NPCBagIndex].Type, amount, SlotType.NPC背包);
        //            //Total revenue
        //            int presentCost = (int)(cost * itemDetails.sellPercentage);
        //            playerBag.Money += presentCost;
        //            NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money -= presentCost;
        //        }
        //        else if(playerBag.itemList[index].itemAmount < amount)
        //        {
        //            Debug.Log("I do not have these amount of good");
        //        }
        //        else
        //        {
        //            Debug.Log("NPC does not have these amount of money");
        //        }

        //    }
        //    else if (!isSellTrade)//Buy
        //    {

        //        if(playerBag.Money - cost >= 0 && amount <= NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList[NPCBagIndex].itemAmount)
        //        {
        //            if (CheckBagCapacity())
        //            {
        //                AddItemAtIndex(itemDetails.itemID, index, playerBag.itemList[index].Type, amount, SlotType.人物背包);
        //                RemoveItem(itemDetails.itemID, amount, SlotType.NPC背包);
        //            }
        //            playerBag.Money -= cost;
        //            NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.Money += cost;
        //        }
        //        else 
        //        {
        //            Debug.Log("I do not have these much of item");
        //            return; 
        //        }

        //    }
        //    //Refresh UI
        //    EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        //    EventHandler.CallUpdateInventoryUI(InventoryLocation.NPC, NPCManager.Instance.GetNPCDetail(NPCName).NPCBag.itemList);
        //}

        //public string returnNPCName()
        //{
        //    return NPCName;
        //}


        public Color GetQualityColor(BasicQualityType quality)
        {
            Color qualityColor;
            qualityColor = quality
                switch
            {
                BasicQualityType.赤 => Red,
                BasicQualityType.橙 => Orange,
                BasicQualityType.黄 => Yellow,
                BasicQualityType.绿 => Green,
                BasicQualityType.青 => Cyan,
                BasicQualityType.蓝 => Blue,
                BasicQualityType.紫 => Purple,
                _ => Grey
            };
            return qualityColor;
        }

        public Color GetEquipQualityColor(EquipQualityType quality)
        {
            Color qualityColor;
            qualityColor = quality
                switch
            {
                EquipQualityType.神器 => Red,
                EquipQualityType.传说 => Orange,
                EquipQualityType.史诗 => Yellow,
                EquipQualityType.卓越 => Green,
                EquipQualityType.精良 => Cyan,
                EquipQualityType.优秀 => Blue,
                EquipQualityType.普通 => Purple,
                _ => Grey
            };
            return qualityColor;
        }
    }


}
