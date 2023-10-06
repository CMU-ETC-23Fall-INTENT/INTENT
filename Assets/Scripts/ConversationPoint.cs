using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;
using UnityEditor.EditorTools;

namespace INTENT
{
    [RequireComponent(typeof(SphereCollider))]
    public class ConversationPoint : InteractionPointClass
    {
        [Tooltip("The name of the conversation to start when this point is interacted with.")]
        [SerializeField] private string conversationName;
        [SerializeField] private bool clearTaskOnEnd = false;
        [SerializeField] private TaskPoint autoClearTaskPoint;
        [SerializeField] private bool startTaskOnEnd = false;
        [SerializeField] private TaskPoint autoStartNextTaskPoint;




        protected override void OnValidate() 
        {
            base.OnValidate();
            this.name = "ConversationPoint: " + conversationName;            
        }
        protected override void Interact()
        {
            if(IsInRange)
            {
                base.Interact();
                InConvo = true;
                DialogueRunner.StartDialogue(conversationName);
                if(clearTaskOnEnd)
                {
                    DialogueRunner.onDialogueComplete.AddListener(ClearTask);
                }
                if(startTaskOnEnd)
                {
                    DialogueRunner.onDialogueComplete.AddListener(StartNextTask);
                }
                
                
            }
            
        }
        public void ForceStartConversation()
        {
            InConvo = true;
            DialogueRunner.StartDialogue(conversationName);
        }
        public void StartNextTask()
        {
            EventManager.Instance.TaskEvents.TaskStarted(autoStartNextTaskPoint.TaskId);
            DialogueRunner.onDialogueComplete.RemoveListener(StartNextTask);
        }
        public void ClearTask()
        {
            EventManager.Instance.TaskEvents.TaskCompleted(autoClearTaskPoint.TaskId);
            DialogueRunner.onDialogueComplete.RemoveListener(ClearTask);
        }
    }
}
