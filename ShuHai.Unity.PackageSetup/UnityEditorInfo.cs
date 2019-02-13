using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ShuHai.Unity.PackageSetup
{
    public static class UnityEditorInfo
    {
        #region Version

        public static UnityVersion Version => version ?? (version = ParseVersion());

        private static UnityVersion version;

        private static readonly Regex VersionRegex = new Regex(@"m_EditorVersion: (?<ver>\d+\.\d+\.*\d*\w*\d*)\Z");

        private static UnityVersion ParseVersion()
        {
            var versionPath = Path.Combine(ProjectPaths.Settings, "ProjectVersion.txt");
            var versionText = File.ReadAllText(versionPath);

            var match = VersionRegex.Match(versionText);
            if (!match.Success)
                throw new InvalidOperationException("Failed to parse unity version from ProjectVersion.txt.");

            var ver = match.Groups["ver"].Value;
            return UnityVersion.Parse(ver);
        }

        #endregion Version
    }
}