using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace INTENT
{
    public enum PointLocation
    {
        Office,
        CoffeeRoom
    }
    [RequireComponent(typeof(SphereCollider))]
    public class UltimateInteractionPoint : MonoBehaviour
    {
         #region Components

        [SerializeField] private SphereCollider sphereCollider;

        [SerializeField] private GameObject hintText;

        [SerializeField] private GameObject indicatorSphere;
        #endregion
        

        [SerializeField] private bool available;
        [SerializeField] private PointLocation pointLocation;
        
        [SerializeField] private GameObject interactionFolder;
        [SerializeField] private List<InteractionBase> Interactions = new List<InteractionBase>();


        #region Status
        private Collider playerCollider = null;
        private bool isPlayerInRange => playerCollider != null;

        private int currentInteractionIndex = 0;

        private bool isFromReinable = false;
        #endregion

    


        private void OnValidate()
        {
            LoadAllInteractions();
            this.name = "First " + Interactions[currentInteractionIndex].name;
        }
        private void OnEnable()
        {
            if(Interactions[currentInteractionIndex].ShowIndicateSphere)
                indicatorSphere.SetActive(true);
            else
                indicatorSphere.SetActive(false);

            TextFaceCamera(false);
        }

        private void OnDisable()
        {
            if (EventManager.Instance == null)
                return;
            EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
        }
        private void Awake()
        {
            LoadAllInteractions();
        }
        private void Start()
        {
            if(!available)
                this.gameObject.SetActive(false);
            
        }


        public void LoadAllInteractions()
        {
            Interactions.Clear();
            foreach (Transform child in interactionFolder.transform)
            {
                Interactions.Add(child.GetComponent<InteractionBase>());
            }
            this.name = "First " + Interactions[currentInteractionIndex].name;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider = other;
                other.gameObject.GetComponent<PlayerController>().CurInteractionPoint = this;
                if(!Interactions[currentInteractionIndex].NeedPressInteract) //auto interact
                {
                    Interact();
                    return;
                }
                if(!isFromReinable)
                    TextFaceCamera(true);

                EventManager.Instance.PlayerEvents.OnInteractPressed += Interact;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider = null;
                other.gameObject.GetComponent<PlayerController>().CurInteractionPoint = null;
                isFromReinable = false;
                TextFaceCamera(false);

                if (EventManager.Instance != null)
                    EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
            }
        }

        private void Interact()
        {
            playerCollider.gameObject.GetComponent<PlayerController>().IsHavingConversation = true;
            sphereCollider.enabled = false;
            TextFaceCamera(false);
            indicatorSphere.SetActive(false);
            Interactions[currentInteractionIndex].FullPerform();
            EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
        }
        public void EndInteraction()
        {
            playerCollider.gameObject.GetComponent<PlayerController>().IsHavingConversation = false;
            sphereCollider.enabled = true;
            isFromReinable = true;
            if(Interactions[currentInteractionIndex].CanPerformOnlyOnce)
            {
                if(!PushIndex())
                {
                    this.gameObject.SetActive(false);
                    return;
                }
                
                if(!Interactions[currentInteractionIndex].NeedPressInteract)
                    Interact();
                
                if(Interactions[currentInteractionIndex].ShowIndicateSphere)
                    indicatorSphere.SetActive(true);
                else
                    indicatorSphere.SetActive(false);

            }
        }

        public bool PushIndex()
        {
            if(currentInteractionIndex < Interactions.Count - 1)
            {
                currentInteractionIndex++;
                return true;
            }                
            else
            {
                Debug.Log("No more interaction");
                return false;
            }
        }

        public void MakeAvailable()
        {
            this.gameObject.SetActive(true);
            available = true;
        }

        public void MakeUnavailable()
        {
            this.gameObject.SetActive(false);
            available = false;
        }

        private void TextFaceCamera(bool active)
        {
            hintText.gameObject.SetActive(active);
            hintText.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
