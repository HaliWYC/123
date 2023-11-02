 using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{


    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]

        public InventoryBag_SO playerBag;


        private void Start()
        {
            EventHandler.callUpdateInventoryUI(InventoryLocation.角色, playerBag.itemList);
        }

        private void OnEnable()
        {
            EventHandler.dropItemEvent += onDropItemEvent;
        }

        

        private void OnDisable()
        {
            EventHandler.dropItemEvent -= onDropItemEvent;
        }
        private void onDropItemEvent(int ID, Vector3 pos)
        {
            removeItem(ID, 1);
        }


        /// <summary>
        /// Use ID to return item
        /// </summary>
        /// <param name="ID">ItemID</param>
        /// <returns></returns>

        public ItemDetails getItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// Add Item to Player's bag
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">Whether destroy object or not </param>
         public void addItem(Item item, bool toDestory)
        {
          //Is Bag spare?

          var index = getItemIndexInBag(item.itemID);

            addItemAtIndex(item.itemID, index, 1);


            //Debug.Log(getItemDetails(item.itemID).itemID + "Name" + getItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }

            //Update UI
            EventHandler.callUpdateInventoryUI(InventoryLocation.角色, playerBag.itemList);
        }

        /// <summary>
        /// Is Bag spare?
        /// </summary>
        /// <returns></returns>
        private bool checkBagCapacity()
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
        private int getItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }

            }
            return -1;
        }

        /// <summary>
        /// Add the item at the certain index
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="index"></param>
        /// <param name="amount"></param>
        private void addItemAtIndex(int ID, int index, int amount)
        {

            if (index == -1 && checkBagCapacity())//Does not have this item 
            {
                var Item = new InventoryItem { itemID = ID, itemAmount = amount };

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
                if (getItemDetails(ID).Stackable)
                {
                    int currentAmount = playerBag.itemList[index].itemAmount + amount;

                    var Item = new InventoryItem { itemID = ID, itemAmount = currentAmount };

                    playerBag.itemList[index] = Item;
                }
                
                else if (!getItemDetails(ID).Stackable && checkBagCapacity())//Not Stackable
                {
                    var Item = new InventoryItem { itemID = ID, itemAmount = amount };

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

        }
        /// <summary>
        /// Swap Item in Player's bag
        /// </summary>
        /// <param name="initialIndex">Initial Index</param>
        /// <param name="targetIndex">Target Index</param>
        public void swapItem(int initialIndex, int targetIndex)
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
            EventHandler.callUpdateInventoryUI(InventoryLocation.角色, playerBag.itemList);
        }

        /// <summary>
        /// Remove certain amount of item in bag
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="removeAmount">Amount</param>
        private void removeItem(int ID, int removeAmount)
        {
            var index = getItemIndexInBag(ID);

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

            EventHandler.callUpdateInventoryUI(InventoryLocation.角色, playerBag.itemList);
        }

    }
}
