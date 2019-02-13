using System;
using System.Collections.Generic;

namespace ShuHai
{
    /// <summary>
    ///     Utility class that create instances of <see cref="IEqualityComparer{T}" /> from method delegates.
    /// </summary>
    public static class EqualityComparerFactory
    {
        public static IEqualityComparer<T> CreateEquals<T>(Func<T, T, bool> method) => Create(method, null);

        public static IEqualityComparer<T> CreateGetHashCode<T>(Func<T, int> method) => Create(null, method);

        public static IEqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
            => new Instance<T>(equals, getHashCode);

        private class Instance<T> : IEqualityComparer<T>
        {
            public readonly Func<T, T, bool> EqualsDelegate = EqualityComparer<T>.Default.Equals;
            public readonly Func<T, int> GetHashCodeDelegate = EqualityComparer<T>.Default.GetHashCode;

            public Instance(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                if (equals != null)
                    EqualsDelegate = equals;
                if (getHashCode != null)
                    GetHashCodeDelegate = getHashCode;
            }

            public bool Equals(T x, T y) { return EqualsDelegate(x, y); }
            public int GetHashCode(T obj) { return GetHashCodeDelegate(obj); }
        }
    }
}