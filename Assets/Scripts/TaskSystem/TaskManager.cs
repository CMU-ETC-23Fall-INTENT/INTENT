using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using INTENT;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public enum TaskStatus
    {
        Hidden,
        Available,
        Started,
        Completed
    }
    public class TaskSaveState
    {
        public TaskStatus TaskStatus;
    }
    public class InteractionSaveState
    {
        public bool IsAvailable;
    }
    public class TaskManager : Singleton<TaskManager>, ISaveable
    {
        [SerializeField] private SerializableDictionary<string, Task> taskDictionary = new SerializableDictionary<string, Task>();
        [SerializeField] private List<GameObject> interactionFolders = new List<GameObject>();
        [SerializeField] private SerializableDictionary<string, UltimateInteractionPoint> allInteractionPoints = new SerializableDictionary<string, UltimateInteractionPoint>();
        private InteractionBase currentInteraction;
        
        private void Awake() 
        {
            LoadTasks("Tasks/EP1");
            LoadTasks("Tasks/EP2");
            LoadInteractionPoints();

        }
        public void LoadInteractionPoints()
        {            
            foreach(GameObject interactionFolder in interactionFolders)
            {
                foreach(UltimateInteractionPoint interactionPoint in interactionFolder.GetComponentsInChildren<UltimateInteractionPoint>())
                {
                    allInteractionPoints.Add(interactionPoint.name, interactionPoint);
                    if(interactionPoint.IsAvailable)
                    {
                        interactionPoint.MakeAvailable();
                    }
                    else
                    {
                        interactionPoint.MakeUnavailable();
                    }
                }
            }
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

        #region Interaction Point
        public void AddAvailableInteractionPoint(UltimateInteractionPoint interactionPoint)
        {
            if(allInteractionPoints.ContainsKey(interactionPoint.name))
            {
                allInteractionPoints[interactionPoint.name].IsAvailable = true;
            }
            else
            {
                Debug.LogError("Interaction Point not found: " + interactionPoint.name);
            }
        }
        public void RemoveAvailableInteractionPoint(UltimateInteractionPoint interactionPoint)
        {
            if(allInteractionPoints.ContainsKey(interactionPoint.name))
            {
                allInteractionPoints[interactionPoint.name].IsAvailable = false;
            }
            else
            {
                Debug.LogError("Interaction Point not found: " + interactionPoint.name);
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

        #region Save & Load

        public string GetIdentifier()
        {
            return "TaskManager";
        }

        public Dictionary<string, string> GetSaveData()
        {
            var res = new Dictionary<string, string>();

            foreach (KeyValuePair<string, Task> entry in taskDictionary)
            {
                TaskSaveState taskSaveState = new TaskSaveState();
                taskSaveState.TaskStatus = entry.Value.TaskStatus;
                res.Add(entry.Key, JsonUtility.ToJson(taskSaveState));
                
            }
            foreach(KeyValuePair<string, UltimateInteractionPoint> entry in allInteractionPoints)
            {
                InteractionSaveState interactionSaveState = new InteractionSaveState();
                interactionSaveState.IsAvailable = entry.Value.IsAvailable; 
                res.Add(entry.Key, JsonUtility.ToJson(interactionSaveState));
            }
            return res;
        }

        public void SetSaveData(Dictionary<string, string> saveData)
        {
            foreach (KeyValuePair<string, string> entry in saveData)
            {
                if (taskDictionary.ContainsKey(entry.Key))
                {
                    TaskSaveState taskSaveState = JsonUtility.FromJson<TaskSaveState>(entry.Value);
                    taskDictionary[entry.Key].TaskStatus = taskSaveState.TaskStatus;
                    switch(taskSaveState.TaskStatus)
                    {
                        case TaskStatus.Started:
                            UIManager.Instance.AddToDoTaskList(taskDictionary[entry.Key]);
                            break;
                        case TaskStatus.Completed:
                            Debug.Log("Task " + taskDictionary[entry.Key].TaskSO.TaskId + " is done");
                            UIManager.Instance.AddDoneTaskList(taskDictionary[entry.Key]);
                            break;
                    }
                }
                else if(allInteractionPoints.ContainsKey(entry.Key))
                {
                    InteractionSaveState interactionSaveState = JsonUtility.FromJson<InteractionSaveState>(entry.Value);
                    switch(interactionSaveState.IsAvailable)
                    {
                        case true:
                            allInteractionPoints[entry.Key].MakeAvailable();
                            break;
                        case false:
                            allInteractionPoints[entry.Key].MakeUnavailable();
                            break;
                    }
                }
            }
        }

        #endregion
    }
}
