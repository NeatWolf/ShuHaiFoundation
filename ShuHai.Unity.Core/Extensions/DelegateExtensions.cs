using System;

namespace ShuHai.Unity
{
    public static class DelegateExtensions
    {
        #region Funcs

        public static TRet InvokeIfNotNull<TRet>(this Func<TRet> self, TRet fallback = default(TRet))
        {
            return self != null ? self() : fallback;
        }

        public static TRet InvokeIfNotNull<TArg, TRet>(
            this Func<TArg, TRet> self, TArg arg, TRet fallback = default(TRet))
        {
            return self != null ? self(arg) : fallback;
        }

        public static TRet InvokeIfNotNull<TArg1, TArg2, TRet>(
            this Func<TArg1, TArg2, TRet> self,
            TArg1 a1, TArg2 a2, TRet fallback = default(TRet))
        {
            return self != null ? self(a1, a2) : fallback;
        }

        public static TRet InvokeIfNotNull<TArg1, TArg2, TArg3, TRet>(
            this Func<TArg1, TArg2, TArg3, TRet> self,
            TArg1 a1, TArg2 a2, TArg3 a3, TRet fallback = default(TRet))
        {
            return self != null ? self(a1, a2, a3) : fallback;
        }

        public static TRet InvokeIfNotNull<TArg1, TArg2, TArg3, TArg4, TRet>(
            this Func<TArg1, TArg2, TArg3, TArg4, TRet> self,
            TArg1 a1, TArg2 a2, TArg3 a3, TArg4 a4, TRet fallback = default(TRet))
        {
            return self != null ? self(a1, a2, a3, a4) : fallback;
        }

        //public static TRet InvokeIfNotNull<TArg1, TArg2, TArg3, TArg4, TArg5, TRet>(
        //    this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TRet> self,
        //    TArg1 a1, TArg2 a2, TArg3 a3, TArg4 a4, TArg5 a5, TRet fallback = default(TRet))
        //{
        //    return self != null ? self(a1, a2, a3, a4, a5) : fallback;
        //}

        //public static TRet InvokeIfNotNull<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TRet>(
        //    this Func<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TRet> self,
        //    TArg1 a1, TArg2 a2, TArg3 a3, TArg4 a4, TArg5 a5, TArg6 a6, TRet fallback = default(TRet))
        //{
        //    return self != null ? self(a1, a2, a3, a4, a5, a6) : fallback;
        //}

        #endregion Funcs

        public static bool InvokeIfNotNull<TRet>(this Predicate<TRet> self, TRet arg, bool fallback = false)
        {
            return self?.Invoke(arg) ?? fallback;
        }
    }
}