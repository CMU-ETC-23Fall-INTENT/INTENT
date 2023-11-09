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
        [SerializeField] private List<PerformTask> AfterPerformTasks;
        [SerializeField] private float waitAfterPerformTime = 1f;
        [SerializeField] private bool canActivateUltimatePoints;
        [SerializeField] private List<UltimateInteractionPoint> activateUltimatePoints;

        [SerializeField] private bool canDeactivateUltimatePoints;

        [SerializeField] private List<UltimateInteractionPoint> deactivateUltimatePoints;


        private DialogueRunner dialogueRunner;
        private UltimateInteractionPoint parentPoint;


        private bool didOnce;
        private void Awake() 
        {
            parentPoint = transform.parent.parent.GetComponent<UltimateInteractionPoint>();
            dialogueRunner = GameManager.Instance.GetDialogueRunner();
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

        public void FullPerform()
        {
            BeforePerform();
            Perform();
        }
        private void BeforePerform()
        {
            GameManager.Instance.SetCurrentInteraction(this);
            if(hasBeforeTask && !didOnce)
            {
                foreach (var task in BeforePerformTasks)
                {
                    switch (task.ChangeToStatus)
                    {
                        case TaskStatus.Available:
                            EventManager.Instance.TaskEvents.TaskAvailable(task.Task.TaskId);
                            Debug.Log("Task " + task.Task.TaskId + "Made Available");
                            break;
                        case TaskStatus.Started:
                            EventManager.Instance.TaskEvents.TaskStarted(task.Task.TaskId);
                            Debug.Log("Task " + task.Task.TaskId + "Started");
                            break;
                        case TaskStatus.Completed:
                            EventManager.Instance.TaskEvents.TaskCompleted(task.Task.TaskId);
                            Debug.Log("Task " + task.Task.TaskId + "Completed");
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
                playerAction.GetComponent<PlayerAction>().enabled = true;
                playerAction.GetComponent<PlayerAction>().OnActionFinished += AfterPerform;
            }
            else
            {
                AfterPerform();
            }
        }
        private void AfterPerform()
        {
            GameManager.ToggleBlur(false);
            StartCoroutine(WaitAfterPerform(waitAfterPerformTime));
        }
        IEnumerator WaitAfterPerform(float sec)
        {
            yield return new WaitForSeconds(sec);
            if(hasAfterTask && !didOnce)
            {
                foreach (var task in AfterPerformTasks)
                {
                    if(task.Task == null)
                        continue;
                    switch (task.ChangeToStatus)
                    {
                        case TaskStatus.Available:
                            EventManager.Instance.TaskEvents.TaskAvailable(task.Task.TaskId);
                            Debug.Log("Task " + task.Task.TaskId + "Made Available");
                            break;
                        case TaskStatus.Started:
                            EventManager.Instance.TaskEvents.TaskStarted(task.Task.TaskId);
                            Debug.Log("Task " + task.Task.TaskId + "Started");
                            break;
                        case TaskStatus.Completed:
                            EventManager.Instance.TaskEvents.TaskCompleted(task.Task.TaskId);
                            Debug.Log("Task " + task.Task.TaskId + "Completed");
                            break;
                    }
                }
            }
            if(canActivateUltimatePoints && !didOnce)
            {
                foreach (var point in activateUltimatePoints)
                {
                    if(point != null)
                        point.MakeAvailable();
                }
            }
            if(canDeactivateUltimatePoints && !didOnce)
            {
                foreach (var point in deactivateUltimatePoints)
                {
                    point.MakeUnavailable();
                }
            }
            didOnce = true;
            parentPoint.EndInteraction();
            dialogueRunner.onDialogueComplete.RemoveListener(AfterPerform);
        }

        public void RemovePoint(int index)
        {
            if(activateUltimatePoints.Count > index)
            {
                Debug.Log("Remove Point: " + activateUltimatePoints[index].name);
                activateUltimatePoints.RemoveAt(index);
            }
        }

        public void RemoveTask(int index)
        {
            if(AfterPerformTasks.Count > index)
            {
                Debug.Log("Remove Task: " + AfterPerformTasks[index].Task.TaskId);
                AfterPerformTasks.RemoveAt(index);
            }
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
