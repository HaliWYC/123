using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public SceneRouteDataList_SO sceneRouteDate;
    public List<NPCPosition> npcPositionList;

    [Header("数据")]
    public List<NPCList_SO> NPCList;

    private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log(sceneRouteDate.sceneRouteList.Count);
        InitSceneRouteDict();
        foreach(NPCPosition position in npcPositionList)
        {
            position.npc.position = position.position;
        }
    }


    /// <summary>
    /// Initialise the dictionary
    /// </summary>
    private void InitSceneRouteDict()
    {
        
        if (sceneRouteDate.sceneRouteList.Count > 0)
        {
            foreach(SceneRoute route in sceneRouteDate.sceneRouteList)
            {
                var key = route.fromSceneName + route.goToSceneName;

                if (sceneRouteDict.ContainsKey(key))
                    continue;
                else
                    sceneRouteDict.Add(key, route);
            }
        }
    }

    public NPCDetails GetNPCDetail(string NPCName)
    {
        foreach(NPCList_SO NPCList_SO in NPCList)
        {
            return NPCList_SO.NPCDetailsList.Find(i => i.NPCName == NPCName);
        }
        return null;
    }

    /// <summary>
    /// Get the path between two scenes
    /// </summary>
    /// <param name="fromSceneName">Initial scene</param>
    /// <param name="goToSceneName">Target scene</param>
    /// <returns></returns>
    public SceneRoute GetSceneroute(string fromSceneName,string goToSceneName)
    {
        return sceneRouteDict[fromSceneName + goToSceneName];
    }

    
}
