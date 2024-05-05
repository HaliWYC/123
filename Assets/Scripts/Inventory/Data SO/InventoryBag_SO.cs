using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="InventoryBag_SO",menuName ="Inventory/InventoryBag")]
public class InventoryBag_SO : ScriptableObject
{
    public int ShanHaiGold;
    public int Gold;
    public List<InventoryItem> equipList;
    public List<InventoryItem> consumeList;
    public List<InventoryItem> taskList;
    public List<InventoryItem> otherList;
}
