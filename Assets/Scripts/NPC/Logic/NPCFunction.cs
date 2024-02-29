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
            CloseShop();
        }
    }

    public void OpenShop()
    {
        isOpen = true;
        EventHandler.CallFindNPCEvent(Name);
        EventHandler.CallBaseBagOpenEvent(SlotType.NPC背包, NPCManager.Instance.GetNPCDetail(Name).NPCBag);
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }

    public void CloseShop()
    {
        isOpen = false;
        EventHandler.CallBaseBagCloseEvent(SlotType.NPC背包, NPCManager.Instance.GetNPCDetail(Name).NPCBag);
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
    }


}
