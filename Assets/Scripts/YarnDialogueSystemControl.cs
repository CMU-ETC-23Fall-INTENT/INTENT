using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace INTENT
{
    [RequireComponent(typeof(DialogueRunner))]

    public class YarnDialogueSystemControl : Singleton<YarnDialogueSystemControl>
    {
        private DialogueRunner  _dialogueRunner;

        [Header("DialogueBackground")]
        [SerializeField] private Image normalDialogueBackground;
        [SerializeField] private Image selfThinkingDialogueBackground;

        [Header("SpeakerUI")]
        [SerializeField] private RawImage speakerUILeft;
        [SerializeField] private RawImage speakerUIRight;

        void Awake()
        {
            _dialogueRunner = GetComponent<DialogueRunner>();

            //_dialogueRunner.onNodeStart.AddListener(OnNodeStart);
            //_dialogueRunner.onNodeComplete.AddListener(OnNodeComplete);
            //_dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
            //_dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
            //_dialogueRunner.onCommand.AddListener(OnCommand);
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


        [YarnCommand("ToggleSelfThinking")]
        public static void ToggleSelfThinking(bool bEnable)
        {
            Instance.normalDialogueBackground.gameObject.SetActive(!bEnable);
            Instance.selfThinkingDialogueBackground.gameObject.SetActive(bEnable);
        }
    }
}
