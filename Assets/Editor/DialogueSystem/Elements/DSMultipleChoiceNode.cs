using INTENT.DS.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace INTENT.DS.Elements
{
    using Utilities;
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(UnityEngine.Vector2 position)
        {
            base.Initialize(position);
            DialogueType = DSDialogueType.MultipleChoice;
            DialogueName = "NewMultipleChoiceDialogue";

            Choices.Add("New Choice");
        }

        #region Element Creation
        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort("", Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            Button deleteChoiceButton = DSElementUtility.CreateButton("X");
            deleteChoiceButton.AddToClassList("ds-node__button");

            TextField choiceTextField = DSElementUtility.CreateTextField(choice);

            choiceTextField.AddClasses("ds-node__text-field", "ds-node__choice-text-field", "ds-node__text-field__hidden");
            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
        #endregion

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                Choices.Add("New Choice");

                Port choicePort = CreateChoicePort("New Choice");
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1,addChoiceButton);

            /* Output container  */
            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();

        }

    }
}
