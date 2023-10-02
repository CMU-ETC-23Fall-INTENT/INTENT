using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace INTENT
{
    using DS;
    // A singleton class that manages the game
    public class GameManager : Singleton<GameManager>
    {


        [SerializeField] private ConversationPanelControl conversationPanelControl;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerInput playerInput;
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
            InactiveSystemInit();
            
            playerMap = playerInput.actions.FindActionMap("Player");
            uiMap = playerInput.actions.FindActionMap("UI");
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
    }
}
