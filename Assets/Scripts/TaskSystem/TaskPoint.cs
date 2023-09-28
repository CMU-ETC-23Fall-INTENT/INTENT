using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//This is the interactable points that will be used to start or finish tasks
[RequireComponent(typeof(SphereCollider))]
public class TaskPoint : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private TaskScriptableObject TaskSO;
    [SerializeField] private TaskStatus TaskStatus;

    [Tooltip("True if this is the start point of the task, false if this is the finish point")]
    [SerializeField] private bool isStartPoint = false;

    [Tooltip("True if this is a daily task and should start automatically every day")]
    [SerializeField] private bool isDailyTask = false;

    [Tooltip("The next task point that should be started when this task is completed, leave empty if none")]
    [SerializeField] private TaskPoint autoStartNextTaskPoint;



    private string TaskId;
    private bool isInRange = false;


    [Header("Components")]
    [SerializeField] private TextMeshPro pressEText;

    private void OnValidate() 
    {
        TaskId = TaskSO.TaskId;
        this.name = "TaskPoint: " + TaskId + "_" + (isStartPoint? "S" : "F");
    }

    //If the task is a daily task, start it when the scene starts
    private void Start() 
    {
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
        EventManager.Instance.PlayerEvents.OnInteractPressed -= InteractwithTask;
        EventManager.Instance.TaskEvents.OnTaskStatusChanged -= StatusChange;
    }
    private void InteractwithTask()
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
                if(autoStartNextTaskPoint != null)
                    EventManager.Instance.TaskEvents.TaskStarted(autoStartNextTaskPoint.TaskId);
            }
        }
    }
    
    //When the task change status event is called, this function will be called
    private void StatusChange(Task task)
    {
        if(task.TaskSO.TaskId == TaskId)
        {
            TaskStatus = task.TaskStatus;
        }
    }
    //When the player is in range of the task point, subscribe to the interact event
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            isInRange = true;
            TextFaceCamera(true);
            
            EventManager.Instance.PlayerEvents.OnInteractPressed += InteractwithTask;
        }
    }
    //When the player is out of range of the task point, unsubscribe from the interact event
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {            
            isInRange = false;
            TextFaceCamera(false);

            if(EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= InteractwithTask;
        }
    }

    private void TextFaceCamera(bool active)
    {
        pressEText.gameObject.SetActive(active);
        pressEText.transform.LookAt(Camera.main.transform);
        pressEText.transform.Rotate(0, 180, 0);
    }
}

