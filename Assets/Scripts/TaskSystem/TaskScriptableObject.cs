using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This is the scriptable object that will be used to store the task data
[CreateAssetMenu(fileName = "TaskSO", menuName = "CreateTask", order = 0)]
public class TaskScriptableObject : ScriptableObject 
{
    [field: SerializeField] public string TaskId { get; private set; }

    [Header("Task Description")]
    public string TaskTitle;

    private void OnValidate() 
    {
        #if UNITY_EDITOR
        TaskId = this.name;
        #endif
    }
}

