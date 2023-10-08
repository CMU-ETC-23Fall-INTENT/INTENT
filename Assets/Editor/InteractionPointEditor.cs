using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace INTENT
{
    [CustomEditor(typeof(InteractionPointClass))]
    public class InteractionPointEditor : Editor
    {        
        private SerializedProperty hintText;
        private SerializedProperty dialogueRunner;
        private SerializedProperty sphereCollider;

        protected virtual void OnEnable() 
        {
            hintText = serializedObject.FindProperty("HintText");
            dialogueRunner = serializedObject.FindProperty("DialogueRunner");
            sphereCollider = serializedObject.FindProperty("SphereCollider");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(dialogueRunner);
            EditorGUILayout.PropertyField(hintText);
            EditorGUILayout.PropertyField(sphereCollider);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
