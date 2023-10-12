using System.Collections;
using System.Collections.Generic;
using INTENT;
using TMPro;
using UnityEngine;
using Yarn.Unity;


//This is the interactable points that will be used to start or finish tasks
[RequireComponent(typeof(SphereCollider))]
public class TaskPoint : InteractionPointClass
{
    #region Components
    [SerializeField] private GameObject indicatorSphere;
    #endregion


    #region Task Properties
    [SerializeField] private TaskScriptableObject TaskSO;
    [SerializeField] private TaskStatus TaskStatus;

    [Tooltip("True if this is the start point of the task, false if this is the finish point")]
    [SerializeField] private bool isStartPoint = false;


    
    [Tooltip("True if this is a daily task, which means it will start when the scene starts")]
    [SerializeField] private bool isDailyTask = false;



    [Tooltip("True if the next task point should start automatically when this task is completed")]
    [SerializeField] private bool autoStartNextTask = false;
    [SerializeField] private TaskPoint autoStartNextTaskPoint;


    [Tooltip("True if the conversation should start automatically when the interacted")]
    [SerializeField] private bool autoStartConversation = false;
    [SerializeField] private ConversationPoint autoStartConversationPoint;
    #endregion

    

    public string TaskId;



    protected override void OnValidate() 
    {
        base.OnValidate();
        if(TaskSO == null)
            return;
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
        switch(TaskStatus)
        {
            case TaskStatus.Hidden:
                indicatorSphere.SetActive(false);
                break;
            case TaskStatus.Available:
                indicatorSphere.SetActive(false);
                break;
            case TaskStatus.Started:
                indicatorSphere.SetActive(true);
                break;
            case TaskStatus.Completed:
                indicatorSphere.SetActive(false);
                break;
        }
    }
    private void OnEnable() 
    {
        if(TaskStatus == TaskStatus.Hidden)
            this.GetComponent<SphereCollider>().enabled = false;
        EventManager.Instance.TaskEvents.OnTaskStatusChanged += StatusChange;
    }
    protected override void OnDisable() 
    {
        base.OnDisable();
        
        if(EventManager.Instance != null)
            EventManager.Instance.TaskEvents.OnTaskStatusChanged -= StatusChange;
    }

    //When the player presses the interact button, check if the player is in range and if the task is available
    protected override void Interact()
    {
        if(IsPlayerInRange)
        {
            Debug.Log("This is a " + isStartPoint);
            if(TaskStatus == TaskStatus.Hidden || Interacted)
                return;
            base.Interact();
            //If this is the start point, start the task, if this is the finish point, complete the task
            if(isStartPoint && TaskStatus == TaskStatus.Available)
            {
                Interacted = true;
                EventManager.Instance.TaskEvents.TaskStarted(TaskId);
            }
            else if(!isStartPoint && TaskStatus == TaskStatus.Started)
            {
                Interacted = true;
                EventManager.Instance.TaskEvents.TaskCompleted(TaskId);
                //If there is a next task point, start it
                if(autoStartNextTask && autoStartNextTaskPoint != null)
                    EventManager.Instance.TaskEvents.TaskStarted(autoStartNextTaskPoint.TaskId);
            }

            if(autoStartConversation)
            {
                autoStartConversationPoint.StartConversation();
            }
        }
    }
    
    //When the task change status event is called, this function will be called
    private void StatusChange(Task task)
    {
        if(task.TaskSO.TaskId == TaskId)
        {
            TaskStatus = task.TaskStatus;
            if(TaskStatus == TaskStatus.Available || TaskStatus == TaskStatus.Started)
            {
                Interacted = false;
                indicatorSphere.SetActive(true);
                this.GetComponent<SphereCollider>().enabled = true;
            }
            else if(TaskStatus == TaskStatus.Completed)
            {
                this.gameObject.SetActive(false);
            }
        }
        
    }
    //When the player is in range of the task point, subscribe to the interact event
    

    void OnDrawGizmosSelected()
    {        
        if(autoStartNextTaskPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, autoStartNextTaskPoint.transform.position);
        }
    }
}

