using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskItemDetailsList_SO", menuName = "Inventory/TaskItemDetailsList")]
public class TaskItemDetailsList_SO : ScriptableObject
{
    public List<TaskItemDetails> taskItemDetailsList;
}
