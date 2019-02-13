using System;
using UnityEngine;

namespace ShuHai.Unity
{
    public class Updater : MonoBehaviour
    {
        public static Updater Instance => SingleComponent<Updater>.Instance;

        public static event Action UpdateEvent
        {
            add => Instance.ThisUpdate += value;
            remove => Instance.ThisUpdate -= value;
        }

        private event Action ThisUpdate;

        private void Update() { ThisUpdate?.Invoke(); }

        private void Awake() { DontDestroyOnLoad(gameObject); }
    }
}