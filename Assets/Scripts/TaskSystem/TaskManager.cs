using System;
using System.Collections;
using System.Collections.Generic;
using INTENT;
using UnityEngine;
using Yarn.Unity;


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
    private List<Task> currentTaskList = new List<Task>();
    private List<Task> doneTaskList = new List<Task>();
    private InteractionBase currentInteraction;
    
    private void Awake() 
    {
        LoadTasks("Tasks/EP1");
        LoadTasks("Tasks/EP2");
    }

    #region OnEnable & OnDisable
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
    #endregion


    #region Event Listeners
    //Make the task available, can be interact with
    private void AvailableTask(string id)
    {
        if(taskDictionary.ContainsKey(id))
        {        
            taskDictionary[id].TaskStatus = TaskStatus.Available;
            ChangeTaskStatus(id, TaskStatus.Available);
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
            currentTaskList.Add(taskDictionary[id]);
            UIManager.Instance.AddToDoTaskList(taskDictionary[id]);
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
            currentTaskList.Remove(taskDictionary[id]);
            doneTaskList.Add(taskDictionary[id]);
            UIManager.Instance.AddDoneTaskList(taskDictionary[id]);
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
    #endregion

    #region Tasks Change from Player Choice

    public void SetCurrentInteraction(InteractionBase interaction)
    {
        currentInteraction = interaction;
    }

    [YarnCommand("RemoveNextUltimatePoint")]
    public void RemoveNextUltimatePoint(int index)
    {
        if (currentInteraction != null)
        {
            currentInteraction.RemovePoint(index);
        }
        else
        {
            Debug.LogError("currentInteraction is null");
        }
    }

    [YarnCommand("RemoveNextTask")]
    public void RemoveNextTask(int index)
    {
        if (currentInteraction != null)
        {
            currentInteraction.RemoveTask(index);
        }
        else
        {
            Debug.LogError("currentInteraction is null");
        }
    }

    #endregion

    #region Load & Get Tasks
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

    //Check if the task is done
    public bool IsTaskDone(string taskID)
    {
        if(taskDictionary.ContainsKey(taskID))
        {
            if(taskDictionary[taskID].TaskStatus == TaskStatus.Completed)
            {
                Debug.Log("Task " + taskID + " is done");
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.LogError("Task ID not found: " + taskID);
            return false;
        }
    }

    //Loading all tasks from Resources folder
    private void LoadTasks(string path)
    {
        TaskScriptableObject[] taskScriptableObjects = Resources.LoadAll<TaskScriptableObject>(path);
        
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
    #endregion
}
