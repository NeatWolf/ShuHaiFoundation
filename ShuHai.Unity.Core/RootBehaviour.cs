using UnityEngine;

namespace ShuHai.Unity
{
    internal sealed class RootBehaviour : MonoBehaviour
    {
        private void Update() { Root.OnUpdate(); }

        private void OnApplicationQuit() { Root.OnDeinitialize(); }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            var gameObject = new GameObject(typeof(RootBehaviour).FullName) { hideFlags = HideFlags.HideAndDontSave };
            DontDestroyOnLoad(gameObject);

            gameObject.AddComponent<RootBehaviour>();

            Root.OnInitialize();
        }
    }
}