using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Drorwer
{
    [CustomPropertyDrawer(typeof(AudioData.SFXEntry))]
    public class SFXEntryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Fetch properties
            SerializedProperty keyProp = property.FindPropertyRelative("key");
            SerializedProperty clipProp = property.FindPropertyRelative("clip");
            SerializedProperty volProp = property.FindPropertyRelative("volume");
            SerializedProperty pitchProp = property.FindPropertyRelative("pitch");

            // Define rects
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float padding = 2f;

            Rect foldoutRect = new Rect(position.x, position.y, position.width, lineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;
                Rect currentRect = new Rect(position.x, position.y + lineHeight + padding, position.width, lineHeight);

                // Draw Key
                EditorGUI.PropertyField(currentRect, keyProp);
                currentRect.y += lineHeight + padding;

                // Draw Clip & Play Button on the same line
                Rect clipRect = new Rect(currentRect.x, currentRect.y, currentRect.width - 60f, lineHeight);
                Rect btnRect = new Rect(currentRect.x + currentRect.width - 55f, currentRect.y, 55f, lineHeight);

                EditorGUI.PropertyField(clipRect, clipProp);

                // This is the cool part: A button to play audio directly in the editor!
                if (GUI.Button(btnRect, "▶ Play"))
                {
                    PlayClip(clipProp.objectReferenceValue as AudioClip);
                }
                currentRect.y += lineHeight + padding;

                // Draw Volume
                EditorGUI.PropertyField(currentRect, volProp);
                currentRect.y += lineHeight + padding;

                // Draw Pitch
                EditorGUI.PropertyField(currentRect, pitchProp);

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                return (EditorGUIUtility.singleLineHeight + 2f) * 5f;
            }
            return EditorGUIUtility.singleLineHeight;
        }

        // A clever reflection hack to play audio in the editor without entering Play Mode
        private void PlayClip(AudioClip clip)
        {
            if (clip == null) return;
            try
            {
                Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
                System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");

                MethodInfo method = audioUtilClass.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) }, null);
            
                if (method == null)
                {
                    method = audioUtilClass.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) }, null);
                }

                if (method != null)
                {
                    method.Invoke(null, new object[] { clip, 0, false });
                }
                else
                {
                    Debug.LogWarning("Could not find the internal audio play method for this Unity version.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error playing audio clip: " + e.Message);
            }
        }
    }
}