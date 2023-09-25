using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace INTENT.DS.Windows
{
    using Utilities;
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }
        private void CreateGUI()
        {
            AddGraphView();

            AddStyles();
        }

        #region Elements Addition
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        private void AddGraphView()
        {
            var graphView = new DSGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
        #endregion


    }
}
