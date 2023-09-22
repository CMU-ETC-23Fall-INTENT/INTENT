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
        }

        public virtual void Draw()
        {
            /* Title Contailer */
            TextField dialogueNameTextField = new TextField()
            {
                value = DialogueName
            };
            titleContainer.Insert(0, dialogueNameTextField);

            /* Input Contailer */
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);

            /* Extension Contailer */

            VisualElement customDataContainer = new VisualElement();

            Foldout textFoldout = new Foldout()
            {
                text = "Dialogue Text"
            };

            TextField dialogueTextField = new TextField()
            {
                value = DialogueText
            };

            textFoldout.Add(dialogueTextField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

        }
    }
}
