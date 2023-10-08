using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;
using UnityEngine.Serialization;

namespace INTENT
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class InteractionPointClass : MonoBehaviour
    {
        #region Components
        [SerializeField] protected DialogueRunner DialogueRunner;
        protected LineView LineView;

        [FormerlySerializedAs("PressEText")][SerializeField] protected GameObject HintText;
        #endregion

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
        protected virtual void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("Player") && !Interacted)
            {
                IsInRange = true;
                TextFaceCamera(true);

                EventManager.Instance.PlayerEvents.OnInteractPressed += Interact;
            }
        }

        protected virtual void OnTriggerExit(Collider other) 
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
            HintText.gameObject.SetActive(active);
            HintText.transform.LookAt(Camera.main.transform);
            HintText.transform.Rotate(0, 180, 0);
        }

    }
}
