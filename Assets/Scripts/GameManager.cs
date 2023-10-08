using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INTENT
{
    using DS;
    using System;
    using static UnityEngine.EventSystems.EventTrigger;

    // A singleton class that manages the game
    public class GameManager : Singleton<GameManager>
    {


        [SerializeField] private ConversationPanelControl conversationPanelControl;

        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private SerializableDictionary<string, Camera> mapNameFocusCamera;
        [SerializeField] private SerializableDictionary<string, GameObject> mapNameNPC;
        private InputActionMap playerMap;
        private InputActionMap uiMap;

        private void InactiveSystemInit()
        {
            if (conversationPanelControl == null)
            {
                Debug.LogError("ConversationPanelControl not found in scene");
            }
            else
            {
                conversationPanelControl.gameObject.SetActive(true);
                conversationPanelControl.gameObject.SetActive(false);
            }

        }

        private void Awake()
        {
            //InactiveSystemInit();

            playerMap = playerInput.actions.FindActionMap("Player");
            uiMap = playerInput.actions.FindActionMap("UI");
        }

        private void Start()
        {
            //foreach (KeyValuePair<string, GameObject> entry in mapNameNPC)
            //{


            //    Camera[] focusCameras = gameObject.GetComponentsInChildren<Camera>(true);

            //    //if (focusCameras.Length != 0)
            //    //{
            //    //    mapNameCamera[entry.Key] = focusCameras[0];
            //    //}
            //    //else
            //    //{
            //    //    string errorMsg = String.Format("Name:{0} | GameObject:{1} doesn't have a focusCamera!", entry.Key, entry.Value.name);
            //    //    Debug.LogError(errorMsg);
            //    //}

            //}
        }

        // TODO: hesitant about this design, should we have a singleton DialogueRuntimeManager or make dialogues events?
        // public DialogueRuntimeManager DialogueManager { get; private set; }

        public void StartDialogue(DSDialogue dialogue)
        {
            conversationPanelControl.gameObject.SetActive(true);
            conversationPanelControl.Dialogue = dialogue;

            playerMap.Disable();
            uiMap.Enable();
        }
        public void EndDialogue()
        {
            conversationPanelControl.gameObject.SetActive(false);
            conversationPanelControl.Dialogue = null;
            //TODO: record: dialogue is played.
            playerMap.Enable();
            uiMap.Disable();
        }
        public void PlayerCanMove(bool canMove)
        {
            if (canMove)
            {
                playerMap.Enable();
                uiMap.Disable();
            }
            else
            {
                playerMap.Disable();
                uiMap.Enable();
            }
        }

        public Camera GetFocusCameraOfCharacterByName(string name) {
            if (mapNameFocusCamera.ContainsKey(name))
            {
                return mapNameFocusCamera[name];
            }
            else
            {
                Debug.Log("NPC with FocusCamera with name " + name + " not found in mapNameFocusCamera");
                return null;
            }
        }

        public GameObject GetNPCByName(string name)
        {
            if (mapNameNPC.ContainsKey(name))
                return mapNameNPC[name];
            else
                Debug.Log("NPC with name " + name + " not found in mapNameNPC");
            return null;
        }

        public void DisableAllCharacterFocusCamera()
        {
            Debug.Log("DisableAllCharacterFocusCamera");
            foreach(Camera camera in mapNameFocusCamera.Values)
            {
                camera.gameObject.SetActive(false);
            }
        }

        public void EnableCharacterFocusCamera(string name, bool optional = false)
        {
            Debug.Log("EnableCharacterFocusCamera "+name);
            Camera camera = GetFocusCameraOfCharacterByName(name);
            if(camera != null)
            {
                camera.gameObject.SetActive(true);
            }
            else
            {
                if(optional)
                {
                    Debug.Log("EnableCharacterFocusCamera "+name+ " failed");
                }
                else
                {
                    Debug.LogError("EnableCharacterFocusCamera "+name+" failed");   
                }
            }
        }

        public void DisableAllCharacterUI()
        {
            Debug.Log("DisableAllCharacterUI");
            foreach (KeyValuePair<string, GameObject> entry in mapNameNPC)
            {
                Transform cameraTransform = entry.Value.transform.Find("FocusCamera");
                Transform modelUITransform = entry.Value.transform.Find("ModelUI");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(false);
                    Debug.Log(entry.Key + " Camera Disabled.");
                }
                if (modelUITransform != null)
                {
                    modelUITransform.gameObject.SetActive(false);
                    Debug.Log(entry.Key + " ModelUI Disabled.");
                }
            }
        }

        public void EnableCharacterUI(string name)
        {
            Debug.Log("EnableCharacterUI " + name);
            GameObject gameObject = GetNPCByName(name);
            if (gameObject != null)
            {
                Transform cameraTransform = gameObject.transform.Find("FocusCamera");
                Transform modelUITransform = gameObject.transform.Find("ModelUI");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(true);
                    Debug.Log(name + " Camera Enabled.");
                }
                if (modelUITransform != null)
                {
                    modelUITransform.gameObject.SetActive(true);
                    Debug.Log(name + " ModelUI Enabled.");
                }
            }
            else
            {
                Debug.Log("EnableCharacterFocusCamera " + name + " failed, switch to Default");
                gameObject = GetNPCByName("Default");
                if (gameObject != null)
                {
                    Transform cameraTransform = gameObject.transform.Find("FocusCamera");
                    Transform modelUITransform = gameObject.transform.Find("ModelUI");
                    if (cameraTransform != null)
                    {
                        cameraTransform.gameObject.SetActive(true);
                        Debug.Log(name + " Camera Enabled.");
                    }
                    if (modelUITransform != null)
                    {
                        modelUITransform.gameObject.SetActive(true);
                        Debug.Log(name + " ModelUI Enabled.");
                    }
                }
            }
        }
    }
}
