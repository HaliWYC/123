﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;

    public string itemName;

    public Sprite itemIcon;

    public Sprite iconOnWorldSprite;

    public ItemType itemType;



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
public class EquipItemDetails : ItemDetails
{
    public EquipItemType equipItemType;
    public ItemQualityType EquipItemQuality;
    public CharacterFightingData_SO EquipData;
    public List<SkillDetails_SO> equipSkills;
}

[System.Serializable]
public class ConsumeItemDetails : ItemDetails
{
    public ConsumeItemType consumeItemType;
    public BasicQualityType ConsumeItemQuality;
    public List<Buff_SO> BuffList;
}

[System.Serializable]
public class TaskItemDetails : ItemDetails
{
    public BasicQualityType TaskItemQuality;
    public TaskItemType taskItemType;
}

[System.Serializable]
public class OtherItemDetails : ItemDetails
{
    public BasicQualityType OtherItemQuality;
    public OtherItemType otherItemType;
}



[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
    public ItemType Type;
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

[System.Serializable]
public class NPCDetails
{
    
    [Header("基本信息")]
    public string NPCName;
    public int NPCID;
    public EnemyLevelType enemyType;
    public EnemyState enemyState;

    [Header("战斗信息")]
    //后期合并到Attack Data之中
    public float sightRadius;
    //public float fightSpeed;
    
    [Header("背包信息")]
    public InventoryBag_SO NPCBag;
}

[System.Serializable]
public class SceneRoute
{
    public string fromSceneName;
    public string goToSceneName;

    public List<ScenePath> scenePathList; 
}


[System.Serializable]
public class ScenePath
{
    public string sceneName;
    public Vector2Int fromGridCell;
    public Vector2Int goToGridCell;
}

