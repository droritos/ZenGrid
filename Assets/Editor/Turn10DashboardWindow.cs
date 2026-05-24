using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    public class Turn10DashboardWindow : EditorWindow
    {
        private Texture2D logo;
        private string scanResult = "System Ready.";

        [MenuItem("Tools/Dashboard Window")]
        public static void ShowWindow()
        {
            GetWindow<Turn10DashboardWindow>("Turn10 Dashboard");
        }

        private void OnEnable()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Art/Icon/Turn10 New Logo.png");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            if (logo != null)
            {
                Rect rect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandWidth(true));
                GUI.DrawTexture(rect, logo, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.Space(20);
        
            if (GUILayout.Button("Scan Project for Audio Assets", GUILayout.Height(30)))
            {
                string[] guids = AssetDatabase.FindAssets("t:AudioClip");
                scanResult = "Found " + guids.Length + " audio files in project.";
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(scanResult, MessageType.Info);

            EditorGUILayout.Space(20);

            if (GUILayout.Button("Clear All Local Save Data", GUILayout.Height(30)))
            {
                if (EditorUtility.DisplayDialog("Warning", "Delete all save data?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();
                }
            }
        }
    }
}