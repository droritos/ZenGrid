using UnityEditor;
using UnityEngine;
using ZenGrid;

namespace GameEditor.Inspectors
{
    [CustomEditor(typeof(GameModeConfig))]
    public class GameModeConfigEditor : UnityEditor.Editor
    {
        private Texture2D trophyIcon;

        private SerializedProperty modeTypeProp;
        private SerializedProperty displayNameProp;
        private SerializedProperty descriptionProp;
        private SerializedProperty turnsBetweenLotusProp;
        private SerializedProperty lotusCanSpreadProp;
        private SerializedProperty maxSimultaneousLotusProp;

        private void OnEnable()
        {
            trophyIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Art/Icon/trophy.png");

            modeTypeProp = serializedObject.FindProperty("modeType");
            displayNameProp = serializedObject.FindProperty("displayName");
            descriptionProp = serializedObject.FindProperty("description");
            turnsBetweenLotusProp = serializedObject.FindProperty("turnsBetweenLotus");
            lotusCanSpreadProp = serializedObject.FindProperty("lotusCanSpread");
            maxSimultaneousLotusProp = serializedObject.FindProperty("maxSimultaneousLotus");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.fontSize = 14;
            titleStyle.normal.textColor = new Color(0.2f, 0.6f, 1.0f);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Turn10 Game Mode Controller", titleStyle);
            EditorGUILayout.Space();

            if (trophyIcon != null)
            {
                Rect rect = GUILayoutUtility.GetRect(40, 40, GUILayout.ExpandWidth(false));
                GUI.DrawTexture(rect, trophyIcon);
                EditorGUILayout.Space();
            }

            EditorGUILayout.LabelField("Identity", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(modeTypeProp);
            EditorGUILayout.PropertyField(displayNameProp);
            EditorGUILayout.PropertyField(descriptionProp);

            EditorGUILayout.Space();

            if (modeTypeProp.enumValueIndex == 0)
            {
                EditorGUILayout.HelpBox("Classic Mode Active: Lotus enemies will spawn and spread.", MessageType.Info);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Lotus Settings (Classic only)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(turnsBetweenLotusProp);
                EditorGUILayout.PropertyField(lotusCanSpreadProp);
                EditorGUILayout.PropertyField(maxSimultaneousLotusProp);

                if (turnsBetweenLotusProp.intValue <= 0)
                {
                    EditorGUILayout.HelpBox("Warning: 'Turns Between Lotus' is 0 or less. The board will flood with Lotus instantly!", MessageType.Error);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Zen Mode Active: All Lotus mechanics are completely disabled and hidden from this view.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}