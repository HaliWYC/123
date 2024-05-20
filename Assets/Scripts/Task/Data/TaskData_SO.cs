using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName ="NEW Task",menuName ="Task/TaskData")]
public class TaskData_SO : ScriptableObject
{
    
    public int ID;
    public string taskName;
    public TaskType taskType;
    public Sprite taskIcon;
    [TextArea]
    public string description;

    public bool isStarted;
    public bool isFinished;
    public bool isCompleted;
    public bool isTraced;

    public List<TaskRequirements> taskRequirements = new List<TaskRequirements>();
    public List<TaskReward> taskRewards = new List<TaskReward>();

    public void CheckTaskProgress()
    {
        var finishRequire = taskRequirements.Where(r => r.RequiredAmount <= r.CurrentAmount);
        isCompleted = finishRequire.Count() == taskRequirements.Count;
    }

    public List<string> GenerateRequirementsNameList()
    {
        List<string> targetList = new List<string>();
        foreach(var target in taskRequirements)
        {
            targetList.Add(target.targetName);
        }
        return targetList;
    }

}

[System.Serializable]
public class TaskRequirements
{
    public string targetName;
    public TaskRequirementsType targetType;
    public int RequiredAmount;
    public int CurrentAmount;
}

[System.Serializable]
public class TaskReward
{
    public int RewardID;
    public string RewardName;
    public float amount;
    public TaskRewardType type;
    public bool isSecret;
}
