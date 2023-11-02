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

    public static event Action<int, Vector3> dropItemEvent;
    public static void callDropItemEvent(int ID, Vector3 pos)
    {
        dropItemEvent?.Invoke(ID, pos);
    }

    public static event Action<int, int> gameMinuteEvent;

    public static void callGameMinuteEvent(int minute,int hour)
    {
        gameMinuteEvent?.Invoke(minute, hour);
    }

    public static event Action<int, int, int, int, Seasons> gameDateEvent;

    public static void callGameDateEvent(int hour,int day,int month,int year,Seasons season)
    {
        gameDateEvent?.Invoke(hour, day, month, year, season);
    }

    public static event Action<string, Vector3> transitionEvent;

    public static void callTransitionEvent(string sceneName,Vector3 pos)
    {
        transitionEvent?.Invoke(sceneName, pos);
    }

    public static event Action beforeSceneUnloadEvent;
    public static void callBeforeSceneUnloadEvent()
    {
        beforeSceneUnloadEvent?.Invoke();
    }

    public static event Action afterSceneLoadedEvent;
    public static void callAfterSceneLoadedEvent()
    {
        afterSceneLoadedEvent?.Invoke();
    }

    public static event Action<Vector3> moveToPosition;
    public static void callMoveToPosition(Vector3 targetPosition)
    {
        moveToPosition?.Invoke(targetPosition);
    }

    public static event Action<ItemDetails, bool> itemSelectedEvent;
    public static void callItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        itemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<Vector3, ItemDetails> mouseClickEvent;
    public static void callMouseClickEvent(Vector3 pos, ItemDetails itemDetails)
    {
        mouseClickEvent?.Invoke(pos, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> executeActionAfterAnimation;
    public static void callExecuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
    {
        executeActionAfterAnimation?.Invoke(pos, itemDetails);
    }
}
