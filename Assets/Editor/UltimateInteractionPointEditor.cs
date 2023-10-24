using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace INTENT
{
    [CustomEditor(typeof(UltimateInteractionPoint))]
    public class UltimateInteractionPointEditor : Editor
    {
        private SerializedProperty dialogueRunner;
        private SerializedProperty sphereCollider;
        private SerializedProperty hintText;

        protected virtual void OnEnable() 
        {
            dialogueRunner = serializedObject.FindProperty("dialogueRunner");
            sphereCollider = serializedObject.FindProperty("sphereCollider");
            hintText = serializedObject.FindProperty("hintText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(dialogueRunner);
            EditorGUILayout.PropertyField(sphereCollider);
            EditorGUILayout.PropertyField(hintText);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
