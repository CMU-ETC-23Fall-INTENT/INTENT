using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace INTENT.DS.Elements
{
    using System.Numerics;
    using Enumerations;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Utilities;

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
            /* Title container */
            TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName);

            dialogueNameTextField.AddClasses("ds-node__text-field", "ds-node__filename-text-field", "ds-node__text-field__hidden");
            titleContainer.Insert(0, dialogueNameTextField);

            /* Input container */
            Port inputPort = this.CreatePort("input", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            /* Extension container */

            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = DSElementUtility.CreateFoldout("Dialogue Text");

            TextField dialogueTextField = DSElementUtility.CreateTextArea(DialogueText);
            dialogueTextField.AddClasses("ds-node__text-field", "ds-node__dialogue-text-field");
            textFoldout.Add(dialogueTextField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

        }
    }
}
