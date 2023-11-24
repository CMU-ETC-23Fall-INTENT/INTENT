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
        [SerializeField]
        private int pointID;
        private int preventChangeID;
        private bool isPointIDSet = false;        
        public int PointID
        {
            get { return pointID; }
            private set { pointID = value; }
        }
         #region Components

        [SerializeField] private SphereCollider sphereCollider;

        [SerializeField] private GameObject hintText;

        [SerializeField] private GameObject indicatorSphere;
        #endregion
        

        public bool IsAvailable;
        [SerializeField] private bool forceTeleportOnEnable;

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

    


        private void OnValidate()
        {
            LoadAllInteractions();
            CreateID();
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
        public void InitailizePoint()
        {
            this.gameObject.SetActive(true);
            if(Interactions[currentInteractionIndex].ShowIndicateSphere)
                indicatorSphere.SetActive(true);
            else
                indicatorSphere.SetActive(false);

            if(forceTeleportOnEnable)
            {
                this.transform.position = GameManager.Instance.GetPlayer().transform.position;
            }
            foreach(InteractionBase interaction in Interactions)
            {
                interaction.InitializeInteraction();
            }
            TextFaceCamera(false);
        }



        public void CreateID()
        {
            if(isPointIDSet)
            {
                pointID = preventChangeID;
                return;
            }
            List<UltimateInteractionPoint> allPoints = FindObjectsOfType<UltimateInteractionPoint>(true).ToList();
            foreach(UltimateInteractionPoint point in allPoints)
            {
                if(point == this)
                {
                    allPoints.Remove(point);
                    break;
                }
            }
            int[] allIDs = new int[allPoints.Count];
            for(int i = 0; i < allPoints.Count; i++)
            {
                allIDs[i] = allPoints[i].PointID;
            }
            System.Array.Sort(allIDs);
            for(int i = 0; i < allIDs.Length; i++)
            {
                if(allIDs[i] != i)
                {
                    pointID = i;
                    preventChangeID = i;
                    isPointIDSet = true;
                    return;
                }
            }
            isPointIDSet = true;
            preventChangeID = allIDs.Length;
            pointID = allIDs.Length;
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
            string message = string.Format("Interact: \"{0}\"", this.name);
            LoggingManager.Log("Interaction", message);
            playerCollider.gameObject.GetComponent<PlayerController>().IsInInteraction = true;
            sphereCollider.enabled = false;
            TextFaceCamera(false);
            indicatorSphere.SetActive(false);
            Interactions[currentInteractionIndex].FullPerform();
            EventManager.Instance.PlayerEvents.OnInteractPressed -= Interact;
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
                    Debug.Log("No more interaction");
                    MakeUnavailable();
                    return;
                }
                else
                {
                    Debug.Log("Next interaction");
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

        public void MakeAvailable()
        {
            foreach(TaskScriptableObject taskSO in requiredTasks)
            {
                if(!TaskManager.Instance.IsTaskDone(taskSO.TaskId))
                {
                    Debug.Log("Task " + taskSO.TaskId + " is not done");
                    return;
                }
            }
            TaskManager.Instance.AddAvailableInteractionPoint(this);
            this.InitailizePoint();
            IsAvailable = true;
        }

        public void MakeUnavailable()
        {
            TaskManager.Instance.RemoveAvailableInteractionPoint(this);
            this.gameObject.SetActive(false);
            IsAvailable = false;
        }

        private void TextFaceCamera(bool active)
        {
            hintText.gameObject.SetActive(active);
            hintText.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
