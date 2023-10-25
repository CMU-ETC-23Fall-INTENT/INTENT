using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INTENT
{
    using System;
    using static UnityEngine.EventSystems.EventTrigger;

    // A singleton class that manages the game
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private SerializableDictionary<string, Camera> mapNameFocusCamera;
        [SerializeField] private SerializableDictionary<string, Texture> mapNameTexture;
        [SerializeField] private GameObject canvasBlur;
        private InputActionMap playerMap;
        private InputActionMap uiMap;

        private void Awake()
        {
            playerMap = playerInput.actions.FindActionMap("Player");
            uiMap = playerInput.actions.FindActionMap("UI");
        }

        private void Start()
        {
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

        public Texture GetAvatarTextureByName(string name)
        {
            if (mapNameTexture.ContainsKey(name))
                return mapNameTexture[name];
            else
                Debug.Log("NPC with name " + name + " not found in mapNameTexture");
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
            //Debug.Log("EnableCharacterFocusCamera "+name);
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
            foreach (KeyValuePair<string, GameObject> entry in NPCManager.Instance.NPC)
            {
                Transform cameraTransform = entry.Value.transform.Find("FocusCamera");
                //Transform modelUITransform = entry.Value.transform.Find("ModelUI");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(false);
                    //Debug.Log(entry.Key + " Camera Disabled.");
                }
                //if (modelUITransform != null)
                //{
                //    modelUITransform.gameObject.SetActive(false);
                //    Debug.Log(entry.Key + " ModelUI Disabled.");
                //}
            }
        }

        public void EnableCharacterUI(string name)
        {
            Debug.Log("EnableCharacterUI " + name);
            GameObject gameObject = NPCManager.Instance.GetNPCByName(name);
            if (gameObject == null)
            {
                //Debug.Log("EnableCharacterFocusCamera " + name + " failed");
                //gameObject = GetNPCByName("Default");
                //TODO: maybe not, instead disable the Character UI and warn (in debug mode)
            }
            if (gameObject != null)
            {
                Transform cameraTransform = gameObject.transform.Find("FocusCamera");
                //Transform modelUITransform = gameObject.transform.Find("ModelUI");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(true);
                    //Debug.Log(name + " Camera Enabled.");
                }
                //if (modelUITransform != null)
                //{
                //    modelUITransform.gameObject.SetActive(true);
                //    Debug.Log(name + " ModelUI Enabled.");
                //}
            }
        }

        public void ToggleBlur(bool toggle)
        {
            canvasBlur?.SetActive(toggle);
        }
    }
}
