using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace INTENT
{
    [CustomEditor(typeof(InteractionPointClass))]
    public class InteractionPointEditor : Editor
    {        
        private SerializedProperty pressEText;
        private SerializedProperty dialogueRunner;

        protected virtual void OnEnable() 
        {
            pressEText = serializedObject.FindProperty("PressEText");
            dialogueRunner = serializedObject.FindProperty("DialogueRunner");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(dialogueRunner);
            EditorGUILayout.PropertyField(pressEText);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
