using System;
using INTENT.DS.Elements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace INTENT.DS.Windows
{
    using Enumerations;
    using System.Collections.Generic;
    using UnityEngine;
    using Utilities;

    public class DSGraphView : GraphView
    {
        private DSEditorWindow editorWindow;
        private DSSearchWindow searchWindow;

        public DSGraphView(DSEditorWindow dsEditorWindow)
        {
            editorWindow = dsEditorWindow;
            AddManipulators();
            AddGridBackground();

            AddSearchWindow();

            AddStyles();
        }

        #region Overrides
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach((port) =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });
            return compatiblePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger()); // Allows us to drag the graph around
            // this.AddManipulator(new ContentZoomer()); // Allows us to zoom in and out

            this.AddManipulator(new SelectionDragger()); // Allows us to drag multiple nodes
            this.AddManipulator(new RectangleSelector()); // Allows us to select multiple nodes by dragging a box

            this.AddManipulator(CreateNodeContextualMenu("Add Dialogue Node (Single Choice)", DSDialogueType.SingleChoice)); // Allows us to create nodes
            this.AddManipulator(CreateNodeContextualMenu("Add Dialogue Node (Multiple Choices)", DSDialogueType.MultipleChoice)); // Allows us to create nodes

            this.AddManipulator(CreateGroupContextualMenu());
        }
        private IManipulator CreateNodeContextualMenu(string actionTitle,DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType,actionEvent.eventInfo.localMousePosition)));
            });
            return contextualMenuManipulator;
        }


        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", actionEvent.eventInfo.localMousePosition)));
            });
            return contextualMenuManipulator;
        }

        #endregion

        #region Element Creation
        public DSNode CreateNode(DSDialogueType dialogueType, UnityEngine.Vector2 localMousePosition)
        {
            Type nodeType = Type.GetType($"INTENT.DS.Elements.DS{dialogueType}Node");
            DSNode node = Activator.CreateInstance(nodeType) as DSNode;

            node.Initialize(localMousePosition);
            node.Draw();
            return node;
        }


        public Group CreateGroup(string title, UnityEngine.Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title,
            };

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));
            return group;
        }

        #endregion

        #region Element Addition
        private void AddStyles()
        {
            this.AddStyleSheets("DialogueSystem/DSGraphViewStyles.uss","DialogueSystem/DSNodeStyles.uss");
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        #endregion

        #region Utilities
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow) // fix for search window
            {
                worldMousePosition = editorWindow.rootVisualElement.ChangeCoordinatesTo(editorWindow.rootVisualElement.parent, mousePosition - editorWindow.position.position);
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
        #endregion

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
            }

            searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }


    }
}
