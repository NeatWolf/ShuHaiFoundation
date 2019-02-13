using System;
using UnityEngine;

namespace ShuHai.Unity
{
    public static class SingleComponent<T>
        where T : Component
    {
        public static T Instance => GetInstance(true);

        public static T GetInstance(bool createOnNotFound)
        {
            if (instance != null)
                return instance;

            instance = UnityObjectUtil.FindUniqueSceneComponent<T>();

            if (instance == null)
            {
                if (createOnNotFound)
                {
                    var gameObject = new GameObject(typeof(T).Name);
                    instance = gameObject.AddComponent<T>();
                }
                else
                {
                    throw new InvalidOperationException($@"Instance of ""{typeof(T)}"" not found.");
                }
            }

            return instance;
        }

        private static T instance;
    }
}