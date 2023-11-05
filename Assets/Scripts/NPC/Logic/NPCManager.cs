using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    public SceneRouteDataList_SO sceneRouteDate;
    public List<NPCPosition> npcPositionList;

    [Header("交易数据")]
    public List<NPCDetails> NPCDetails;

    private Dictionary<string, SceneRoute> sceneRouteDict = new Dictionary<string, SceneRoute>();

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log(sceneRouteDate.sceneRouteList.Count);
        initSceneRouteDict();

        //foreach(NPCPosition position in npcPositionList)
        //{
        //    position.npc.position = position.position;
        //}
    }


    /// <summary>
    /// Initialise the dictionary
    /// </summary>
    private void initSceneRouteDict()
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


    /// <summary>
    /// Get the path between two scenes
    /// </summary>
    /// <param name="fromSceneName">Initial scene</param>
    /// <param name="goToSceneName">Target scene</param>
    /// <returns></returns>
    public SceneRoute getSceneroute(string fromSceneName,string goToSceneName)
    {
        return sceneRouteDict[fromSceneName + goToSceneName];
    }
}
