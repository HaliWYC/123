using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Equip, playerBag.equipList);
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Consume, playerBag.consumeList);
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Task, playerBag.taskList);
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Other, playerBag.otherList);
        }

        private void UseConsumeItem(ConsumeItem_SO consume)
        {
            playerBag.Gold += consume.Gold;
            playerBag.ShanHaiGold += consume.ShanHaiGold;
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Consume, playerBag.consumeList);
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

        public ItemDetails GetItemDetails(int ID, string itemName)
        {
            ItemDetails item = null;
            item = equipItemDetailsList_SO.equipItemDetailsList.Find(i => i.itemID == ID && i.itemName == itemName);
            if (item != null)
                return item;
            item = consumeItemDetailsList_SO.consumeItemDetailsList.Find(i => i.itemID == ID && i.itemName == itemName);
            if (item != null)
                return item;
            item = taskItemDetailsList_SO.taskItemDetailsList.Find(i => i.itemID == ID && i.itemName == itemName);
            if (item != null)
                return item;
            item = otherItemDetailsList_SO.otherItemDetailsList.Find(i => i.itemID == ID && i.itemName == itemName);
            return item;
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

        #region ManipulateItem
        /// <summary>
        /// Add Item to Player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">Whether destroy object or not </param>
        public void AddItem(Item item)
        {
            //Is Bag spare?

            var index = GetItemIndexInBag(item.itemID, item.itemType);

            AddItemAtIndex(item.itemID, index, item.itemType, 1);
            //Update UI
            switch (item.itemType)
            {
                case ItemType.Equip:
                    EventHandler.CallUpdatePlayerInventoryUI(item.itemType, playerBag.equipList);
                    break;
                case ItemType.Consume:
                    EventHandler.CallUpdatePlayerInventoryUI(item.itemType, playerBag.consumeList);
                    break;
                case ItemType.Task:
                    EventHandler.CallUpdatePlayerInventoryUI(item.itemType, playerBag.taskList);
                    break;
                case ItemType.Other:
                    EventHandler.CallUpdatePlayerInventoryUI(item.itemType, playerBag.otherList);
                    break;
            }

        }

        public void AddItem(ItemDetails itemDetails ,int amount)
        {
            var index = GetItemIndexInBag(itemDetails.itemID, itemDetails.itemType);

            AddItemAtIndex(itemDetails.itemID, index, itemDetails.itemType, amount);
            //Update UI
            switch (itemDetails.itemType)
            {
                case ItemType.Equip:
                    EventHandler.CallUpdatePlayerInventoryUI(itemDetails.itemType, playerBag.equipList);
                    break;
                case ItemType.Consume:
                    EventHandler.CallUpdatePlayerInventoryUI(itemDetails.itemType, playerBag.consumeList);
                    break;
                case ItemType.Task:
                    EventHandler.CallUpdatePlayerInventoryUI(itemDetails.itemType, playerBag.taskList);
                    break;
                case ItemType.Other:
                    EventHandler.CallUpdatePlayerInventoryUI(itemDetails.itemType, playerBag.otherList);
                    break;
            }
        }

        public void AddItem(ItemDetails itemDetails, int amount, NPCDetails NPC)
        {
            var index = GetItemIndexInBag(itemDetails.itemID, itemDetails.itemType, NPC);

            AddItemAtIndex(itemDetails.itemID, index, itemDetails.itemType, amount, NPC);
            //Update UI
            TradeUI.Instance.UpdateNPCBagItems();
        }

        private void AddNewItem(int ID, int amount, ItemType type)
        {
            if(GetItemDetails(ID, type).Stackable)
            {
                var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type = type };
                switch (type)
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
            }
            else
            {
                for(int index = 0; index < amount; index++)
                {
                    var Item = new InventoryItem { itemID = ID, itemAmount = 1, Type = type };
                    switch (type)
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
                }
            }
        }
        private void AddNewItem(int ID, int amount, ItemType type, NPCDetails NPC)
        {
            if (GetItemDetails(ID, type).Stackable)
            {
                var Item = new InventoryItem { itemID = ID, itemAmount = amount, Type = type };
                switch (type)
                {
                    case ItemType.Equip:
                        NPC.NPCBag.equipList.Add(Item);
                        break;
                    case ItemType.Consume:
                        NPC.NPCBag.consumeList.Add(Item);
                        break;
                    case ItemType.Task:
                        NPC.NPCBag.taskList.Add(Item);
                        break;
                    case ItemType.Other:
                        NPC.NPCBag.otherList.Add(Item);
                        break;

                }
            }
            else
            {
                for (int index = 0; index < amount; index++)
                {
                    var Item = new InventoryItem { itemID = ID, itemAmount = 1, Type = type };
                    switch (type)
                    {
                        case ItemType.Equip:
                            NPC.NPCBag.equipList.Add(Item);
                            break;
                        case ItemType.Consume:
                            NPC.NPCBag.consumeList.Add(Item);
                            break;
                        case ItemType.Task:
                            NPC.NPCBag.taskList.Add(Item);
                            break;
                        case ItemType.Other:
                            NPC.NPCBag.otherList.Add(Item);
                            break;

                    }
                }
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
        private int GetItemIndexInBag(int ID,ItemType itemType)
        {
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
        }

        private int GetItemIndexInBag(int ID, ItemType itemType, NPCDetails NPC)
        {
            switch (itemType)
            {
                case ItemType.Equip:
                    for (int i = 0; i < NPC.NPCBag.equipList.Count; i++)
                    {
                        if (NPC.NPCBag.equipList[i].itemID == ID)
                        {
                            return i;
                        }
                    }
                    break;
                case ItemType.Consume:
                    for (int i = 0; i < NPC.NPCBag.consumeList.Count; i++)
                    {
                        if (NPC.NPCBag.consumeList[i].itemID == ID)
                        {
                            return i;
                        }
                    }
                    break;
                case ItemType.Task:
                    for (int i = 0; i < NPC.NPCBag.taskList.Count; i++)
                    {
                        if (NPC.NPCBag.taskList[i].itemID == ID)
                        {
                            return i;
                        }
                    }
                    break;
                case ItemType.Other:
                    for (int i = 0; i < NPC.NPCBag.otherList.Count; i++)
                    {
                        if (NPC.NPCBag.otherList[i].itemID == ID)
                        {
                            return i;
                        }
                    }
                    break;
            }
            return -1;
        }

        /// <summary>
        /// Add the item at the certain index
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        private void AddItemAtIndex(int ID, int index, ItemType itemType, int amount)
        {
            if (index == -1)//Does not have this item 
            {
                AddNewItem(ID, amount, itemType);
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
                else
                {
                    AddNewItem(ID, amount, itemType);
                }
            }
        }

        private void AddItemAtIndex(int ID, int index, ItemType itemType, int amount, NPCDetails NPC)
        {
            if (index == -1)//Does not have this item 
            {
                AddNewItem(ID, amount, itemType, NPC);
            }
            else//Does have this item
            {
                if (GetItemDetails(ID, itemType).Stackable)
                {
                    int currentAmount;
                    switch (itemType)
                    {
                        case ItemType.Equip:
                            currentAmount = NPC.NPCBag.equipList[index].itemAmount + amount;

                            var equipItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                            NPC.NPCBag.equipList[index] = equipItem;
                            break;
                        case ItemType.Consume:
                            currentAmount = NPC.NPCBag.consumeList[index].itemAmount + amount;

                            var consumeItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                            NPC.NPCBag.consumeList[index] = consumeItem;
                            break;
                        case ItemType.Task:
                            currentAmount = NPC.NPCBag.taskList[index].itemAmount + amount;

                            var taskItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                            NPC.NPCBag.taskList[index] = taskItem;
                            break;
                        case ItemType.Other:
                            currentAmount = NPC.NPCBag.otherList[index].itemAmount + amount;

                            var otherItem = new InventoryItem { itemID = ID, itemAmount = currentAmount, Type = itemType };

                            NPC.NPCBag.otherList[index] = otherItem;
                            break;
                    }
                }
                else
                {
                    AddNewItem(ID, amount, itemType, NPC);
                }
            }

        }
        /// <summary>
        /// Remove certain amount of item in bag
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="removeAmount">Amount</param>
        public void RemoveItem(int ID, ItemType itemType,int removeAmount)
        {
            int index = GetItemIndexInBag(ID,itemType);
            if (index != -1)
            {
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
                        EventHandler.CallUpdatePlayerInventoryUI(itemType, playerBag.equipList);
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
                        EventHandler.CallUpdatePlayerInventoryUI(itemType, playerBag.consumeList);
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
                        EventHandler.CallUpdatePlayerInventoryUI(itemType, playerBag.taskList);
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
                        EventHandler.CallUpdatePlayerInventoryUI(itemType, playerBag.otherList);
                        break;
                }
            }
            
        }
        public void RemoveItem(int ID, ItemType itemType, int removeAmount, NPCDetails NPC)
        {
            int index = GetItemIndexInBag(ID, itemType, NPC);
            if (index != -1)
            {
                switch (itemType)
                {
                    case ItemType.Equip:
                        if (NPC.NPCBag.equipList[index].itemAmount > removeAmount)
                        {
                            var amount = NPC.NPCBag.equipList[index].itemAmount - removeAmount;
                            var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = NPC.NPCBag.equipList[index].Type };
                            NPC.NPCBag.equipList[index] = item;
                        }
                        else if (NPC.NPCBag.equipList[index].itemAmount == removeAmount)
                        {
                            NPC.NPCBag.equipList.RemoveAt(index);
                        }
                        break;
                    case ItemType.Consume:
                        if (NPC.NPCBag.consumeList[index].itemAmount > removeAmount)
                        {
                            var amount = NPC.NPCBag.consumeList[index].itemAmount - removeAmount;
                            var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = NPC.NPCBag.consumeList[index].Type };
                            NPC.NPCBag.consumeList[index] = item;
                        }
                        else if (NPC.NPCBag.consumeList[index].itemAmount == removeAmount)
                        {
                            NPC.NPCBag.consumeList.RemoveAt(index);
                        }

                        break;
                    case ItemType.Task:
                        if (NPC.NPCBag.taskList[index].itemAmount > removeAmount)
                        {
                            var amount = NPC.NPCBag.taskList[index].itemAmount - removeAmount;
                            var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = NPC.NPCBag.taskList[index].Type };
                            NPC.NPCBag.taskList[index] = item;
                        }
                        else if (NPC.NPCBag.taskList[index].itemAmount == removeAmount)
                        {
                            NPC.NPCBag.taskList.RemoveAt(index);
                        }
                        break;
                    case ItemType.Other:
                        if (NPC.NPCBag.otherList[index].itemAmount > removeAmount)
                        {
                            var amount = NPC.NPCBag.otherList[index].itemAmount - removeAmount;
                            var item = new InventoryItem { itemID = ID, itemAmount = amount, Type = NPC.NPCBag.otherList[index].Type };
                            NPC.NPCBag.otherList[index] = item;
                        }
                        else if (NPC.NPCBag.otherList[index].itemAmount == removeAmount)
                        {
                            NPC.NPCBag.otherList.RemoveAt(index);
                        }
                        break;
                }
                TradeUI.Instance.UpdateNPCBagItems();
            }

        }

        #endregion

        #region ManipulateCharacterData
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
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
                    RemoveItem(equip.itemID, equip.itemType, 1);
                    inventoryUI.Mount.UpDateSlot(equip, ItemType.Equip, 1);
                    break;
            }
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Equip, playerBag.equipList);
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
            EventHandler.CallUpdatePlayerInventoryUI(ItemType.Equip, playerBag.equipList);
            LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryUI.equipBag.GetComponent<RectTransform>());
            GameManager.Instance.playerInformation.TakeOffItem(equip);
        }


        public void UseConsumeItem(ConsumeItemDetails consumeItem, int amount)
        {
            int index = GetItemIndexInBag(consumeItem.itemID, ItemType.Consume);
            if (playerBag.consumeList[index].itemAmount >= amount)
            {
                RemoveItem(consumeItem.itemID, ItemType.Consume, amount);
                for (int itemIndex = 0; itemIndex < amount; itemIndex++)
                {
                    GameManager.Instance.playerInformation.UseConsumeItem(consumeItem.consumeData);
                    UseConsumeItem(consumeItem.consumeData);
                }
                EventHandler.CallUpdateTaskProgressEvent(consumeItem.itemName, amount);
            }
            //FIXME:Generate proper prompt
            else
                Debug.Log("I do not have that amount of item");
        }

        #endregion

        #region GetColor

        public Color GetBasicQualityColor(BasicQualityType quality)
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
        #endregion

        #region Task

        public void CheckItemInBag(string itemName)
        {
            foreach(var item in playerBag.equipList)
            {
                if (GetEquipItemDetails(item.itemID).itemName == itemName)
                    EventHandler.CallUpdateTaskProgressEvent(itemName, item.itemAmount);
            }
            foreach (var item in playerBag.consumeList)
            {
                if (GetConsumeItemDetails(item.itemID).itemName == itemName)
                    EventHandler.CallUpdateTaskProgressEvent(itemName, item.itemAmount);
            }
            foreach (var item in playerBag.taskList)
            {
                if (GetTaskItemDetails(item.itemID).itemName == itemName)
                    EventHandler.CallUpdateTaskProgressEvent(itemName, item.itemAmount);
            }
            foreach (var item in playerBag.otherList)
            {
                if (GetOtherItemDetails(item.itemID).itemName == itemName)
                    EventHandler.CallUpdateTaskProgressEvent(itemName, item.itemAmount);
            }
        }

        #endregion

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


    }


}
