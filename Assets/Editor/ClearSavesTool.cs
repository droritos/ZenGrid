using UnityEditor;
using UnityEngine;

namespace GameEditor
{
    public class ClearSavesTool : EditorWindow
    {
        //shortcut: Ctrl+Shift+C
        [MenuItem("ShortcutTools/Clear Saved Data %#c")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("✅ [Turn10] All PlayerPrefs (Saved Data) have been cleared!");
        }
    }
}