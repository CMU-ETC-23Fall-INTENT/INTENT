using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INTENT
{
    using DS;
    // A singleton class that manages the game
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private ConversationPanelControl conversationPanelControl;

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
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            InactiveSystemInit();
        }

        // TODO: hesitant about this design, should we have a singleton DialogueRuntimeManager or make dialogues events?
        // public DialogueRuntimeManager DialogueManager { get; private set; }

        public void StartDialogue(DSDialogue dialogue)
        {
            conversationPanelControl.gameObject.SetActive(true);
            conversationPanelControl.Dialogue = dialogue;
        }
        public void EndDialogue()
        {
            conversationPanelControl.gameObject.SetActive(false);
            conversationPanelControl.Dialogue = null;
            //TODO: record: dialogue is played.
        }



    }
}
