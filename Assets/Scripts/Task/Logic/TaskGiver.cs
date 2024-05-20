using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Dialogue;

[RequireComponent(typeof(DialogueController))]
public class TaskGiver : MonoBehaviour
{
    private DialogueController controller;

    private TaskData_SO currentTask;

    public DialoguePiece_SO startDialogue;
    public DialoguePiece_SO processingDialogue;
    public DialoguePiece_SO finishedDialogue;
    public DialoguePiece_SO completedDialogue;

    #region GetTaskState
    public bool IsStarted
    {
        get
        {
            if (TaskManager.Instance.HaveTask(currentTask))
                return TaskManager.Instance.GetTask(currentTask).IsStarted;
            else
                return false;
        }
    }

    public bool IsFinished
    {
        get
        {
            if (TaskManager.Instance.HaveTask(currentTask))
                return TaskManager.Instance.GetTask(currentTask).IsFinished;
            else
                return false;
        }
    }

    public bool IsCompleted
    {
        get
        {
            if (TaskManager.Instance.HaveTask(currentTask))
                return TaskManager.Instance.GetTask(currentTask).IsCompleted;
            else
                return false;
        }
    }

    #endregion

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }

    private void Start()
    {
        controller.currentData = startDialogue;
    }
}
