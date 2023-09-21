using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace INTENT.DS.Windows
{
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

        private void AddStyles()
        {
            var styleSheet = EditorGUIUtility.Load("DialogueSystem/DSVariables.uss") as StyleSheet;
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void AddGraphView()
        {
            var graphView = new DSGraphView();
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
    }
}
