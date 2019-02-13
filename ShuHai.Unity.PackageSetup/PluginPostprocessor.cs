using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShuHai.Unity.PackageSetup
{
    using BuildTargetTraits = EnumTraits<BuildTarget>;

    public class PluginPostprocessor : AssetPostprocessor
    {
        public const string AssembliesPath = "Assets/ShuHai/Assemblies";

        public static void ApplyPluginSettings(IEnumerable<PluginImporter> pluginImporters)
        {
            if (pluginImporters == null)
                throw new ArgumentNullException(nameof(pluginImporters));

            foreach (var importer in pluginImporters)
                ApplyPluginSettings(importer);
        }

        public static void ApplyPluginSettings(PluginImporter importer)
        {
            if (importer == null)
                throw new ArgumentNullException(nameof(importer));

            var path = importer.assetPath;
            var dir = Path.GetDirectoryName(path);
            if (dir != AssembliesPath)
                return;

            var name = Path.GetFileNameWithoutExtension(path);
            var nameParts = name.Split('.');
            if (nameParts[0] != BaseNamespace)
                return;

            bool forEditor = nameParts.Contains("Editor");
            importer.SetCompatibleWithEditor(forEditor);
            if (forEditor)
            {
                foreach (var platform in BuildTargetTraits.Values.Where(t => t != BuildTarget.NoTarget))
                    importer.SetExcludeFromAnyPlatform(platform, true);
            }

            //Debug.Log($"ApplyPluginSettings({name})");
        }

        private static readonly string BaseNamespace = typeof(PluginPostprocessor).Namespace.Split('.')[0];

        //private void OnPreprocessAsset()
        //{
        //    if (!(assetImporter is PluginImporter pluginImporter))
        //        return;
        //    ApplyPluginSettings(pluginImporter);
        //}

        private static void OnPostprocessAllAssets(
            string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var pluginImporters = importedAssets
                .Select(p => AssetImporter.GetAtPath(p) as PluginImporter)
                .Where(p => p != null);
            ApplyPluginSettings(pluginImporters);
        }

        //[UnityEditor.Callbacks.DidReloadScripts]
        //private static void OnScriptsReloaded() { ApplyPluginSettings(PluginImporter.GetAllImporters()); }
    }
}