using System;
using INTENT.DS.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace INTENT.DS.Windows
{
    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        private DSNode CreateNode(UnityEngine.Vector2 localMousePosition)
        {
            DSNode node = new DSNode();
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

            this.AddManipulator(CreateNodeContextualMenu()); // Allows us to create nodes
        }

        private IManipulator CreateNodeContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction("Add Node", actionEvent => AddElement(CreateNode(actionEvent.eventInfo.localMousePosition)));
            });
            return contextualMenuManipulator;
        }

        private void AddStyles()
        {
            var styleSheet = EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss") as StyleSheet;
            styleSheets.Add(styleSheet);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
    }
}
