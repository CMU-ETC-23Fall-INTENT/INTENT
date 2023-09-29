using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    using DS;
    // A singleton class that manages the game
    public class GameManager : Singleton<GameManager>
    {

        private ConversationPanelControl conversationPanelControl;


        private void Awake()
        {
            conversationPanelControl = FindObjectOfType<ConversationPanelControl>();
            if(conversationPanelControl == null)
            {
                Debug.LogError("ConversationPanelControl not found in scene");
            }
            conversationPanelControl.gameObject.SetActive(false);
        }




        // TODO: hesitant about this design, should we have a singleton DialogueRuntimeManager or make dialogues events?
        // public DialogueRuntimeManager DialogueManager { get; private set; }

        public void StartDialogue(DSDialogue dialogue)
        {
            conversationPanelControl.gameObject.SetActive(true);
            conversationPanelControl.Dialogue = dialogue;
        }


    }
}
