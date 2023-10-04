using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace INTENT
{
    [RequireComponent(typeof(DialogueRunner))]

    public class YarnDialogueSystemControl : MonoBehaviour
    {
        private DialogueRunner  _dialogueRunner;

        void Awake()
        {
            _dialogueRunner = GetComponent<DialogueRunner>();

            _dialogueRunner.onNodeStart.AddListener(OnNodeStart);
            _dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
            _dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
            _dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
            _dialogueRunner.onCommand.AddListener(OnCommand);
        }

        public void OnNodeStart(string nodeName)
        {
            Debug.Log("YarnDialogueSystemControl:OnNodeStart: " + nodeName);
        }

        public void OnNodeComplete (string nodeName)
        {
            Debug.Log("YarnDialogueSystemControl:OnNodeComplete: " + nodeName);
        }
        public void OnDialogueStart()
        {
            Debug.Log("YarnDialogueSystemControl:OnDialogueStart");
        }
        public void OnDialogueComplete ()
        {
            Debug.Log("YarnDialogueSystemControl:OnDialogueComplete");
        }

        public void OnCommand (string command)
        {
            Debug.Log("YarnDialogueSystemControl:OnCommand: " + command);
        }
    }
}
