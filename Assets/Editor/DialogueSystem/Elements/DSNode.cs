using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace INTENT.DS.Elements
{
    using System.Numerics;
    using Enumerations;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class DSNode : Node
    {
        // public string GUID;
        public string DialogueName { get; set; }
        public string DialogueText { get; set; }
        // public bool EntryPoint = false; 
        public List<string> Choices { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(UnityEngine.Vector2 position)
        {
            DialogueName = "NewDialogue";
            Choices = new List<string>();
            DialogueText = "Hello world!";

            SetPosition(new Rect(position, UnityEngine.Vector2.zero));

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* Title Contailer */
            TextField dialogueNameTextField = new TextField()
            {
                value = DialogueName
            };
            dialogueNameTextField.AddToClassList("ds-node__text-field");
            dialogueNameTextField.AddToClassList("ds-node__filename-text-field");
            dialogueNameTextField.AddToClassList("ds-node__text-field__hidden");
            titleContainer.Insert(0, dialogueNameTextField);

            /* Input Contailer */
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);

            /* Extension Contailer */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text"
            };

            TextField dialogueTextField = new TextField()
            {
                value = DialogueText
            };
            dialogueTextField.AddToClassList("ds-node__text-field");
            dialogueTextField.AddToClassList("ds-node__quote-text-field");
            textFoldout.Add(dialogueTextField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

        }
    }
}
