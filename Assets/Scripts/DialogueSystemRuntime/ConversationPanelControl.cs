using System.Collections;
using System.Collections.Generic;
using DS;
using DS.Enumerations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace INTENT
{
    public class ConversationPanelControl : MonoBehaviour
    {
        public DSDialogue Dialogue
        {
            get { return _Dialogue; }
            set
            {
                _Dialogue = value;
                UpdatePanel();
            }
        }
        private DSDialogue _Dialogue;

        [SerializeField] public SerializableDictionary<int, Button> Buttons;

        private TextMeshProUGUI textMeshProUGUI;

        // Start is called before the first frame update
        void Awake()
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if (textMeshProUGUI == null)
            {
                Debug.LogError("No TextMeshProUGUI found in children of " + gameObject.name);
                return;
            }
            if(Buttons == null)
            {
                Debug.LogError("No Buttons found in children of " + gameObject.name);
                return;
            }

            Debug.Log("Buttons: " + Buttons.Count);
            foreach (var button in Buttons)
            {
                button.Value.onClick.AddListener(delegate { OnChoice(button.Key); });
                button.Value.gameObject.SetActive(false);
            }

            UpdatePanel();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnContinue() //TODO: spacebar
        {
            bool changed = false;
            Debug.Log("Dialogue.Choices.Count: " + Dialogue.dialogue.Choices.Count);
            if (Dialogue.dialogue.DialogueType == DSDialogueType.MultipleChoice)
            {
                //don't do anything to the text panel
            }
            else
            {
                if (Dialogue.dialogue.Choices.Count == 0)
                {
                    //TODO: end conversation
                }
                if (Dialogue.dialogue.Choices.Count == 1) //Next dialogue
                {
                    Dialogue.dialogue = Dialogue.dialogue.Choices[0].NextDialogue;
                    changed = true;
                }
            }

            if(changed)
            {
                UpdatePanel();
            }
        }
        public void OnChoice(int idx)
        {
            Debug.Log("OnChoice " + idx);
            Dialogue.dialogue = Dialogue.dialogue.Choices[idx-1].NextDialogue;
            UpdatePanel();
        }

        public void UpdatePanel()
        {
            UpdateText();
            UpdateChoices();
        }
        private void UpdateText()
        {
            if (Dialogue == null || Dialogue.dialogue == null) { textMeshProUGUI.text = ""; return; }
            textMeshProUGUI.text = Dialogue.dialogue.Text;
        }

        private void UpdateChoices()
        {
            if (Dialogue == null) return;
            if (Dialogue.dialogue.DialogueType == DSDialogueType.MultipleChoice)
            {
                //Show Buttons with updated text
                for (int i = 1; i <= Dialogue.dialogue.Choices.Count; i++)
                {
                    Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = Dialogue.dialogue.Choices[i-1].Text;
                    Buttons[i].gameObject.SetActive(true);
                }
                foreach (var button in Buttons)
                {
                    if (button.Key > Dialogue.dialogue.Choices.Count)
                        button.Value.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var button in Buttons)
                {
                    button.Value.gameObject.SetActive(false);
                }
                //hide buttons
            }
        }
    }
}
