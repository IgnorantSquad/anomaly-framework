#if UNITY_EDITOR

namespace Anomaly.Editor
{
    using System;
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    [CustomEditor(typeof(CustomBehaviour), true)]
    public class CustomBehaviourEditor : Editor
    {
        private CustomBehaviour self = null;
        private Dictionary<string, bool> editorFold = new Dictionary<string, bool>();

        private List<System.Reflection.FieldInfo> serializedFields = new List<System.Reflection.FieldInfo>();
        private List<System.Reflection.FieldInfo> componentDataList = new List<System.Reflection.FieldInfo>();

        private int selectedTab = 0;
        private string[] tabs = new string[] {
            "All",
            "Components",
            "Fields"
        };


        private void OnEnable()
        {
            self = target as CustomBehaviour;

            var fields = target.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            foreach (var field in fields)
            {
                if (!field.IsPublic && Attribute.GetCustomAttribute(field, typeof(SerializeField)) == null) continue;
                if (Attribute.GetCustomAttribute(field, typeof(HideInInspector)) != null) continue;

                if (!field.FieldType.IsSubclassOf(typeof(CustomComponent)))
                {
                    serializedFields.Add(field);
                    continue;
                }

                componentDataList.Add(field);
            }
        }


        public override void OnInspectorGUI()
        {
            GUILayout.Space(5);

            selectedTab = GUILayout.Toolbar(selectedTab, tabs);

            GUILayout.Space(10);

            switch (selectedTab)
            {
                case 0:
                    ShowComponentTab();
                    GUILayout.Space(10);
                    EditorUtils.DrawHorizontalLine(Color.gray);
                    GUILayout.Space(20);
                    ShowBaseTab();
                    break;
                case 1:
                    ShowComponentTab();
                    break;
                case 2:
                    ShowBaseTab();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowComponentTab()
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorUtils.Label("Component Data", 16, FontStyle.Bold);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel += 1;

            for (int i = 0; i < componentDataList.Count; ++i)
            {
                EditorGUILayout.BeginVertical("box");
                Serialize(componentDataList[i].Name);
                EditorGUILayout.EndVertical();
            }

            EditorGUI.indentLevel -= 1;

            EditorGUILayout.EndVertical();
        }

        private void ShowBaseTab()
        {
            float prevWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = 70F;

            EditorGUI.indentLevel += 1;

            for (int i = 0; i < serializedFields.Count; ++i)
            {
                Serialize(serializedFields[i].Name);
            }

            EditorGUI.indentLevel -= 1;

            EditorGUIUtility.labelWidth = prevWidth;
        }

        private void Serialize(string fieldName)
        {
            var serializedProperty = serializedObject.FindProperty(fieldName);
            if (serializedProperty == null) return;

            EditorGUILayout.PropertyField(serializedProperty, true);
        }
    }
}
#endif