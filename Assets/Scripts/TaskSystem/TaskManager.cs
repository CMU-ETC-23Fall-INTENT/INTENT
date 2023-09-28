using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TaskStatus
{
    Hidden,
    Available,
    Started,
    Completed
}
public class TaskManager : Singleton<TaskManager>
{
    private Dictionary<string, Task> taskDictionary = new Dictionary<string, Task>();
    private void Awake() 
    {
        LoadTasks();
    }
    private void OnEnable() 
    {
        EventManager.Instance.TaskEvents.OnTaskAvailable += AvailableTask;
        EventManager.Instance.TaskEvents.OnTaskStarted += StartTask;
        EventManager.Instance.TaskEvents.OnTaskCompleted += CompleteTask;
    }


    private void OnDisable() 
    {
        if(EventManager.Instance == null)
            return;
        EventManager.Instance.TaskEvents.OnTaskAvailable -= AvailableTask;
        EventManager.Instance.TaskEvents.OnTaskStarted -= StartTask;
        EventManager.Instance.TaskEvents.OnTaskCompleted -= CompleteTask;        
    }


    //Make the task available, can be interact with
    private void AvailableTask(string id)
    {
        if(taskDictionary.ContainsKey(id))
        {        
            taskDictionary[id].TaskStatus = TaskStatus.Available;
            ChangeTaskStatus(id, TaskStatus.Available);
            Debug.Log("Task Available: " + id);
        }
        else
            Debug.LogError("Task ID not found: " + id);
    }

    //Player starts the task
    private void StartTask(string id)
    {
        if(taskDictionary.ContainsKey(id))
        {
            taskDictionary[id].TaskStatus = TaskStatus.Started;
            ChangeTaskStatus(id, TaskStatus.Started);
            Debug.Log("Task Started: " + id);
        }            
        else
            Debug.LogError("Task ID not found: " + id);
    }

    //Player finishes the task
    private void CompleteTask(string id)
    {
        if(taskDictionary.ContainsKey(id))
        {
            taskDictionary[id].TaskStatus = TaskStatus.Completed;
            ChangeTaskStatus(id, TaskStatus.Completed);
            Debug.Log("Task Completed: " + id);
        }
        else
            Debug.LogError("Task ID not found: " + id);
    }

    //Change the task status of the task and also change it on the task point with event
    private void ChangeTaskStatus(string taskId, TaskStatus taskStatus)
    {
        GetTaskbyId(taskId).TaskStatus = taskStatus;
        EventManager.Instance.TaskEvents.TaskStatusChanged(GetTaskbyId(taskId));
    }

    //Get the task by id
    private Task GetTaskbyId(string taskId)
    {
        if(taskDictionary.ContainsKey(taskId))
        {
            return taskDictionary[taskId];
        }
        else
        {
            Debug.LogError("Task ID not found: " + taskId);
            return null;
        }
    }

    //Loading all tasks from Resources folder
    private void LoadTasks()
    {
        TaskScriptableObject[] taskScriptableObjects = Resources.LoadAll<TaskScriptableObject>("Tasks");
        
        foreach (TaskScriptableObject taskSO in taskScriptableObjects)
        {
            if(taskDictionary.ContainsKey(taskSO.TaskId))
            {
                Debug.LogError("Duplicate Task ID: " + taskSO.TaskId);
            }
            else
            {
                taskDictionary.Add(taskSO.TaskId, new Task(taskSO));
            }
        }
    }
}
