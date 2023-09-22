using System;
using INTENT.DS.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace INTENT.DS.Windows
{
    using Enumerations;
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        private DSNode CreateNode(DSDialogueType dialogueType, UnityEngine.Vector2 localMousePosition)
        {
            Type nodeType = Type.GetType($"INTENT.DS.Elements.DS{dialogueType}Node");
            DSNode node = Activator.CreateInstance(nodeType) as DSNode;

            node.Initialize(localMousePosition);
            node.Draw();
            return node;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger()); // Allows us to drag the graph around
            // this.AddManipulator(new ContentZoomer()); // Allows us to zoom in and out

            this.AddManipulator(new SelectionDragger()); // Allows us to drag multiple nodes
            this.AddManipulator(new RectangleSelector()); // Allows us to select multiple nodes by dragging a box

            this.AddManipulator(CreateNodeContextualMenu("Add Dialogue Node (Single Choice)",DSDialogueType.SingleChoice)); // Allows us to create nodes
            this.AddManipulator(CreateNodeContextualMenu("Add Dialogue Node (Multiple Choices)",DSDialogueType.MultipleChoice)); // Allows us to create nodes
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle,DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType,actionEvent.eventInfo.localMousePosition)));
            });
            return contextualMenuManipulator;
        }

        private void AddStyles()
        {
            var styleSheetGraphView = EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss") as StyleSheet;
            var styleSheetNode = EditorGUIUtility.Load("DialogueSystem/DSNodeStyles.uss") as StyleSheet;
            styleSheets.Add(styleSheetGraphView);
            styleSheets.Add(styleSheetNode);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
    }
}
