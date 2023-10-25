using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace INTENT
{
    [CustomEditor(typeof(UltimateInteractionPoint))]
    public class UltimateInteractionPointEditor : Editor
    {
        private SerializedProperty sphereCollider;
        private SerializedProperty hintText;
        private SerializedProperty indicatorSphere;
        private SerializedProperty available;
        private SerializedProperty pointLocation;
        private SerializedProperty interactionFolder;
        private SerializedProperty Interactions;
        private UltimateInteractionPoint thisPoint;

        private void OnEnable()
        {
            sphereCollider = serializedObject.FindProperty("sphereCollider");
            hintText = serializedObject.FindProperty("hintText");
            indicatorSphere = serializedObject.FindProperty("indicatorSphere");
            available = serializedObject.FindProperty("available");
            pointLocation = serializedObject.FindProperty("pointLocation");
            interactionFolder = serializedObject.FindProperty("interactionFolder");
            Interactions = serializedObject.FindProperty("Interactions");
            thisPoint = (UltimateInteractionPoint)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(sphereCollider);
            EditorGUILayout.PropertyField(hintText);
            EditorGUILayout.PropertyField(indicatorSphere);
            EditorGUILayout.PropertyField(interactionFolder);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Point Properties", EditorStyles.boldLabel);
            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(available);
            EditorGUILayout.PropertyField(pointLocation);
            EditorGUILayout.HelpBox("Interactions are added through adding Interaction Prefabs under the InteractionList and pressing the Load Interaction button", MessageType.Info);
            EditorGUILayout.PropertyField(Interactions);
            EditorGUI.indentLevel -= 1;
            EditorGUILayout.Space();

            if (GUILayout.Button("Load Interaction"))
            {
                thisPoint.LoadAllInteractions();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
