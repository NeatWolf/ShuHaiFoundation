using System;

namespace ShuHai
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class CompatibleUnityVersionAttribute : Attribute
    {
        public UnityVersion MinUnityVersion;

        public UnityVersion MaxUnityVersion;

        public CompatibleUnityVersionAttribute(string minUnityVersion, string maxUnityVersion = null)
        {
            MinUnityVersion = ParseUnityVersion(minUnityVersion);
            MaxUnityVersion = ParseUnityVersion(maxUnityVersion);
        }

        private static UnityVersion ParseUnityVersion(string version)
        {
            return string.IsNullOrEmpty(version) ? null : UnityVersion.Parse(version);
        }
    }
}