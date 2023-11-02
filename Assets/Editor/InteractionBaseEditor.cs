using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

namespace INTENT
{
    [CustomEditor(typeof(InteractionBase))]
    public class InteractionBaseEditor : Editor
    {
        private SerializedProperty isConversation;
        private SerializedProperty conversationName;
        private SerializedProperty hasActionPrefab;
        private SerializedProperty actionName;
        private SerializedProperty playerAction;
        private SerializedProperty needPressInteract;
        private SerializedProperty showIndicateSphere;
        private SerializedProperty canPerformOnlyOnce;
        private SerializedProperty hasBeforeTask;
        private SerializedProperty beforePerformTasks;
        private SerializedProperty hasAfterTask;
        private SerializedProperty afterPerformTasks;
        private SerializedProperty waitAfterPerformTime;
        private SerializedProperty canActivateUltimatePoints;
        private SerializedProperty activateUltimatePoints;
        private SerializedProperty canDeactivateUltimatePoints;
        private SerializedProperty deactivateUltimatePoints;

        private void OnEnable()
        {
            isConversation = serializedObject.FindProperty("isConversation");
            conversationName = serializedObject.FindProperty("conversationName");
            hasActionPrefab = serializedObject.FindProperty("hasActionPrefab");
            actionName = serializedObject.FindProperty("actionName");
            playerAction = serializedObject.FindProperty("playerAction");
            needPressInteract = serializedObject.FindProperty("NeedPressInteract");
            showIndicateSphere = serializedObject.FindProperty("ShowIndicateSphere");
            canPerformOnlyOnce = serializedObject.FindProperty("CanPerformOnlyOnce");
            hasBeforeTask = serializedObject.FindProperty("hasBeforeTask");
            beforePerformTasks = serializedObject.FindProperty("BeforePerformTasks");
            hasAfterTask = serializedObject.FindProperty("hasAfterTask");
            afterPerformTasks = serializedObject.FindProperty("AfterPerformTasks");
            waitAfterPerformTime = serializedObject.FindProperty("waitAfterPerformTime");
            canActivateUltimatePoints = serializedObject.FindProperty("canActivateUltimatePoints");
            activateUltimatePoints = serializedObject.FindProperty("activateUltimatePoints");
            canDeactivateUltimatePoints = serializedObject.FindProperty("canDeactivateUltimatePoints");
            deactivateUltimatePoints = serializedObject.FindProperty("deactivateUltimatePoints");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(isConversation);
            EditorGUI.indentLevel += 1;
            if (isConversation.boolValue)
            {
                EditorGUILayout.PropertyField(conversationName);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.LabelField("Conversation Settings", EditorStyles.boldLabel);
            }
            else
            {
                EditorGUILayout.PropertyField(hasActionPrefab);
                if (hasActionPrefab.boolValue)
                {
                    EditorGUILayout.PropertyField(playerAction);
                    EditorGUILayout.Space();
                }
                else
                {
                    EditorGUILayout.PropertyField(actionName);
                    EditorGUILayout.Space();
                }
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
                EditorGUILayout.LabelField("Action Settings", EditorStyles.boldLabel);
            }

            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(needPressInteract);
            EditorGUILayout.PropertyField(showIndicateSphere);
            EditorGUILayout.PropertyField(canPerformOnlyOnce);
            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;


            EditorGUILayout.LabelField("Task Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(hasBeforeTask);
            if(hasBeforeTask.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(beforePerformTasks, true);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(hasAfterTask);
            if (hasAfterTask.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(afterPerformTasks, true);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.LabelField("Delay between perform and after perform", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(waitAfterPerformTime);
            if(waitAfterPerformTime.floatValue < 0.1)
            {
                waitAfterPerformTime.floatValue = 0.1f;
            }
            
            EditorGUILayout.Space();
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.LabelField("Activate next Ultimate points", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(canActivateUltimatePoints);
            if (canActivateUltimatePoints.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(activateUltimatePoints, true);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.PropertyField(canDeactivateUltimatePoints);
            if (canDeactivateUltimatePoints.boolValue)
            {
                EditorGUI.indentLevel += 1;
                EditorGUILayout.PropertyField(deactivateUltimatePoints, true);
                EditorGUILayout.Space();
                EditorGUI.indentLevel -= 1;
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
