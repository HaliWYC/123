using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBag_SO bagData;
    private bool isOpen;

    private void Update()
    {
        if(isOpen&&Input.GetKeyDown(KeyCode.Escape))
        {
            closeShop();
        }
    }

    public void openShop()
    {
        isOpen = true;
        EventHandler.callBaseBagOpenEvent(SlotType.NPC背包, bagData);
        EventHandler.callUpdateGameStateEvent(GameState.Pause);
    }

    public void closeShop()
    {
        isOpen = false;
        EventHandler.callBaseBagCloseEvent(SlotType.NPC背包, bagData);
        EventHandler.callUpdateGameStateEvent(GameState.GamePlay);
    }
}
