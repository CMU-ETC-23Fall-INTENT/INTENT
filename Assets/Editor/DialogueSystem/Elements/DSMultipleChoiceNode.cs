using INTENT.DS.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace INTENT.DS.Elements
{
    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(UnityEngine.Vector2 position)
        {
            base.Initialize(position);
            DialogueType = DSDialogueType.MultipleChoice;
            DialogueName = "NewMultipleChoiceDialogue";

            Choices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };
            addChoiceButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1,addChoiceButton);

            /* Output container  */
            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = "";
                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };
                deleteChoiceButton.AddToClassList("ds-node__button");

                TextField choiceTextField = new TextField()
                {
                    value = choice
                };
                choiceTextField.AddToClassList("ds-node__text-field");
                choiceTextField.AddToClassList("ds-node__choice-text-field");
                choiceTextField.AddToClassList("ds-node__text-field__hidden");
                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();

        }

    }
}
