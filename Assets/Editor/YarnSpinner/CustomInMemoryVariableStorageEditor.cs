﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Yarn.Unity.Editor
{

    [CustomEditor(typeof(CustomInMemoryVariableStorage))]
    public class CustomInMemoryVariableStorageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var varStorage = (CustomInMemoryVariableStorage)target;

            varStorage.showDebug = EditorGUILayout.Foldout(varStorage.showDebug, "Debug Variables");

            if (!varStorage.showDebug)
            {
                return;
            }

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Not in Play Mode, so no variables to display!", MessageType.Info);
                return;
            }

            var style = EditorStyles.label;
            var list = varStorage.GetDebugList();
            var height = style.CalcHeight(new GUIContent(list), EditorGUIUtility.currentViewWidth);
            EditorGUILayout.SelectableLabel(list, GUILayout.MaxHeight(height), GUILayout.ExpandHeight(true));

        }
    }

}
