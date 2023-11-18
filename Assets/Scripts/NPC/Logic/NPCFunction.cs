using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public string Name;    
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
        EventHandler.callFindNPCEvent(Name);
        EventHandler.callBaseBagOpenEvent(SlotType.NPC背包, NPCManager.Instance.getNPCDetail(Name).NPCBag);
        EventHandler.callUpdateGameStateEvent(GameState.Pause);
    }

    public void closeShop()
    {
        isOpen = false;
        EventHandler.callBaseBagCloseEvent(SlotType.NPC背包, NPCManager.Instance.getNPCDetail(Name).NPCBag);
        EventHandler.callUpdateGameStateEvent(GameState.GamePlay);
    }


}
