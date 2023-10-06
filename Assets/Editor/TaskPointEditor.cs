using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Yarn.Unity;
using Unity.VisualScripting;

namespace INTENT
{
    [CustomEditor(typeof(TaskPoint))]
    public class TaskPointEditor : InteractionPointEditor
    {        
        private SerializedProperty pressEText;
        private SerializedProperty dialogueRunner;
        private SerializedProperty TaskSO;
        private SerializedProperty TaskStatus;
        private SerializedProperty isStartPoint;
        private SerializedProperty isDailyTask;
        private SerializedProperty autoStartNext;
        private SerializedProperty autoStartNextTaskPoint;
        private SerializedProperty autoStartConversation;
        private SerializedProperty conversationName;
        private int selectedType;
        protected override void OnEnable() 
        {
            base.OnEnable();
            TaskSO = serializedObject.FindProperty("TaskSO");
            TaskStatus = serializedObject.FindProperty("TaskStatus");
            isStartPoint = serializedObject.FindProperty("isStartPoint");
            isDailyTask = serializedObject.FindProperty("isDailyTask");
            autoStartNext = serializedObject.FindProperty("autoStartNext");
            autoStartNextTaskPoint = serializedObject.FindProperty("autoStartNextTaskPoint");
            autoStartConversation = serializedObject.FindProperty("autoStartConversation");
            conversationName = serializedObject.FindProperty("conversationName");
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Use this for only task and if the task start or clear before the conversation starts", MessageType.Info);
            EditorGUILayout.Space();


            base.OnInspectorGUI();
            serializedObject.Update();
            


            EditorGUILayout.LabelField("Task Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(TaskSO);
            EditorGUILayout.PropertyField(TaskStatus);
            
            selectedType = EditorGUILayout.Popup("Task Point Type", isStartPoint.boolValue ? 0 : 1, new string[] {"Start Point", "Finish Point"});
            isStartPoint.boolValue = (selectedType == 0);
            EditorGUILayout.PropertyField(isDailyTask);
            EditorGUILayout.Space();


            EditorGUILayout.PropertyField(autoStartNext);
            if(autoStartNext.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(autoStartNextTaskPoint);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                EditorGUILayout.HelpBox("No task will start from finishing this task. Check Auto Start Next to enable", MessageType.Info);
            }
            EditorGUILayout.Space();


            EditorGUILayout.PropertyField(autoStartConversation);
            if(autoStartConversation.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(conversationName);
                EditorGUI.indentLevel -= 1;
            }
            else
            {
                EditorGUILayout.HelpBox("No conversation will start from interacting with this task. Check Auto Start Conversation to enable", MessageType.Info);
            }

            if(target.GetComponent<ConversationPoint>() != null)
            {
                EditorGUILayout.HelpBox("Only use a Conversation Point or a Task Point depending on if task start/clear first or conversation start first", MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
