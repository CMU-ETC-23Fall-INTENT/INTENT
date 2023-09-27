using System.Collections;
using System.Collections.Generic;
using DS;
using TMPro;
using UnityEngine;

namespace INTENT
{
    public class ConversationPanelControl : MonoBehaviour
    {
        [SerializeField] public DSDialogue Dialogue;
        private TextMeshProUGUI textMeshProUGUI;
        // Start is called before the first frame update
        void Start()
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            if(textMeshProUGUI == null)
            {
                Debug.LogError("No TextMeshProUGUI found in children of " + gameObject.name);
                return;
            }
            if(Dialogue == null)
            {
                Debug.LogError("No Dialogue bind in " + gameObject.name);
                return;
            }
            textMeshProUGUI.text = Dialogue.dialogue.Text;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnClick()
        {
            Debug.Log("OnClick");
            Debug.Log("Dialogue.Choices.Count: " + Dialogue.dialogue.Choices.Count);
            if(Dialogue.dialogue.Choices.Count == 0)
            {
                //TODO: end conversation
                return;
            }
            if(Dialogue.dialogue.Choices.Count == 1)
            {
                Dialogue.dialogue = Dialogue.dialogue.Choices[0].NextDialogue;
                textMeshProUGUI.text = Dialogue.dialogue.Text;
                return;
            }
            //TODO: multiple choices

        }
    }
}
