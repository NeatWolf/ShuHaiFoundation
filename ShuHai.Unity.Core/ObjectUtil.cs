using System;

namespace ShuHai.Unity
{
    public static class ObjectUtil
    {
        public static Type GetType(object obj) { return obj != null ? obj.GetType() : null; }
    }
}