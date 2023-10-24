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
        private SerializedProperty indicatorSphere;

        protected virtual void OnEnable() 
        {
            hintText = serializedObject.FindProperty("HintText");
            dialogueRunner = serializedObject.FindProperty("DialogueRunner");
            sphereCollider = serializedObject.FindProperty("SphereCollider");
            indicatorSphere = serializedObject.FindProperty("IndicatorSphere");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(dialogueRunner);
            EditorGUILayout.PropertyField(hintText);
            EditorGUILayout.PropertyField(sphereCollider);
            EditorGUILayout.PropertyField(indicatorSphere);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
