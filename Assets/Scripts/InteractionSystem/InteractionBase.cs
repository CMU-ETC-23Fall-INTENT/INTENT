using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
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
        [SerializeField] private string actionName;


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
        }
        private void OnValidate() 
        {
            if(dialogueRunner == null)
                dialogueRunner = FindObjectOfType<DialogueRunner>();
            switch(isConversation)
            {
                case true:
                    this.name = "Conversation: " + conversationName;
                    break;
                case false:
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
                GameManager.Instance.ToggleBlur(true);
                dialogueRunner.StartDialogue(conversationName);
                dialogueRunner.onDialogueComplete.AddListener(AfterPerform);
            }
            else
            {
                AfterPerform();
            }
        }
        private void AfterPerform()
        {
            GameManager.Instance.ToggleBlur(false);
            StartCoroutine(WaitAfterPerform(waitAfterPerformTime));
        }
        IEnumerator WaitAfterPerform(float sec)
        {
            yield return new WaitForSeconds(sec);
            if(hasAfterTask && !didOnce)
            {
                foreach (var task in AfterPerformTasks)
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
            if(canActivateUltimatePoints && !didOnce)
            {
                foreach (var point in activateUltimatePoints)
                {
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
