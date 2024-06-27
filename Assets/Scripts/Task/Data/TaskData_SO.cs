using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ShanHai_IsolatedCity.Inventory;
using ShanHai_IsolatedCity.Dialogue;

[CreateAssetMenu(fileName ="NEW Task",menuName ="Task/TaskData")]
public class TaskData_SO : ScriptableObject
{
    
    public int ID;
    public string taskName;
    public TaskType taskType;
    public Sprite taskIcon;
    [TextArea]
    public string description;

    public DialoguePiece_SO progressDialogue;
    public DialoguePiece_SO finishedDialogue;
    public DialoguePiece_SO completedDialogue;
    public bool isStarted;
    public bool isFinished;
    public bool isCompleted;
    public bool isTraced;

    public List<TaskRequirements> taskRequirements = new List<TaskRequirements>();
    public List<TaskReward> taskRewards = new List<TaskReward>();

    public void CheckTaskProgress()
    {
        var finishRequire = taskRequirements.Where(r => r.RequiredAmount <= r.CurrentAmount);
        isFinished = finishRequire.Count() == taskRequirements.Count;
    }

    public List<string> GenerateRequirementsNameList()
    {
        List<string> targetList = new List<string>();
        foreach(var target in taskRequirements)
        {
            targetList.Add(target.requireName);
        }
        return targetList;
    }


    public void GiveRewards()
    {
        HandInRequirement();
        foreach(var reward in taskRewards)
        {
            //TODO:Complete other type
            switch (reward.rewardType)
            {
                case TaskRewardType.Item:
                    ItemDetails item = InventoryManager.Instance.GetItemDetails(reward.RewardID, reward.RewardName);
                    InventoryManager.Instance.AddItem(item, (int)reward.amount);
                    break;
            }
        }
        
    }

    private void HandInRequirement()
    {
        foreach(var require in taskRequirements)
        {
            //TODO:Complete other type
            switch (require.requireType)
            {
                case TaskRequirementsType.BringItem:
                    ItemDetails item = InventoryManager.Instance.GetItemDetails(require.requireID, require.requireName);
                    for(int num = 0; num < require.RequiredAmount; num++)
                    {
                        InventoryManager.Instance.RemoveItem(item.itemID, item.itemType, 1);
                    }
                    break;
            }
            
        }
    }

}

[System.Serializable]
public class TaskRequirements
{
    public int requireID;
    public string requireName;
    public TaskRequirementsType requireType;
    public int RequiredAmount;
    public int CurrentAmount;
}

[System.Serializable]
public class TaskReward
{
    public int RewardID;
    public string RewardName;
    public float amount;
    public TaskRewardType rewardType;
    public bool isSecret;
}
