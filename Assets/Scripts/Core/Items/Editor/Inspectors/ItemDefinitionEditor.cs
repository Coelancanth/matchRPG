using UnityEngine;
using UnityEditor;
using Core.Items.Data;
using System;
using System.Collections.Generic;

    [CustomEditor(typeof(ItemDefinition))]
    public class ItemDefinitionEditor : UnityEditor.Editor
    {
        private static readonly Dictionary<string, Type> ComponentTypes = new Dictionary<string, Type>
        {
            { "Weapon", typeof(WeaponComponentDefinition) },
            { "Armor", typeof(ArmorComponentDefinition) }
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add Component", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            foreach (var componentType in ComponentTypes)
            {
                if (GUILayout.Button(componentType.Key))
                {
                    var components = serializedObject.FindProperty("_components");
                    components.InsertArrayElementAtIndex(components.arraySize);
                    var newElement = components.GetArrayElementAtIndex(components.arraySize - 1);
                    newElement.managedReferenceValue = Activator.CreateInstance(componentType.Value);
                    serializedObject.ApplyModifiedProperties();
                }
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }