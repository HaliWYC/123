using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : Singleton<TaskUI>
{
    [Header("Type")]
    public Button missionButton;
    public Button SideQuestButton;
    public Button DailyTaskButton;
    public Button CompletedTasksButton;

    [Header("Task")]
    public GameObject taskContentBoard;
    public RectTransform TaskListHolder;
    public GameObject TaskNameButtonPrefab;
    public TextMeshProUGUI taskNameInBoard;
    public Image taskIconInBoard;
    public TextMeshProUGUI descriptionInBoard;
    public Button TraceButton;

    [Header("Requirements")]
    public RectTransform RequirementsHolder;
    public GameObject RequirementPrefab;

    [Header("Rewards")]
    public RectTransform RewardsHolder;
    public GameObject RewardPrefab;

    protected override void Awake()
    {
        base.Awake();
        missionButton.onClick.AddListener(GenerateMissionTask);
        SideQuestButton.onClick.AddListener(GenerateSideQuest);
        DailyTaskButton.onClick.AddListener(GenerateDailyTask);
        CompletedTasksButton.onClick.AddListener(GenerateCompletedTask);
    }

    public void SetUpTasksList()
    {
        GenerateMissionTask();
    }

    public void GenerateMissionTask()
    {
        foreach (Transform task in TaskListHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RequirementsHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RewardsHolder)
        {
            Destroy(task.gameObject);
        }

        foreach (var task in TaskManager.Instance.tasks)
        {
            if(task.taskData.taskType == TaskType.Mission && !task.taskData.isCompleted)
            {
                var newTask = Instantiate(TaskNameButtonPrefab, TaskListHolder).GetComponent<TaskNameButton>();
                newTask.SetUpTaskButton(task.taskData);
                newTask.taskNameText = taskNameInBoard;
            }
        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(TaskListHolder);
    }

    public void GenerateSideQuest()
    {
        foreach (Transform task in TaskListHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RequirementsHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RewardsHolder)
        {
            Destroy(task.gameObject);
        }

        foreach (var task in TaskManager.Instance.tasks)
        {
            if (task.taskData.taskType == TaskType.SideQuest && !task.taskData.isCompleted)
            {
                var newTask = Instantiate(TaskNameButtonPrefab, TaskListHolder).GetComponent<TaskNameButton>();
                newTask.SetUpTaskButton(task.taskData);
                newTask.taskNameText = taskNameInBoard;

            }
        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(TaskListHolder);
    }

    public void GenerateDailyTask()
    {
        for(int index = 0; index < TaskListHolder.childCount; index++)
        {
            Destroy(TaskListHolder.GetChild(index).gameObject);
        }
        foreach (Transform task in RequirementsHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RewardsHolder)
        {
            Destroy(task.gameObject);
        }

        foreach (var task in TaskManager.Instance.tasks)
        {
            if (task.taskData.taskType == TaskType.DailyQuest && !task.taskData.isCompleted)
            {
                var newTask = Instantiate(TaskNameButtonPrefab, TaskListHolder).GetComponent<TaskNameButton>();
                newTask.SetUpTaskButton(task.taskData);
                newTask.taskNameText = taskNameInBoard;
            }
        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(TaskListHolder);
    }

    public void GenerateCompletedTask()
    {
        foreach (Transform task in TaskListHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RequirementsHolder)
        {
            Destroy(task.gameObject);
        }
        foreach (Transform task in RewardsHolder)
        {
            Destroy(task.gameObject);
        }

        foreach (var task in TaskManager.Instance.tasks)
        {
            if (task.taskData.isCompleted)
            {
                var newTask = Instantiate(TaskNameButtonPrefab, TaskListHolder).GetComponent<TaskNameButton>();
                newTask.SetUpTaskButton(task.taskData);
                newTask.taskNameText = taskNameInBoard;
            }
        }
        //LayoutRebuilder.ForceRebuildLayoutImmediate(TaskListHolder);
    }

    public void TraceTask()
    {

    }

    public void GenerateTaskRequirementsList(TaskData_SO taskData)
    {
        descriptionInBoard.text = taskData.description;
        foreach (Transform task in RequirementsHolder)
        {
            Destroy(task.gameObject);
        }

        foreach(var Requirement in taskData.taskRequirements)
        {
            TaskRequirementUI taskRequire = Instantiate(RequirementPrefab, RequirementsHolder).GetComponent<TaskRequirementUI>();
            taskRequire.SetUpTaskRequirement(Requirement.targetName, Requirement.RequiredAmount, Requirement.CurrentAmount);
        }
    }

    public void GenerateTaskRewardsList(TaskData_SO taskData)
    {
        foreach (Transform task in RewardsHolder)
        {
            Destroy(task.gameObject);
        }

        foreach (var reward in taskData.taskRewards)
        {
            Text rewardText = Instantiate(RewardPrefab, RewardsHolder).GetComponent<Text>();
            if (reward.isSecret)
                rewardText.text = "A secret reward";
            else
            {
                switch (reward.type)
                {
                    case TaskRewardType.Item:
                        rewardText.text = reward.amount.ToString() + " " + reward.RewardName;
                        break;
                }
            }
        }
    }
}
