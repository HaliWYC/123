using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
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
        EventHandler.CallFindNPCEvent(GetComponent<EnemyController>().NPCID);
        EventHandler.CallBaseBagOpenEvent(SlotType.NPCBag, NPCManager.Instance.GetNPCDetail(GetComponent<EnemyController>().NPCID).NPCBag);
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
        Time.timeScale = 0;
    }

    public void CloseShop()
    {
        isOpen = false;
        EventHandler.CallBaseBagCloseEvent(SlotType.NPCBag, NPCManager.Instance.GetNPCDetail(GetComponent<EnemyController>().NPCID).NPCBag);
        EventHandler.CallUpdateGameStateEvent(GameState.GamePlay);
        Time.timeScale = 1;
    }


}
