using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanHai_IsolatedCity.Dialogue;

[RequireComponent(typeof(DialogueController))]
public class TaskGiver : MonoBehaviour
{
    private DialogueController controller;
    private TaskData_SO currentTask;
    private DialoguePiece_SO progressDialogue;
    private DialoguePiece_SO finishedDialogue;
    private DialoguePiece_SO completedDialogue;

    private void OnEnable()
    {
        EventHandler.UpdateTaskDataEvent += OnUpdateTaskDataEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateTaskDataEvent -= OnUpdateTaskDataEvent;
    }

    private void OnUpdateTaskDataEvent(TaskData_SO taskData)
    {
        currentTask = taskData;
        if (currentTask != null)
        {
            if (currentTask.progressDialogue != null)
            {
                progressDialogue = currentTask.progressDialogue;
            }

            if (currentTask.finishedDialogue != null)
            {
                finishedDialogue = currentTask.finishedDialogue;
            }

            if (currentTask.completedDialogue != null)
            {
                completedDialogue = currentTask.completedDialogue;
            }
        }
        UpdateDialogueState();
    }

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

    public void UpdateDialogueState()
    {
        if (IsStarted)
        {
            if (IsFinished)
            {
                controller.currentData = finishedDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }

        if (IsCompleted)
        {
            controller.currentData = completedDialogue;
        }
        EventHandler.CallUpdateDialogueDataEvent(controller.currentData);
        EventHandler.CallUpdateDialogueOptionEvent(DialogueOptionType.Task);
        EventHandler.CallUpdateDialoguePieceEvent(controller.currentData.dialoguePieces[0]);
    }
}
