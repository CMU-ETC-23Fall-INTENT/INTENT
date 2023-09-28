using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TaskScriptableObject", menuName = "INTENT/TaskScriptableObject", order = 0)]
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

