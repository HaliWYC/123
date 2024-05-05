using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NEW SceneSoundList",menuName ="Sound/SceneSoundList")]
public class SceneSoundList_SO : ScriptableObject
{
    public List<SceneSoundDetails> sceneSoundDetailsList;

    public SceneSoundDetails GetSceneSoundDetails(string name)
    {
        return sceneSoundDetailsList.Find(s => s.SceneName == name);
    }
    
}
[System.Serializable]
public class SceneSoundDetails
{
    [SceneName]public string SceneName;

    public string gameSound;
    public string ambientSound;
    
}
