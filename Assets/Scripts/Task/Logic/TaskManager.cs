using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskManager : Singleton<TaskManager>
{
    [System.Serializable]
    public class Task
    {
        public TaskData_SO taskData;
        public bool IsStarted { get { return taskData.isStarted; } set { taskData.isStarted = value; } }
        public bool IsFinished { get { return taskData.isFinished; } set { taskData.isFinished = value; } }
        public bool IsCompleted { get { return taskData.isCompleted; } set { taskData.isCompleted = value; } }
        public bool IsTraced { get { return taskData.isTraced; } set { taskData.isTraced = value; } }
    }


    private void OnEnable()
    {
        EventHandler.UpdateTaskProgressEvent += OnUpdateTaskProgressEvent;
    }

    private void OnDisable()
    {
        EventHandler.UpdateTaskProgressEvent -= OnUpdateTaskProgressEvent;
    }

    private void OnUpdateTaskProgressEvent(string Name, int amount)
    {
        foreach(var task in tasks)
        {
            var matchTasks = task.taskData.taskRequirements.Find(r => r.requireName == Name);
            if (matchTasks != null)
                matchTasks.CurrentAmount += amount;
            task.taskData.CheckTaskProgress();
        }
    }

    public List<Task> tasks = new List<Task>();

    public bool HaveTask(TaskData_SO task)
    {
        if (task != null)
        {
            return tasks.Any(t => t.taskData.ID == task.ID || t.taskData.taskName == task.taskName);
        }
        else
            return false;
    }

    public Task GetTask(TaskData_SO task)
    {
        return tasks.Find(t => t.taskData.ID == task.ID || t.taskData.taskName == task.taskName);
    }


}
