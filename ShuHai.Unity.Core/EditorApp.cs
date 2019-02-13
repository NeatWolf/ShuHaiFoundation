using System;
using System.Linq;
using System.Reflection;

namespace ShuHai.Unity
{
    /// <summary>
    ///     A proxy class that delegates some functions in UnityEditor.EditorApplication if the current runtime is actually
    ///     running with UnityEditor assembly loaded.
    /// </summary>
    public static class EditorApp
    {
        /// <summary>
        ///     Indicates whether the class is valid to use.
        /// </summary>
        public static bool Valid => Assemblies.UnityEditor != null;

        #region Update Event

        public static event Action Update
        {
            add => (UpdateAddMethod ?? DefaultUpdateAdd)(value);
            remove => (UpdateRemoveMethod ?? DefaultUpdateRemove)(value);
        }

        public static Action<Action> UpdateAddMethod;
        public static Action<Action> UpdateRemoveMethod;

        private static void DefaultUpdateAdd(Action value) => UpdateModify(Delegate.Combine, value);
        private static void DefaultUpdateRemove(Action value) => UpdateModify(Delegate.Remove, value);

        private static void UpdateModify(Func<Delegate, Delegate, Delegate> method, Action value)
        {
            var valueDelegate = Delegate.CreateDelegate(callbackFunctionType, value.Target, value.Method);
            var combinedDelegate = method((Delegate)updateFieldInfo.GetValue(null), valueDelegate);
            updateFieldInfo.SetValue(null, combinedDelegate);
        }

        private static FieldInfo updateFieldInfo => _updateFieldInfo.Value;

        private static readonly Lazy<FieldInfo> _updateFieldInfo = new Lazy<FieldInfo>(
            () => editorApplicationType.GetField("update", BindingFlags.Static | BindingFlags.Public));

        #endregion Update Event

        #region Reflected Types

        private static Type editorApplicationType => _editorApplicationType.Value;
        private static Type callbackFunctionType => _callbackFunctionType.Value;

        private static readonly Lazy<Type> _editorApplicationType =
            new Lazy<Type>(() => FindType("UnityEditor.EditorApplication"));

        private static readonly Lazy<Type> _callbackFunctionType =
            new Lazy<Type>(() => FindType("UnityEditor.EditorApplication+CallbackFunction"));

        private static Type FindType(string fullName)
            => Assemblies.UnityEditor.GetTypes().First(t => t.FullName == fullName);

        #endregion Reflected Types
    }
}