using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        [SerializeField] private int pointID;
        private int preventChangeID;
        private bool isPointIDSet = false;        
        public int PointID
        {
            get { return pointID; }
            private set 
            { 
                pointID = value;
            }
        }

         #region Components

        [SerializeField] private SphereCollider sphereCollider;

        [SerializeField] private GameObject hintText;

        [SerializeField] public GameObject IndicatorSphere;
        #endregion
        
        public bool IsAvailable;
        [SerializeField] private bool forceTeleportOnEnable;
        [SerializeField] private float delayBeforeEnable = 0f;

        [SerializeField] private List<TaskScriptableObject> requiredTasks = new List<TaskScriptableObject>();
        
        [SerializeField] private GameObject interactionFolder;
        [SerializeField] private List<InteractionBase> Interactions = new List<InteractionBase>();


        #region Status
        private Collider playerCollider = null;
        private bool isPlayerInRange => playerCollider != null;

        private int currentInteractionIndex = 0;

        private bool isFromReinable = false;

        private int index = 0;
        #endregion

        
        public bool CheckIDDuplicate()
        {
            UltimateInteractionPoint[] allPoints = FindObjectsOfType<UltimateInteractionPoint>();
            foreach(UltimateInteractionPoint point in allPoints)
            {
                if(point.PointID == this.PointID && point != this)
                {
                    string error = this.name + " has Duplicate ID: " + this.PointID + " with " + point.gameObject.name;
                    Debug.LogError(error, this.gameObject);
                    return false;
                }
            }
            return true;
        }

        private void OnValidate()
        {
            CheckIDDuplicate();
            LoadAllInteractions();
        }
        public InteractionBase GetCurrentInteraction()
        {
            return Interactions[currentInteractionIndex];
        }
        private void OnDisable()
        {
            if (EventManager.Instance == null)
                return;
        }
        private void Awake()
        {
            LoadAllInteractions();
            IndicatorSphere.SetActive(false);
            TextFaceCamera(false);
        }
        private void InitailizePoint()
        {
            this.gameObject.SetActive(true);
            
            foreach(InteractionBase interaction in Interactions)
            {
                interaction.InitializeInteraction();
            }
            TextFaceCamera(false);

            StartCoroutine(DelayBeforeShow(delayBeforeEnable));
        }
        IEnumerator DelayBeforeShow(float sec)
        {
            yield return new WaitForSeconds(sec);

            if(Interactions[currentInteractionIndex].ShowIndicateSphere)
                IndicatorSphere.SetActive(true);
            else
                IndicatorSphere.SetActive(false);

            if(forceTeleportOnEnable)
            {
                this.transform.position = GameManager.Instance.GetPlayer().transform.position;
            }
        }


        

        public void LoadAllInteractions()
        {
            Interactions.Clear();
            foreach (Transform child in interactionFolder.transform)
            {
                Interactions.Add(child.GetComponent<InteractionBase>());
            }
            index = 0;
            string newName = "First " + Interactions[0].name;
            while(GameObject.Find(newName) != null && GameObject.Find(newName) != this.gameObject)
            {
                index++;
                newName = "First " + Interactions[0].name + " " + index;
            }
            
            this.name = newName;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider = other;
                other.gameObject.GetComponent<PlayerController>().CurrInteractionPoint = this;
                if(!Interactions[currentInteractionIndex].NeedPressInteract) //auto interact
                {
                    Interact();
                    return;
                }
                if(!isFromReinable)
                    TextFaceCamera(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollider = null;
                other.gameObject.GetComponent<PlayerController>().CurrInteractionPoint = null;
                isFromReinable = false;
                TextFaceCamera(false);

            }
        }

        public void Interact()
        {
            string message = string.Format("Interact: \"{0}\"", this.name);
            LoggingManager.Log("Interaction", message);
            playerCollider.gameObject.GetComponent<PlayerController>().IsInInteraction = true;
            playerCollider.gameObject.GetComponent<PlayerController>().CurrInteractionPoint = null;
            sphereCollider.enabled = false;
            TextFaceCamera(false);
            IndicatorSphere.SetActive(false);
            Interactions[currentInteractionIndex].FullPerform();
        }
        public void EndInteraction()
        {
            playerCollider.gameObject.GetComponent<PlayerController>().IsInInteraction = false;
            sphereCollider.enabled = true;
            isFromReinable = true;
            if(Interactions[currentInteractionIndex].CanPerformOnlyOnce)
            {
                if(!PushIndex())
                {
                    //Debug.Log("No more interaction");
                    ToggleAvailable(false);
                    return;
                }
                else
                {
                    //Debug.Log("Next interaction");
                }
                
                if(!Interactions[currentInteractionIndex].NeedPressInteract)
                    Interact();
                
                InitailizePoint();

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
                return false;
            }
        }
        public void ChangeCurrentIndex(int i)
        {
            if(i < Interactions.Count)
            {
                currentInteractionIndex = i;
                InitailizePoint();
            }                
            else
                Debug.LogError("Index out of range");
        }
        public int GetCurrentIndex()
        {
            return currentInteractionIndex;
        }
        public void ToggleAvailable(bool toggle, bool isNPC = false)
        {
            switch(toggle)
            {
                case true:
                    foreach(TaskScriptableObject taskSO in requiredTasks)
                    {
                        if(!TaskManager.Instance.IsTaskDone(taskSO.TaskId))
                        {
                            //Debug.Log("Task " + taskSO.TaskId + " is not done");
                            return;
                        }
                    }

                    if(!isNPC)
                        TaskManager.Instance.AddAvailableInteractionPoint(this);

                    this.InitailizePoint();
                    IsAvailable = true;
                    break;
                case false:
                    if(!isNPC)
                        TaskManager.Instance.RemoveAvailableInteractionPoint(this);
                        
                    this.gameObject.SetActive(false);
                    IsAvailable = false;
                    break;
            }
        }

        private void TextFaceCamera(bool active)
        {
            hintText.gameObject.SetActive(active);
        }
    }
}
