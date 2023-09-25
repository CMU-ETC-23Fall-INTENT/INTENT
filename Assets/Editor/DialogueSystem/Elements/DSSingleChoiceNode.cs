using INTENT.DS.Enumerations;
using INTENT.DS.Utilities;
using UnityEditor.Experimental.GraphView;

namespace INTENT.DS.Elements
{
    public class DSSingleChoiceNode : DSNode
    {
        public DSSingleChoiceNode()
        {
        }
        
        public override void Initialize(UnityEngine.Vector2 position)
        {
            base.Initialize(position);
            DialogueType = DSDialogueType.SingleChoice;
            DialogueName = "NewSingleChoiceDialogue";

            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();

            /* Output container  */
            foreach (string choice in Choices)
            {

                Port choicePort = this.CreatePort(choice, Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();

        }

    }
}
