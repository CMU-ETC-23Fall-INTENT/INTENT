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
        [SerializeField] public DSDialogue Dialogue;
        [SerializeField] public SerializableDictionary<int, Button> Buttons;

        private TextMeshProUGUI textMeshProUGUI;

        // Start is called before the first frame update
        void Start()
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
            if(Dialogue == null)
            {
                Debug.LogError("No Dialogue bind in " + gameObject.name);
                return;
            }
            textMeshProUGUI.text = Dialogue.dialogue.Text;

            Debug.Log("Buttons: " + Buttons.Count);
            foreach (var button in Buttons)
            {
                button.Value.onClick.AddListener(delegate { OnChoice(button.Key); });
                button.Value.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnContinue() //TODO: spacebar
        {
            bool changed = false;
            Debug.Log("Dialogue.Choices.Count: " + Dialogue.dialogue.Choices.Count);
            if(Dialogue.dialogue.Choices.Count == 0)
            {
                //TODO: end conversation
            }
            if(Dialogue.dialogue.Choices.Count == 1) //Next dialogue
            {
                Dialogue.dialogue = Dialogue.dialogue.Choices[0].NextDialogue;
                changed = true;
            }
            else // Dialogue.dialogue.Choices.Count > 1 multiple choices
            {
                //don't do anything to the text panel
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
            textMeshProUGUI.text = Dialogue.dialogue.Text;
        }

        private void UpdateChoices()
        {
            if (Dialogue.dialogue.DialogueType == DSDialogueType.MultipleChoice)
            {
                //Show Buttons with updated text
                for (int i = 1; i <= Dialogue.dialogue.Choices.Count; i++)
                {
                    Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = Dialogue.dialogue.Choices[i-1].Text;
                    Buttons[i].gameObject.SetActive(true);
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
