using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using INTENT;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    public enum Episode
    {
        Episode1,
        Episode2
    }
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
        public int CurrentInteractionIndex;
        public bool IsAvailable;
    }
    public class EpisodeSaveState
    {
        public Episode CurrentEpisodeIndex;
    }
    public class ActionSaveState
    {
        public int CurrentActionState;
    }
    public class TaskManager : Singleton<TaskManager>, ISaveable
    {
        private Dictionary<string, Task> taskDictionary = new Dictionary<string, Task>();
        [SerializeField] private Episode currentEpisodeIndex;
        [SerializeField] private List<GameObject> interactionFolders = new List<GameObject>();
        [SerializeField] private List<GameObject> actionFolders = new List<GameObject>();
        [Header("Episode 1")]
        [SerializeField] private GameObject ep1Folder;
        [Header("Episode 2")]
        [SerializeField] private GameObject ep2Folder;

        private Dictionary<int, UltimateInteractionPoint> allInteractionPoints = new Dictionary<int, UltimateInteractionPoint>();
        [SerializeField] private SerializableDictionary<string, PlayerAction> allActions = new SerializableDictionary<string, PlayerAction>();
        private InteractionBase currentInteraction;
        
        private void Awake() 
        {
            LoadTasks("Tasks/EP1");
            LoadTasks("Tasks/EP2");
            LoadInteractionPoints();
            LoadActions();

        }
        private void Start() 
        {
            ActivateEpisode(currentEpisodeIndex);            
        }
        public void ActivateEpisode(Episode episode, bool isFromSave = false)
        {
            switch(episode)
            {
                case Episode.Episode1:
                    ep1Folder.SetActive(true);
                    ep2Folder.SetActive(false);
                    currentEpisodeIndex = Episode.Episode1;
                    SoundManager2D.Instance.StopBGM();
                    if(!isFromSave)
                    {
                        NPCManager.Instance.TeleportToLocation("Player", "Hallway", 0);
                        NPCManager.Instance.TeleportToLocation("Ali", "CoffeeRoom", 0);
                        NPCManager.Instance.TeleportToLocation("Tony", "CoffeeRoom", 1);
                        NPCManager.Instance.TeleportToLocation("Ming", "PlayerOffice", 3);
                        NPCManager.Instance.TeleportToLocation("Ash", "WaitRoom", 0);
                    }                    
                    break;
                case Episode.Episode2:
                    ep1Folder.SetActive(false);
                    ep2Folder.SetActive(true);
                    currentEpisodeIndex = Episode.Episode2;
                    SoundManager2D.Instance.StopBGM();
                    if(!isFromSave)
                    {
                        NPCManager.Instance.TeleportToLocation("Player", "Hallway", 1);
                        NPCManager.Instance.TeleportToLocation("Ali", "WaitRoom", 1);
                        NPCManager.Instance.TeleportToLocation("Tony", "PlayerOffice", 5);
                        NPCManager.Instance.TeleportToLocation("Ming", "PlayerOffice", 3);
                        NPCManager.Instance.TeleportToLocation("Ash", "PlayerOffice", 4);
                    }
                    break;
            }
        }
        public void LoadInteractionPoints()
        {     
            foreach(GameObject folder in interactionFolders)
            {
                foreach(UltimateInteractionPoint interactionPoint in folder.GetComponentsInChildren<UltimateInteractionPoint>())
                {
                    allInteractionPoints.Add(interactionPoint.PointID, interactionPoint);
                    if(interactionPoint.IsAvailable)
                    {
                        interactionPoint.ToggleAvailable(true);
                    }
                    else
                    {
                        interactionPoint.ToggleAvailable(false);;
                    }
                }
            }
        }
        public void LoadActions()
        {
            foreach(GameObject folder in actionFolders)
            {
                foreach(PlayerAction action in folder.GetComponentsInChildren<PlayerAction>())
                {
                    allActions.Add(action.name, action);
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
        public void RemoveNextUltimatePoint(int pointID)
        {
            if (currentInteraction != null)
            {
                currentInteraction.RemovePoint(pointID);
            }
            else
            {
                Debug.LogError("currentInteraction is null");
            }
        }

        [YarnCommand("RemoveNextTask")]
        public void RemoveNextTask(string taskID)
        {
            if (currentInteraction != null)
            {
                currentInteraction.RemoveTask(taskID);
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
            if(allInteractionPoints.ContainsKey(interactionPoint.PointID))
            {
                allInteractionPoints[interactionPoint.PointID].IsAvailable = true;
            }
            else
            {
                Debug.LogError("Interaction Point not found: " + interactionPoint.PointID);
            }
        }
        public void RemoveAvailableInteractionPoint(UltimateInteractionPoint interactionPoint)
        {
            if(allInteractionPoints.ContainsKey(interactionPoint.PointID))
            {
                allInteractionPoints[interactionPoint.PointID].IsAvailable = false;
            }
            else
            {
                Debug.LogError("Interaction Point not found: " + interactionPoint.PointID);
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

        public class TaskManagerSaveData:ISaveData
        {
            public Dictionary<string, TaskSaveState> TaskSaveStates = new Dictionary<string, TaskSaveState>();
            public Dictionary<int, InteractionSaveState> InteractionSaveStates = new Dictionary<int, InteractionSaveState>();
            public Dictionary<string, ActionSaveState> ActionSaveStates = new Dictionary<string, ActionSaveState>();
            public EpisodeSaveState EpiSaveState = new EpisodeSaveState();
        }
        public ISaveData GetSaveData()
        {
            TaskManagerSaveData taskManagerSaveData = new TaskManagerSaveData();
            foreach (KeyValuePair<string, Task> entry in taskDictionary)
            {
                TaskSaveState taskSaveState = new TaskSaveState();
                taskSaveState.TaskStatus = entry.Value.TaskStatus;
                taskManagerSaveData.TaskSaveStates.Add(entry.Key, taskSaveState);
            }
            foreach (KeyValuePair<int, UltimateInteractionPoint> entry in allInteractionPoints)
            {
                InteractionSaveState interactionSaveState = new InteractionSaveState();
                interactionSaveState.CurrentInteractionIndex = entry.Value.GetCurrentIndex();
                interactionSaveState.IsAvailable = entry.Value.IsAvailable;
                taskManagerSaveData.InteractionSaveStates.Add(entry.Value.PointID, interactionSaveState);
            }
            foreach (KeyValuePair<string, PlayerAction> entry in allActions)
            {
                ActionSaveState actionSaveState = new ActionSaveState();
                actionSaveState.CurrentActionState = entry.Value.ActionState;
                taskManagerSaveData.ActionSaveStates.Add(entry.Key, actionSaveState);
            }
            taskManagerSaveData.EpiSaveState.CurrentEpisodeIndex = currentEpisodeIndex;
            return taskManagerSaveData;
        }

        public void SetSaveData(ISaveData saveData)
        {
            TaskManagerSaveData taskManagerSaveData = saveData as TaskManagerSaveData;
            UIManager.Instance.ClearAllTaskList();
            foreach (KeyValuePair<string, TaskSaveState> entry in taskManagerSaveData.TaskSaveStates)
            {
                if (taskDictionary.ContainsKey(entry.Key))
                {
                    taskDictionary[entry.Key].TaskStatus = entry.Value.TaskStatus;
                    switch (entry.Value.TaskStatus)
                    {
                        case TaskStatus.Started:
                            StartCoroutine(UIManager.Instance.DelayedAdd(0, taskDictionary[entry.Key]));
                            break;
                        case TaskStatus.Completed:
                            StartCoroutine(UIManager.Instance.DelayedAdd(1, taskDictionary[entry.Key]));
                            break;
                    }
                }
                else
                {
                    Debug.LogError("Task ID not found: " + entry.Key);
                }
            }
            foreach (KeyValuePair<int, InteractionSaveState> entry in taskManagerSaveData.InteractionSaveStates)
            {
                if (allInteractionPoints.ContainsKey(entry.Key))
                {
                    allInteractionPoints[entry.Key].ChangeCurrentIndex(entry.Value.CurrentInteractionIndex);
                    switch (entry.Value.IsAvailable)
                    {
                        case true:
                            allInteractionPoints[entry.Key].ToggleAvailable(true);
                            break;
                        case false:
                            allInteractionPoints[entry.Key].ToggleAvailable(false);;
                            break;
                    }
                }
                else
                {
                    Debug.LogError("Interaction Point not found: " + entry.Key);
                }
            }
            foreach (KeyValuePair<string, ActionSaveState> entry in taskManagerSaveData.ActionSaveStates)
            {
                if (allActions.ContainsKey(entry.Key))
                {
                    allActions[entry.Key].ActionState = entry.Value.CurrentActionState;
                    allActions[entry.Key].ResetAction(entry.Value.CurrentActionState);
                }
                else
                {
                    Debug.LogError("Action not found: " + entry.Key);
                }
            }
            currentEpisodeIndex = taskManagerSaveData.EpiSaveState.CurrentEpisodeIndex;
            ActivateEpisode(currentEpisodeIndex, true);
        }

        #endregion
    }
}
