using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace INTENT
{
    using System;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using Yarn.Unity;
    using static UnityEngine.EventSystems.EventTrigger;

    // A singleton class that manages the game
    public class GameManager : Singleton<GameManager>
    {
        [Header("Player")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private string playerName;
        [SerializeField] private GameObject player;
        [SerializeField] private CinemachineVirtualCamera followCamera;
        [SerializeField] private ElevatorController elevatorController;

        public string PlayerName
        {
            get => playerName;
            set
            {
                playerName = value;
                dialogueRunner?.VariableStorage?.SetValue("$playerName", playerName); //Update the name in the dialogue system
                string result = "";
                bool? @bool = dialogueRunner?.VariableStorage?.TryGetValue("$playerName", out result);
                if (@bool.HasValue && @bool.Value)
                    Debug.Log("Player name set to " + result);
                else
                    Debug.LogError("Player name not set");
            }
        }

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

        private bool isBlurFromYarnCommand = false;
        private bool isBlurFromUI = false;
        public bool IsBlurred
        {
            get => isBlurFromYarnCommand || isBlurFromUI;
        }

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
        public GameObject GetPlayer()
        {
            return player;
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

        [YarnCommand("ToggleBlur")]
        public static void ToggleBlur(bool toggle)
        {
            Instance.isBlurFromYarnCommand = toggle;
            Instance.canvasBlur?.SetActive(Instance.IsBlurred);
        }

        public static void ToggleBlurFromUI(bool toggle)
        {
            Instance.isBlurFromUI = toggle;
            Instance.canvasBlur?.SetActive(Instance.IsBlurred);
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


        

        [YarnCommand("CameraFocusOnNPC")]
        public void CameraFocusOnNPC(string npcName, bool toggle)
        {
            Debug.Log("In");
            if(toggle)
            {
                followCamera.Follow = NPCManager.Instance.GetNPCByName(npcName).transform;
                Debug.Log("CameraFocusOnNPC " + npcName);
            }
            else
            {
                followCamera.Follow = player.transform;
                Debug.Log("Camera Return to player");
            }
        }


        public void GameStart()
        {
            elevatorController.gameObject.SetActive(true);
            elevatorController.GameStart();
        }

        public void ResetGameState()
        {
            dialogueRunner.Stop();
        }

        public void ReloadGame()
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
    }
}
