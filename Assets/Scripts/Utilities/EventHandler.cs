using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Dialogue;

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

    public static event Action<int, int,int,Seasons> gameMinuteEvent;

    public static void callGameMinuteEvent(int minute,int hour,int day,Seasons season)
    {
        gameMinuteEvent?.Invoke(minute, hour,day,season);
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

    public static event Action<DialoguePiece> showDialogueEvent;
    public static void callShowDialogueEvent(DialoguePiece dialoguePiece)
    {
        showDialogueEvent?.Invoke(dialoguePiece);
    }

    //Open the shop
    public static event Action<SlotType, InventoryBag_SO> baseBagOpenEvent;
    public static void callBaseBagOpenEvent(SlotType slotType,InventoryBag_SO bag_SO)
    {
        baseBagOpenEvent?.Invoke(slotType, bag_SO);
    }

    public static event Action<SlotType, InventoryBag_SO> baseBagCloseEvent;
    public static void callBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bag_SO)
    {
        baseBagCloseEvent?.Invoke(slotType, bag_SO);
    }

    public static event Action<GameState> updateGameStateEvent;
    public static void callUpdateGameStateEvent(GameState gameState)
    {
        updateGameStateEvent?.Invoke(gameState);
    }

    public static event Action<ItemDetails, bool> showTradeUI;
    public static void callShowTradeUI(ItemDetails item,bool isSell)
    {
        showTradeUI?.Invoke(item,isSell);
    }

    public static event Action<string> FindNPCEvent;
    public static void callFindNPCEvent(string NPCName)
    {
        FindNPCEvent?.Invoke(NPCName);
    }

    public static event Action<Seasons, LightShift, float> lightShiftEvent;
    public static void callLightShiftEvent(Seasons seasons,LightShift lightShift,float timeDifference)
    {
        lightShiftEvent?.Invoke(seasons, lightShift, timeDifference);
    }
}
