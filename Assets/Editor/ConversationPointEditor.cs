using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

namespace INTENT
{
    [CustomEditor(typeof(ConversationPoint))]
    public class ConversationPointEditor : InteractionPointEditor
    {
        private SerializedProperty conversationName;
        private SerializedProperty clearTaskOnEnd;
        private SerializedProperty autoClearTaskPoint;
        private SerializedProperty startTaskOnEnd;
        private SerializedProperty autoStartNextTaskPoint;
        

        protected override void OnEnable() 
        {
            base.OnEnable();
            conversationName = serializedObject.FindProperty("conversationName");
            clearTaskOnEnd = serializedObject.FindProperty("clearTaskOnEnd");
            autoClearTaskPoint = serializedObject.FindProperty("autoClearTaskPoint");
            startTaskOnEnd = serializedObject.FindProperty("startTaskOnEnd");
            autoStartNextTaskPoint = serializedObject.FindProperty("autoStartNextTaskPoint");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use this for only conversation and if the task start or clear after the conversation", MessageType.Info);
            EditorGUILayout.Space();

            base.OnInspectorGUI();
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Conversation Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(conversationName);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(clearTaskOnEnd);
            if(clearTaskOnEnd.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(autoClearTaskPoint);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                EditorGUILayout.HelpBox("If this is not checked, no task will be cleared when the conversation ends", MessageType.Info);
            }
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(startTaskOnEnd);
            if(startTaskOnEnd.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(autoStartNextTaskPoint);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                EditorGUILayout.HelpBox("If this is not checked, no task will start when the conversation ends", MessageType.Info);
            }

            if(target.GetComponent<TaskPoint>() != null)
            {
                EditorGUILayout.HelpBox("Only use a Conversation Point or a Task Point depending on if task start/clear first or conversation start first", MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
