using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public enum ConversationPointType
{
    Hidden,
    Available
}

namespace INTENT
{
    [RequireComponent(typeof(SphereCollider))]
    public class ConversationPoint : InteractionPointClass
    {
        [Tooltip("The name of the conversation to start when this point is interacted with.")]
        [SerializeField] private string conversationName;
        [SerializeField] private ConversationPointType conversationPointType;
        [SerializeField] private bool showIndicate = false;
        [SerializeField] private bool canTriggerOnlyOnce = false;
        private bool triggered = false;

        [Tooltip("True if the conversation should start automatically when entered the trigger")]
        [SerializeField] private bool autoTrigger = false;
        [SerializeField] private bool clearTaskOnEnd = false;
        [SerializeField] private TaskPoint autoClearTaskPoint;
        [SerializeField] private bool startTaskOnEnd = false;
        [SerializeField] private TaskPoint autoStartNextTaskPoint;

        [Tooltip("The points that will be made available when this conversation ends")]
        [SerializeField] private ConversationPoint[] makeAvailablePoints;



        protected override void OnValidate()
        {
            base.OnValidate();
            this.name = "ConversationPoint: " + conversationName;
            switch (conversationPointType)
            {
                case ConversationPointType.Hidden:
                    SphereCollider.enabled = false;
                    break;
                case ConversationPointType.Available:
                    SphereCollider.enabled = true;
                    break;
            }
        }
        protected override void Interact()
        {
            if (canTriggerOnlyOnce && triggered)
                return;
            if (IsPlayerInRange)
            {
                base.Interact();
                StartConversation(PlayerCollider);
            }
        }
        public void StartConversation(Collider playerCollider)
        {
            if(playerCollider)
            {
                playerCollider.gameObject.GetComponent<PlayerController>().IsHavingConversation = true;
            }
            DialogueRunner.onDialogueComplete.AddListener(delegate{EndConversation(playerCollider);});

            DialogueRunner.StartDialogue(conversationName);
            IndicatorSphere.SetActive(false);

            if (clearTaskOnEnd)
            {
                DialogueRunner.onDialogueComplete.AddListener(ClearTask);
            }
            if (startTaskOnEnd)
            {
                DialogueRunner.onDialogueComplete.AddListener(StartNextTask);
            }
            triggered = true;
            if (canTriggerOnlyOnce)
            {
                SphereCollider.enabled = false;
                conversationPointType = ConversationPointType.Hidden;
            }

            foreach (ConversationPoint point in makeAvailablePoints)
            {
                DialogueRunner.onDialogueComplete.AddListener(MakeNextAvailable);
            }
        }
        public void MakeAvailable()
        {
            SphereCollider.enabled = true;
            conversationPointType = ConversationPointType.Available;
            if(showIndicate)
                IndicatorSphere.SetActive(true);
        }
        public void MakeNextAvailable()
        {
            foreach (ConversationPoint point in makeAvailablePoints)
            {
                point.MakeAvailable();
            }
            DialogueRunner.onDialogueComplete.RemoveListener(MakeNextAvailable);
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
        public void EndConversation(Collider playerCollider)
        {
            playerCollider.gameObject.GetComponent<PlayerController>().IsHavingConversation = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && canTriggerOnlyOnce && triggered)
                return;
            base.OnTriggerEnter(other);
            if (autoTrigger)
            {
                Interact();
            }
        }
    }
}
