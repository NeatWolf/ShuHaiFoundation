using UnityEditor;
using UnityEngine;

namespace ShuHai.Unity.PackageSetup
{
    [CustomEditor(typeof(ArchiveInstaller))]
    public class ArchiveInstallerEditor : Editor
    {
        public ArchiveInstaller Target => (ArchiveInstaller)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ExtractionGUI();
        }

        #region Extraction

        private void ExtractionGUI()
        {
            EditorGUILayout.TextField("Last Extracted Archive", ArchiveInstaller.LastExtractedArchive);

            if (GUILayout.Button("Extract Now"))
                ArchiveInstaller.SelectAndExtractArchive(true);
        }

        #endregion Extraction
    }
}