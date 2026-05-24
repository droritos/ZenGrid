using UnityEditor;
using UnityEngine;

namespace GameEditor.Inspectors
{
    [CustomEditor(typeof(ShapeDatabase))]
    public class ShapeDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.fontSize = 13;
            titleStyle.normal.textColor = new Color(0.1f, 0.8f, 0.4f);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Shape Database Management", titleStyle);
            EditorGUILayout.Space();

            SerializedProperty shapesProp = serializedObject.FindProperty("shapes");

            if (shapesProp != null)
            {
                int count = shapesProp.arraySize;

                if (count == 0)
                {
                    EditorGUILayout.HelpBox("CRITICAL ERROR: The Shape Database is completely empty!", MessageType.Error);
                }
                else
                {
                    EditorGUILayout.HelpBox($"Database Integrity Checked. {count} shapes successfully registered.", MessageType.Info);
                }

                EditorGUILayout.PropertyField(shapesProp, true);
            }

            EditorGUILayout.Space();

            GUI.backgroundColor = new Color(0.1f, 0.7f, 1f);
            if (GUILayout.Button("Perform Deep Integrity Check", GUILayout.Height(30)))
            {
                int invalidShapes = 0;
                if (shapesProp != null)
                {
                    for (int i = 0; i < shapesProp.arraySize; i++)
                    {
                        SerializedProperty element = shapesProp.GetArrayElementAtIndex(i);
                        SerializedProperty wProp = element.FindPropertyRelative("width");
                        SerializedProperty hProp = element.FindPropertyRelative("height");
                        SerializedProperty matrixProp = element.FindPropertyRelative("matrix");

                        bool hasBlocks = false;

                        if (wProp != null && hProp != null && matrixProp != null)
                        {
                            if (wProp.intValue > 0 && hProp.intValue > 0)
                            {
                                for (int j = 0; j < matrixProp.arraySize; j++)
                                {
                                    if (matrixProp.GetArrayElementAtIndex(j).intValue == 1)
                                    {
                                        hasBlocks = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!hasBlocks)
                        {
                            invalidShapes++;
                        }
                    }
                }

                if (invalidShapes > 0)
                {
                    string errorMessage = $"Found {invalidShapes} invalid shapes:\n\n";

                    if (shapesProp != null)
                        for (int i = 0; i < shapesProp.arraySize; i++)
                        {
                            SerializedProperty element = shapesProp.GetArrayElementAtIndex(i);
                            SerializedProperty wProp = element.FindPropertyRelative("width");
                            SerializedProperty hProp = element.FindPropertyRelative("height");
                            SerializedProperty matrixProp = element.FindPropertyRelative("matrix");
                            SerializedProperty nameProp = element.FindPropertyRelative("name");

                            bool hasBlocks = false;
                            for (int j = 0; j < matrixProp.arraySize; j++)
                            {
                                if (matrixProp.GetArrayElementAtIndex(j).intValue == 1)
                                {
                                    hasBlocks = true;
                                    break;
                                }
                            }

                            // בודקים אם הצורה לא תקינה (ריק או ללא בלוקים)
                            if (wProp.intValue <= 0 || hProp.intValue <= 0 || !hasBlocks)
                            {
                                string shapeName = string.IsNullOrEmpty(nameProp.stringValue)
                                    ? $"Shape at index {i}"
                                    : $"Shape '{nameProp.stringValue}' (index {i})";

                                errorMessage += $"- {shapeName} is empty or invalid!\n";
                            }
                        }

                    EditorUtility.DisplayDialog("Database Validation Failed", errorMessage, "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Database Validation", "Perfect! All registered shape assets have blocks painted and are fully valid.", "Awesome");
                }
            }
            GUI.backgroundColor = Color.white;

            serializedObject.ApplyModifiedProperties();
        }
    }
}