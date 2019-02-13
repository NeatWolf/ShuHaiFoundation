using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShuHai.Unity
{
    /// <summary>
    ///     A shortcut class for accessing various <see cref="Assembly" /> in unity project.
    /// </summary>
    public static class Assemblies
    {
        #region Commonly Used Names

        public const string UserRuntimeName = "Assembly-CSharp";
        public const string PluginRuntimeName = "Assembly-CSharp-firstpass";
        public const string UserEditorName = "Assembly-CSharp-Editor";
        public const string PluginEditorName = "Assembly-CSharp-Editor-firstpass";

        #endregion Commonly Used Names

        #region Commonly Used Instances

        public static readonly Assembly Mscorlib = typeof(object).Assembly;
        public static readonly Assembly UnityEngine = typeof(UnityEngine.Object).Assembly;
        public static Assembly UserRuntime => userRuntime.Value;
        public static Assembly PluginRuntime => pluginRuntime.Value;

        public static Assembly UnityEditor => unityEditor.Value;
        public static Assembly UserEditor => userEditor.Value;
        public static Assembly PluginEditor => pluginEditor.Value;

        private static readonly Lazy<Assembly> userRuntime = new Lazy<Assembly>(() => FindInCache(UserRuntimeName));
        private static readonly Lazy<Assembly> pluginRuntime = new Lazy<Assembly>(() => FindInCache(PluginRuntimeName));
        private static readonly Lazy<Assembly> unityEditor = new Lazy<Assembly>(() => FindInCache("UnityEditor"));
        private static readonly Lazy<Assembly> userEditor = new Lazy<Assembly>(() => FindInCache(UserEditorName));
        private static readonly Lazy<Assembly> pluginEditor = new Lazy<Assembly>(() => FindInCache(PluginEditorName));

        #endregion Commonly Used Instances

        #region Cached

        /// <summary>
        ///     Enumeration of all loaded assemblies.
        /// </summary>
        public static IEnumerable<Assembly> All => cached;

        /// <summary>
        ///     Number of cached assemblies.
        /// </summary>
        public static int CachedCount => cached.Length;

        public static Assembly FindInCache(string name)
        {
            Ensure.Argument.NotNull(name, nameof(name));
            return cached.FirstOrDefault(a => a.GetName().Name == name);
        }

        /// <summary>
        ///     Searches for an element that matches the condition defined by <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate"> Condition to match. </param>
        /// <returns>
        ///     The first element that matches the condition defined by <paramref name="predicate" />, if found;
        ///     otherwise, null.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="predicate" /> is null. </exception>
        public static Assembly FindInCache(Func<Assembly, bool> predicate)
        {
            Ensure.Argument.NotNull(predicate, nameof(predicate));
            return cached.FirstOrDefault(predicate);
        }

        public static void ReloadCache() { _cached = AppDomain.CurrentDomain.GetAssemblies(); }

        private static Assembly[] cached
        {
            get
            {
                if (_cached == null)
                    ReloadCache();
                return _cached;
            }
        }

        private static Assembly[] _cached;

        #endregion Cached
    }
}