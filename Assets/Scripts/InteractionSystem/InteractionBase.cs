using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    [System.Serializable]
    public struct PerformTask
    {
        public TaskScriptableObject Task;
        public TaskStatus ChangeToStatus;

    }

    [System.Serializable]
    public class InteractionBase: MonoBehaviour
    {
        [SerializeField] private bool isConversation;
        [SerializeField] private string conversationName;
        [SerializeField] private bool hasActionPrefab;
        [SerializeField] private string actionName;
        [SerializeField] private GameObject playerAction;


        public bool NeedPressInteract;
        public bool ShowIndicateSphere;
        public bool CanPerformOnlyOnce;


        [SerializeField] private bool hasBeforeTask;
        [SerializeField] private List<PerformTask> BeforePerformTasks;
        [SerializeField] private bool hasAfterTask;
        [SerializeField] private List<PerformTask> afterPerformTasks;
        private List<PerformTask> dynamicAfterPerformTasks = new List<PerformTask>();
        [SerializeField] private float waitAfterPerformTime = 1f;
        [SerializeField] private bool canActivateUltimatePoints;
        [SerializeField] private List<UltimateInteractionPoint> activateUltimatePoints;
        private List<UltimateInteractionPoint> dynamicActivateUltimatePoints = new List<UltimateInteractionPoint>();

        [SerializeField] private bool canDeactivateUltimatePoints;

        [SerializeField] private List<UltimateInteractionPoint> deactivateUltimatePoints;


        private DialogueRunner dialogueRunner;
        private UltimateInteractionPoint parentPoint;


        private void Awake() 
        {
            parentPoint = transform.parent.parent.GetComponent<UltimateInteractionPoint>();
            dialogueRunner = GameManager.Instance.GetDialogueRunner();
            InitializeInteraction();
        }
        public void InitializeInteraction()
        {
            dynamicAfterPerformTasks = afterPerformTasks.ToList();
            dynamicActivateUltimatePoints = activateUltimatePoints.ToList();
            switch(isConversation)
            {
                case true:
                    this.name = "Conversation: " + conversationName;
                    break;
                case false:
                    if(hasActionPrefab && playerAction != null)
                        this.name = "Action: " + playerAction.name;
                    else
                        this.name = "Action: " + actionName;
                    break;
            }
        }
        private void OnValidate() 
        {
            InitializeInteraction();
        }

        public void FullPerform()
        {
            BeforePerform();
            Perform();
        }
        private void BeforePerform()
        {
            TaskManager.Instance.SetCurrentInteraction(this);
            if(hasBeforeTask)
            {
                foreach (var task in BeforePerformTasks)
                {
                    switch (task.ChangeToStatus)
                    {
                        case TaskStatus.Available:
                            EventManager.Instance.TaskEvents.TaskAvailable(task.Task.TaskId);
                            break;
                        case TaskStatus.Started:
                            EventManager.Instance.TaskEvents.TaskStarted(task.Task.TaskId);
                            break;
                        case TaskStatus.Completed:
                            EventManager.Instance.TaskEvents.TaskCompleted(task.Task.TaskId);
                            break;
                    }
                }
            }
            
        }

        private void Perform()
        {
            if (isConversation)
            {
                GameManager.ToggleBlur(true);
                dialogueRunner.StartDialogue(conversationName);
                dialogueRunner.onDialogueComplete.AddListener(AfterPerform);
            }
            else if(hasActionPrefab && playerAction != null)
            {
                playerAction.GetComponent<PlayerAction>().StartAction();
                playerAction.GetComponent<PlayerAction>().OnActionFinished += AfterPerform;
            }
            else
            {
                AfterPerform();
            }
        }
        private void AfterPerform()
        {
            //Debug.Log("After perform");
            GameManager.ToggleBlur(false);
            StartCoroutine(WaitAfterPerform(waitAfterPerformTime));
        }
        IEnumerator WaitAfterPerform(float sec)
        {
            yield return new WaitForSeconds(sec);
            if(hasAfterTask)
            {
                foreach (var task in dynamicAfterPerformTasks)
                {
                    if(task.Task == null)
                        continue;
                    switch (task.ChangeToStatus)
                    {
                        case TaskStatus.Available:
                            EventManager.Instance.TaskEvents.TaskAvailable(task.Task.TaskId);
                            break;
                        case TaskStatus.Started:
                            EventManager.Instance.TaskEvents.TaskStarted(task.Task.TaskId);
                            break;
                        case TaskStatus.Completed:
                            EventManager.Instance.TaskEvents.TaskCompleted(task.Task.TaskId);
                            break;
                    }
                }
            }
            if(canActivateUltimatePoints)
            {
                foreach (var point in dynamicActivateUltimatePoints)
                {
                    if(point != null)
                        point.ToggleAvailable(true);
                }
            }
            if(canDeactivateUltimatePoints)
            {
                foreach (var point in deactivateUltimatePoints)
                {
                    point.ToggleAvailable(false);
                }
            }
            parentPoint.EndInteraction();
            dialogueRunner.onDialogueComplete.RemoveListener(AfterPerform);
        }

        public void RemovePoint(int pointID)
        {
            foreach(var point in dynamicActivateUltimatePoints)
            {
                if(point.PointID == pointID)
                {
                    dynamicActivateUltimatePoints.Remove(point);
                    //Debug.Log("Point: " + point.name + " removed| ID: " + point.PointID);
                    return;
                }
            }
            Debug.LogError("Point: " + pointID + " not found");
        }

        public void RemoveTask(string taskID)
        {
            foreach(PerformTask task in dynamicAfterPerformTasks)
            {
                if(task.Task.TaskId == taskID)
                {
                    dynamicAfterPerformTasks.Remove(task);
                    return;
                }
            }
            Debug.LogError("Task: " + taskID + " not found");
        }
        public string GetDialogTitle()
        {
            return conversationName;
        }
        public void ChangeDialogTitle(string title)
        {
            conversationName = title;
        }

        #region Gizmos
        //Draws a line from the door to the target door
        void OnDrawGizmos()
        {
            if (activateUltimatePoints.Count > 0)
            {
                Gizmos.color = Color.red;
                foreach(var point in activateUltimatePoints)
                {
                    if(point != null)
                        Gizmos.DrawLine(transform.position, point.transform.position);
                }
            }
        }
        #endregion

    }
}
