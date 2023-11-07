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
        [SerializeField] protected SphereCollider SphereCollider;
        [FormerlySerializedAs("PressEText")][SerializeField] protected GameObject HintText;
        [SerializeField] protected GameObject IndicatorSphere;
        #endregion

        protected DialogueRunner DialogueRunner;
        protected Collider PlayerCollider = null;
        protected bool IsPlayerInRange => PlayerCollider != null;
        protected bool Interacted;
        protected virtual void OnValidate()
        {
            DialogueRunner = GameManager.Instance.GetDialogueRunner();
        }
        protected virtual void OnDisable()
        {
            if (EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerCollider = other;
                TextFaceCamera(true);

                EventManager.Instance.PlayerEvents.OnInteractPressed += Interact;
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerCollider = null;
                TextFaceCamera(false);

                if (EventManager.Instance != null)
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
