using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShuHai.Unity.PackageSetup
{
    public class PluginSettings : ScriptableObject
    {
        public static PluginSettings LoadOrCreate(string assetPath)
        {
            var obj = AssetDatabase.LoadAssetAtPath<PluginSettings>(assetPath);
            if (obj)
                return obj;

            obj = CreateInstance<PluginSettings>();
            AssetDatabase.CreateAsset(obj, assetPath);
            return obj;
        }

        #region Plugins

        [Serializable]
        public class Plugin
        {
            public string Name;

            public bool SupportEditor;

            public BuildTarget[] ExcludePlatforms;
        }

        public List<Plugin> Plugins = new List<Plugin>();

        public Dictionary<string, Plugin> PluginsToDict()
        {
            var comparer = EqualityComparerFactory.Create<Plugin>(
                (l, r) => l.Name == r.Name, p => p.Name.GetHashCode());
            return Plugins.Distinct(comparer).ToDictionary(p => p.Name, p => p);
        }

        #endregion Plugins
    }
}