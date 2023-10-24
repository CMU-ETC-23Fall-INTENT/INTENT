using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    [RequireComponent(typeof(SphereCollider))]
    public class UltimateInteractionPoint : MonoBehaviour
    {
         #region Components
        [SerializeField] private DialogueRunner dialogueRunner;
        private LineView lineView;
        [SerializeField] private SphereCollider sphereCollider;

        [SerializeField] private GameObject hintText;

        [SerializeField] private GameObject indicatorSphere;
        #endregion


        [Tooltip("True if task triggers before conversation, false if conversation triggers before task")]
        [SerializeField] private bool isTaskFirst = false;

        [Tooltip("True if should automatically trigger when entered")]
        [SerializeField] private bool autoTrigger = false;
        private Collider playerCollider = null;
        private bool isPlayerInRange => playerCollider != null;
        private bool interacted;


        #region Task Properties
        [SerializeField] private bool hasTask = false;
        [SerializeField] private TaskScriptableObject TaskSO;
        [SerializeField] private TaskStatus TaskStatus;

        [Tooltip("True if this is the start point of the task, false if this is the finish point")]
        [SerializeField] private bool isStartPoint = false;

        [Tooltip("True if the next task point should start automatically when this task is completed")]
        [SerializeField] private bool autoStartNextTask = false;
        [SerializeField] private TaskPoint autoStartNextTaskPoint;
        #endregion

        #region Conversation Properties

        [SerializeField] private bool hasConversation = false;

        [Tooltip("The name of the conversation to start when this point is interacted with.")]
        [SerializeField] private string conversationName;
        [SerializeField] private ConversationPointType conversationPointType;
        [SerializeField] private bool canTriggerOnlyOnce = false;
        #endregion


        private void OnValidate()
        {
            if (dialogueRunner == null)
                dialogueRunner = FindObjectOfType<DialogueRunner>();
            if (lineView == null)
                lineView = FindObjectOfType<LineView>();
        }
        private void OnDisable()
        {
            if (EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider = other;
                TextFaceCamera(true);

                EventManager.Instance.PlayerEvents.OnInteractPressed += Interact;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider = null;
                TextFaceCamera(false);

                if (EventManager.Instance != null)
                    EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
            }
        }

        private void Interact()
        {
            TextFaceCamera(false);
        }

        private void TextFaceCamera(bool active)
        {
            hintText.gameObject.SetActive(active);
            hintText.transform.LookAt(Camera.main.transform);
            hintText.transform.Rotate(0, 180, 0);
        }
    }
}