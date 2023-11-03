using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;

    public string itemName;

    public Sprite itemIcon;

    public Sprite iconOnWorldSprite;

    public ItemType itemType;

    public WeaponType weaponType;

    [TextArea]
    public string itemDescription;

    public int useRadius;

    public bool canPickUp;

    public bool canDrop;

    public bool Stackable;

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

[System.Serializable]
public class SerilazableVector3
{
    public float x, y, z;

    public SerilazableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerilazableVector3 position;
}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public GridType gridType;
    public bool boolTypeValue;
}

[System.Serializable]
public class TileDetails
{
    public int gridX, gridY;
    public bool meleeOnly;
    public bool rangedOnly;
    public bool canDropItem;
    public bool isNPCObstacle;

    //TODO:Can add some property of the single tile
}

[System.Serializable]
public class NPCPosition
{
    public Transform npc;
    public string startScene;
    public Vector3 position;
}