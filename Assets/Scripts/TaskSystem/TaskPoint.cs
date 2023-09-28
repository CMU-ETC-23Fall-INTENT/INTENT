using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TaskPoint : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private TaskScriptableObject TaskSO;
    [SerializeField] private TaskStatus TaskStatus;

    [Header("Task Start or Finish Point")]
    [Tooltip("True if this is the start point of the task, false if this is the finish point")]
    [SerializeField] private bool isStartPoint = false;
    [Header("Daily Task")]
    [Tooltip("True if this is a daily task and should start automatically every day")]
    [SerializeField] private bool isDailyTask = false;



    private string TaskId;
    private bool isInRange = false;

    private void Awake() 
    {
        TaskId = TaskSO.TaskId;
    }
    private void Start() {
        if(isDailyTask)
        {
            EventManager.Instance.TaskEvents.TaskStarted(TaskId);
        }
    }
    private void OnEnable() 
    {
        EventManager.Instance.TaskEvents.OnTaskStatusChanged += StatusChange;
    }
    private void OnDisable() 
    {
        if(EventManager.Instance == null)
            return;
        EventManager.Instance.PlayerEvents.OnInteractPressed -= GetTask;
        EventManager.Instance.TaskEvents.OnTaskStatusChanged -= StatusChange;
    }
    private void GetTask()
    {
        if(isInRange)
        {
            if(TaskStatus == TaskStatus.Hidden)
                return;
            if(isStartPoint && TaskStatus == TaskStatus.Available)
            {
                EventManager.Instance.TaskEvents.TaskStarted(TaskId);
            }
            else if(!isStartPoint && TaskStatus == TaskStatus.Started)
            {
                EventManager.Instance.TaskEvents.TaskCompleted(TaskId);
            }
        }
    }
    private void StatusChange(Task task)
    {
        if(task.TaskSO.TaskId == TaskId)
        {
            TaskStatus = task.TaskStatus;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            EventManager.Instance.PlayerEvents.OnInteractPressed += GetTask;
            isInRange = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= GetTask;
            isInRange = false;
        }
    }
}

