using System;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Dialogue;

public static class EventHandler
{
    #region ConbatEvents
    public static event Action<Transform, int, AttackEffectType> DamageTextPopEvent;
    public static void CallDamageTextPopEvent(Transform targetPos, int damage, AttackEffectType attackEffect)
    {
        DamageTextPopEvent?.Invoke(targetPos, damage, attackEffect);
    }
    #endregion

    #region Inventory/ItemEvents
    public static event Action<ItemType, List<InventoryItem>> UpdatePlayerInventoryUI;
    public static void CallUpdatePlayerInventoryUI(ItemType itemType, List<InventoryItem> list)
    {
        UpdatePlayerInventoryUI?.Invoke(itemType, list);
    }

    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int ID,Vector3 pos)
    {
        InstantiateItemInScene?.Invoke(ID, pos);
    }

    public static event Action<int, Vector3> DropItemEvent;
    public static void CallDropItemEvent(int ID, Vector3 pos)
    {
        DropItemEvent?.Invoke(ID, pos);
    }

    public static event Action<ItemDetails, bool> ItemSelectedEvent;
    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action CheckInvalidItemsEvent;
    public static void CallCheckInvalidItemsevent()
    {
        CheckInvalidItemsEvent?.Invoke();
    }

    #endregion

    #region LightEvents
    public static event Action<Seasons, LightShift, float> LightShiftEvent;
    public static void CallLightShiftEvent(Seasons seasons, LightShift lightShift, float timeDifference)
    {
        LightShiftEvent?.Invoke(seasons, lightShift, timeDifference);
    }
    #endregion

    #region Scene/GameEvents
    public static event Action<string, Vector3> TransitionEvent;

    public static void CallTransitionEvent(string sceneName,Vector3 pos)
    {
        TransitionEvent?.Invoke(sceneName, pos);
    }

    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    public static event Action<GameState> UpdateGameStateEvent;
    public static void CallUpdateGameStateEvent(GameState gameState)
    {
        UpdateGameStateEvent?.Invoke(gameState);
    }

    public static event Action<Vector3, ItemDetails> MouseClickEvent;
    public static void CallMouseClickEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickEvent?.Invoke(pos, itemDetails);
    }

    public static event Action EndGameEvent;
    public static void CallEndGameEvent()
    {
        EndGameEvent?.Invoke();
    }

    #endregion

    #region Dialougue
    public static event Action<int> FindNPCEvent;
    public static void CallFindNPCEvent(int NPCID)
    {
        FindNPCEvent?.Invoke(NPCID);
    }
    public static event Action<DialoguePiece> UpdateDialoguePieceEvent;
    public static void CallUpdateDialoguePieceEvent(DialoguePiece dialoguePiece)
    {
        UpdateDialoguePieceEvent?.Invoke(dialoguePiece);
    }

    public static event Action<DialoguePieceOnlyText> ShowDialoguePieceOnlyTextEvent;
    public static void CallShowDialoguePieceOnlyTextEvent(DialoguePieceOnlyText dialoguePiece)
    {
        ShowDialoguePieceOnlyTextEvent?.Invoke(dialoguePiece);
    }

    public static event Action<DialoguePiece_SO> UpdateDialogueDataEvent;
    public static void CallUpdateDialogueDataEvent(DialoguePiece_SO dialoguePiece_SO)
    {
        UpdateDialogueDataEvent?.Invoke(dialoguePiece_SO);
    }

    

    public static event Action<DialogueOptionType> UpdateDialogueOptionEvent;
    public static void CallUpdateDialogueOptionEvent(DialogueOptionType option)
    {
        UpdateDialogueOptionEvent?.Invoke(option);
    }
    #endregion

    #region NPCFunction
    public static event Action<string, int> UpdateTaskProgressEvent;
    public static void CallUpdateTaskProgressEvent(string target, int amount)
    {
        UpdateTaskProgressEvent?.Invoke(target, amount);
    }
    public static event Action<bool> NPCAvailableEvent;
    public static void CallNPCAvailableEvent(bool isAvailable)
    {
        NPCAvailableEvent?.Invoke(isAvailable);
    }
    public static event Action<TaskData_SO> UpdateTaskDataEvent;
    public static void CallUpdateTaskDataEvent(TaskData_SO taskData)
    {
        UpdateTaskDataEvent?.Invoke(taskData);
    }
    #endregion

    #region TimeEvents
    public static event Action<int, int, int, Seasons> GameMinuteEvent;

    public static void CallGameMinuteEvent(int minute, int hour, int day, Seasons season)
    {
        GameMinuteEvent?.Invoke(minute, hour, day, season);
    }

    public static event Action<int, int, int, int, Seasons> GameDateEvent;

    public static void CallGameDateEvent(int hour, int day, int month, int year, Seasons season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }
    #endregion

    #region UIEvents
    public static event Action UpdateCharacterInformationUIEvent;
    public static void CallUpdateCharacterInformationUIEvent()
    {
        
        UpdateCharacterInformationUIEvent?.Invoke();
    }

    #endregion

    #region BuffEvents
    public static event Action UpdateBuffListEvent;
    public static void CallUpdateBuffListEvent()
    {
        UpdateBuffListEvent?.Invoke();
    }

    #endregion

    #region PlayerEvent
    public static Action<PlayerState> UpdatePlayerStateEvent;
    public static void CallUpdatePlayerStateEvent(PlayerState ps)
    {
        UpdatePlayerStateEvent?.Invoke(ps);
    }
    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;
    public static void CallExecuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
    {
        ExecuteActionAfterAnimation?.Invoke(pos, itemDetails);
    }
    public static event Action<bool> AllowPlayerInputEvent;
    public static void CallAllowPlayerInputEvent(bool input)
    {
        AllowPlayerInputEvent?.Invoke(input);
    }
    #endregion
}
