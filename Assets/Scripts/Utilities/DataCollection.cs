using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;

    public string itemName;

    public Sprite itemIcon;

    public Sprite iconOnWorldSprite;

    public ItemType itemType;

    public SubType subType;

    [TextArea]
    public string itemDescription;

    public int useRadius;

    public bool canPickUp;

    public int itemPrice;

    [Range(0, 1)]
    public float sellPercentage;

}

[System.Serializable]

public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}