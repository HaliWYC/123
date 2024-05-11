using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Inventory;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LootSpawner : MonoBehaviour
{
    public List<LootItem> lootItems;

    public void InitLootItem(int enemyLevel, InventoryBag_SO inventoryBag)
    {


        List<InventoryItem> allItems = new List<InventoryItem>();

        foreach(InventoryItem equip in inventoryBag.equipList)
        {
            allItems.Add(equip);
        }
        foreach (InventoryItem consume in inventoryBag.consumeList)
        {
            allItems.Add(consume);
        }
        foreach (InventoryItem task in inventoryBag.taskList)
        {
            allItems.Add(task);
        }
        foreach (InventoryItem other in inventoryBag.otherList)
        {
            allItems.Add(other);
        }

        foreach(InventoryItem Item in allItems)
        {
            float pro = Random.Range(0f, 1f);

            bool found = false;
            foreach(LootItem loot in lootItems)
            {
                if(loot.itemID == Item.itemID)
                {
                    if (pro >= loot.lootProbility)
                    {
                        for(int num = 0; num < Item.itemAmount; num++)
                        {
                            Vector3 range = new Vector3(Random.Range(transform.position.x, transform.position.x + 1), Random.Range(transform.position.y, transform.position.y + 1), transform.position.z);
                            Item item = Instantiate(ItemManager.Instance.itemPrefab, range, Quaternion.identity);
                            item.Init(Item.itemID, Item.Type);
                            found = true;
                        }
                    }
                }
            }
            if (pro >= CalculateLootItemPro(enemyLevel) && !found)
            {
                
                for (int num = 0; num < Item.itemAmount; num++)
                {
                    Vector3 range = new Vector3(Random.Range(transform.position.x, transform.position.x + 1), Random.Range(transform.position.y, transform.position.y + 1), transform.position.z);
                    Item item = Instantiate(ItemManager.Instance.itemPrefab, range, Quaternion.identity);
                    item.Init(Item.itemID, Item.Type);
                }
            }
        }
    }

    private float CalculateLootItemPro(int level)
    {
        return level * 0.1f;
    }
}

[System.Serializable]
public class LootItem
{
    public int itemID;
    public ItemType itemType;

    [Range(0, 1)]
    public float lootProbility;
}
