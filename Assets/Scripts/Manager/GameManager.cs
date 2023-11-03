using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INTENT
{
    using System;
    using Yarn.Unity;
    using static UnityEngine.EventSystems.EventTrigger;

    // A singleton class that manages the game
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerController playerController;

        [SerializeField] private SerializableDictionary<string, Camera> mapNameFocusCamera;
        [SerializeField] private SerializableDictionary<string, Texture> mapNameTexture;
        [SerializeField] private GameObject canvasBlur;
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private List<GameObject> GameObjectsToEnableWhenGameStarts;

        [Header("Yarn Spinner Dialogue System")]
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private CustomLineView customLineView;

        private float defaultTypewriterEffectSpeed;
        private InputActionMap playerMap;
        private InputActionMap uiMap;

        private void Awake()
        {
            playerMap = playerInput.actions.FindActionMap("Player");
            uiMap = playerInput.actions.FindActionMap("UI");
            defaultTypewriterEffectSpeed = customLineView.typewriterEffectSpeed;

            foreach (GameObject gameObject in GameObjectsToEnableWhenGameStarts)
            {
                gameObject.SetActive(true);
            }
        }

        private void Start()
        {
        }

        public DialogueRunner GetDialogueRunner()
        {
            return dialogueRunner;
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
        public void PlayerEnterAction()
        {
            playerController.IsInAction = true;
        }
        public void PlayerExitAction()
        {
            playerController.IsInAction = false;
        }
        public void ToggleIsPlayerHavingTutorial(bool bEnable)
        {
            playerController.IsInTutorial = bEnable;
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
            bool bSuccess = false;
            if (gameObject != null)
            {
                Transform cameraTransform = gameObject.transform.Find("FocusCamera");
                if (cameraTransform != null)
                {
                    cameraTransform.gameObject.SetActive(true);
                    bSuccess = true;
                }
            }

            if (!bSuccess)
            {
                Debug.Log("EnableCharacterUI " + name + " failed, clean the render texture");
                RenderTexture rt = UnityEngine.RenderTexture.active;
                UnityEngine.RenderTexture.active = renderTexture;
                GL.Clear(true, true, Color.clear);
                UnityEngine.RenderTexture.active = rt;
            }
        }

        public void ToggleBlur(bool toggle)
        {
            canvasBlur?.SetActive(toggle);
        }

        [YarnCommand("SetTypeWritterEffectSpeed")]
        public void SetTypeWritterEffectSpeed(float speed)
        {
            customLineView.typewriterEffectSpeed = speed;
        }
        [YarnCommand("ResetTypeWritterEffectSpeed")]
        public void ResetTypeWritterEffectSpeed()
        {
            customLineView.typewriterEffectSpeed = defaultTypewriterEffectSpeed;
        }
    }
}
