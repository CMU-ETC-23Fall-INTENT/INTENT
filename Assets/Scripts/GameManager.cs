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
        [SerializeField] private PlayerInput playerInput;
        private ConversationPanelControl conversationPanelControl;
        private InputActionMap playerMap;
        private InputActionMap uiMap;


        private void Awake()
        {
            conversationPanelControl = FindObjectOfType<ConversationPanelControl>();
            if(conversationPanelControl == null)
            {
                Debug.LogError("ConversationPanelControl not found in scene");
            }
            conversationPanelControl.gameObject.SetActive(false);
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


    }
}
