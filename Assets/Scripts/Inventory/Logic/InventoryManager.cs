 using UnityEngine;

namespace ShanHai_IsolatedCity.Inventory
{


    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]

        public InventoryBag_SO playerBag;

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

            //Is Bag already having this item?

            InventoryItem newItem = new InventoryItem();
            newItem.itemID = item.itemID;
            newItem.itemAmount = 1;

            playerBag.itemList[0] = newItem;

            //Debug.Log(getItemDetails(item.itemID).itemID + "Name" + getItemDetails(item.itemID).itemName);
            if (toDestory)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
