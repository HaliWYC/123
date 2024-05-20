using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskNameButton : MonoBehaviour
{
    
    public GameObject Trace;
    public TextMeshProUGUI taskNameText;
    public TaskData_SO currentData;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateTaskContent);
    }

    private void UpdateTaskContent()
    {
        TaskUI.Instance.taskContentBoard.SetActive(true);
        if(!currentData.isCompleted)
            TaskUI.Instance.TraceButton.gameObject.SetActive(true);
        if (currentData!=null)
        {
            taskNameText.text = currentData.taskName;
            if (currentData.taskIcon != null)
                TaskUI.Instance.taskIconInBoard.sprite = currentData.taskIcon;
            else
                TaskUI.Instance.taskIconInBoard.gameObject.SetActive(false);
            TaskUI.Instance.GenerateTaskRequirementsList(currentData);
            TaskUI.Instance.GenerateTaskRewardsList(currentData);
        }
    }

    public void SetUpTaskButton(TaskData_SO taskData)
    {
        currentData = taskData;
        taskNameText.text = currentData.taskName;
    }

    private void OnDisable()
    {
        TaskUI.Instance.taskContentBoard.SetActive(false);
        TaskUI.Instance.TraceButton.gameObject.SetActive(false);
    }
}
