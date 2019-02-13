using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShuHai.Unity.PackageSetup
{
    public class ArchiveInstaller : ScriptableObject
    {
        #region Instance

        public static ArchiveInstaller Instance { get; private set; }

        private static void InitializeInstance()
        {
            if (Instance)
                return;

            var path = AssetPaths.Generated + "/" + typeof(ArchiveInstaller).Name + ".asset";
            Instance = AssetDatabase.LoadAssetAtPath<ArchiveInstaller>(path);

            if (Instance == null)
            {
                Instance = CreateInstance<ArchiveInstaller>();
                AssetDatabase.CreateAsset(Instance, path);
            }
        }

        #endregion Instance

        #region Archives

        public static bool SelectAndExtractArchiveIfNecessary()
        {
            return string.IsNullOrEmpty(Instance.lastExtractedArchive) && SelectAndExtractArchive(false);
        }

        public static bool SelectAndExtractArchive(bool replaceIfExisted)
        {
            var path = SelectArchive();
            return !string.IsNullOrEmpty(path) && ExtractArchive(path, replaceIfExisted);
        }

        private static UnityVersion VersionOfPath(string path)
        {
            var verStr = Path.GetFileNameWithoutExtension(path);
            if (verStr.EndsWith("+"))
                verStr = verStr.RemoveTail(1);
            return UnityVersion.TryParse(verStr, out var ver) ? ver : null;
        }

        #region Extraction

        public static string LastExtractedArchive => Instance.lastExtractedArchive;

        public static bool ExtractArchive(string archivePath, bool replaceIfExisted)
        {
            int fileCount = ZipUtil.Unzip(archivePath, ProjectPaths.Assets, replaceIfExisted);

            if (Instance)
                Instance.lastExtractedArchive = archivePath;

            if (fileCount > 0)
                AssetDatabase.Refresh();
            return fileCount > 0;
        }

        [SerializeField, HideInInspector] private string lastExtractedArchive;

        #endregion Extraction

        #region Selection

        /// <summary>
        ///     Find path of most appropriate archive for current unity project.
        /// </summary>
        public static string SelectArchive()
        {
            var projVer = UnityEditorInfo.Version.MajorVersion;
            var archiveInfo = SelectArchiveInfos()
                .OrderByDescending(p => p.Value)
                .FirstOrDefault(p => projVer >= p.Value);
            return archiveInfo.Key;
        }

        private static IEnumerable<KeyValuePair<string, UnityVersion>> SelectArchiveInfos()
        {
            var zipPaths = Directory.GetFiles(ProjectPaths.Archives, "*.zip");
            foreach (var path in zipPaths)
            {
                var version = VersionOfPath(path);
                if (version != null)
                    yield return new KeyValuePair<string, UnityVersion>(path, version);
            }
        }

        #endregion Selection

        #endregion Archives

        #region Unity Messages

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            InitializeInstance();
            SelectAndExtractArchiveIfNecessary();
        }

        #endregion Unity Messages
    }
}