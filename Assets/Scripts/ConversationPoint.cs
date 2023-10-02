using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace INTENT
{
    using DS;
    [RequireComponent(typeof(DSDialogue))][RequireComponent(typeof(SphereCollider))]
    public class ConversationPoint : MonoBehaviour
    {
        [Header("Conversation Settings")]
        [SerializeField] private DSDialogue dialogue;


        private bool isInRange = false;


        [Header("Components")]
        [SerializeField] private TextMeshPro pressEText;

        private void OnValidate() 
        {
            if(dialogue == null)
            {
                dialogue = GetComponent<DSDialogue>();
            }
            this.name = "ConversationPoint: " + dialogue.dialogue.name;
        }
        private void OnDisable() 
        {
            if(EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= StartConvo;
        }
        private void StartConvo()
        {
            if(isInRange)
            {
                GameManager.Instance.StartDialogue(dialogue);
            }
            
        }
        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("Player"))
            {
                EventManager.Instance.PlayerEvents.OnInteractPressed += StartConvo;
                isInRange = true;
            }
        }
        private void OnTriggerExit(Collider other) 
        {
            if(other.CompareTag("Player"))
            {
                isInRange = false;
                if(EventManager.Instance != null)
                    EventManager.Instance.PlayerEvents.OnInteractPressed -= StartConvo;
            }
        }
    }
}
