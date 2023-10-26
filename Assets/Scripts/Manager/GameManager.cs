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
            foreach (KeyValuePair<string, GameObject> entry in NPCManager.Instance.NPC)
            {
                Transform cameraTransform = entry.Value.transform.Find("FocusCamera");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(false);
                }
            }
        }

        public void EnableCharacterUI(string name)
        {
            GameObject gameObject = NPCManager.Instance.GetNPCByName(name);
            if (gameObject != null)
            {
                Transform cameraTransform = gameObject.transform.Find("FocusCamera");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(true);
                }
            }
        }

        public void ToggleBlur(bool toggle)
        {
            canvasBlur?.SetActive(toggle);
        }
    }
}
