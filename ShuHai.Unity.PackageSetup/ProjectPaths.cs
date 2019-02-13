using System.IO;
using UnityEngine;

namespace ShuHai.Unity.PackageSetup
{
    /// <summary>
    ///     Commonly used rooted path of current project.
    /// </summary>
    public static class ProjectPaths
    {
        #region Project Root Directories

        public static string Root => root ?? (root = Path.GetDirectoryName(Assets));

        public static string Settings => settings ?? (settings = Root + @"\ProjectSettings");

        public static string Temp => temp ?? (temp = Root + "/Temp");

        private static string root;
        private static string settings;
        private static string temp;

        #endregion Project Root Directories

        #region Assets Directories

        public static string Assets => assets ?? (assets = PathUtil.Normalize(Application.dataPath, '\\'));

        public static string ProjectAssets
            => projectAssets ?? (projectAssets = Assets + '\\' + typeof(ProjectPaths).Namespace.Split('.')[0]);

        public static string Archives => archives ?? (archives = ProjectAssets + @"\Archives");

        public static string Assemblies => assemblies ?? (assemblies = ProjectAssets + @"\Assemblies");

        public static string Generated => generated ?? (generated = ProjectAssets + @"\Generated");

        private static string assets;
        private static string projectAssets;
        private static string archives;
        private static string assemblies;
        private static string generated;

        #endregion Assets Directories
    }
}