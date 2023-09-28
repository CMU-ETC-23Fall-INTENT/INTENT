using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public TaskScriptableObject TaskSO;
    public TaskStatus TaskStatus;

    public Task(TaskScriptableObject taskSO)
    {
        TaskSO = taskSO;
        TaskStatus = TaskStatus.Hidden;
    }
}

