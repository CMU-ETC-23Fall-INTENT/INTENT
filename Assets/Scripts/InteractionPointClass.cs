using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

namespace INTENT
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class InteractionPointClass : MonoBehaviour
    {
        #region Components
        [SerializeField] protected DialogueRunner DialogueRunner;
        protected LineView LineView;
        [SerializeField] protected TextMeshPro PressEText;
        #endregion


        protected bool InConvo;
        protected bool IsInRange = false;
        protected bool Interacted;
        protected virtual void OnValidate()
        {
            if(DialogueRunner == null)
                DialogueRunner = FindObjectOfType<DialogueRunner>();
            if(LineView == null)
                LineView = FindObjectOfType<LineView>();
        }
        protected virtual void OnDisable() 
        {
            if(EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
        }
        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("Player") && !Interacted)
            {
                IsInRange = true;
                TextFaceCamera(true);

                EventManager.Instance.PlayerEvents.OnInteractPressed += Interact;
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if(other.CompareTag("Player"))
            {
                IsInRange = false;
                TextFaceCamera(false);

                if(EventManager.Instance != null)
                    EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
            }
        }

        protected virtual void Interact()
        {
            TextFaceCamera(false);
        }

        private void TextFaceCamera(bool active)
        {
            PressEText.gameObject.SetActive(active);
            PressEText.transform.LookAt(Camera.main.transform);
            PressEText.transform.Rotate(0, 180, 0);
        }

    }
}
