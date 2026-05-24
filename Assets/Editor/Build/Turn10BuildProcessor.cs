using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameEditor.Build
{
    public class Turn10BuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            bool clearSaves = EditorUtility.DisplayDialog(
                "Pre-Build Setup",
                "Do you want to clear all local PlayerPrefs before building to ensure a clean test environment?",
                "Yes, Clear Data",
                "No, Keep Data"
            );

            if (clearSaves)
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("[Turn10 Build] PlayerPrefs cleared for a fresh build.");
            }

            bool continueBuild = EditorUtility.DisplayDialog(
                "Final Validation",
                "Build process is about to start. Are you sure you want to proceed?",
                "Start Build",
                "Cancel Build"
            );

            if (!continueBuild)
            {
                throw new BuildFailedException("Build was manually canceled by the developer.");
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log("Build succeeded. I am Not an Agent Lior! ");
            }
        }
    }
}