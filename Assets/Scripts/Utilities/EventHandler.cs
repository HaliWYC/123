using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    public static event Action<InventoryLocation, List<InventoryItem>> updateInventoryUI;

    public static void callUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        updateInventoryUI?.Invoke(location, list);
    }

    public static event Action<int, Vector3> instantiateItemInScene;
    public static void callInstantiateItemInScene(int ID,Vector3 pos)
    {
        instantiateItemInScene?.Invoke(ID, pos);
    }
}
