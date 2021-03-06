﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ShuHai
{
    public class HashCode
    {
        #region Value

        public static implicit operator int(HashCode hc) { return hc.Value; }

        public int Value;

        public HashCode(int value) { Value = value; }

        public HashCode Add(int value)
        {
            Value = Combine(Value, value);
            return this;
        }

        public HashCode Add<T>(T obj)
        {
            Value = Combine(Value, Get(obj));
            return this;
        }

        #endregion Value

        #region Gets

        public static int Get<T1, T2>(T1 o1, T2 o2) => Combine(Get(o1), Get(o2));

        public static int Get<T1, T2, T3>(T1 o1, T2 o2, T3 o3) => Combine(Get(o1), Get(o2), Get(o3));

        public static int Get<T1, T2, T3, T4>(T1 o1, T2 o2, T3 o3, T4 o4)
            => Combine(Get(o1), Get(o2), Get(o3), Get(o4));

        public static int Get<T1, T2, T3, T4, T5>(T1 o1, T2 o2, T3 o3, T4 o4, T5 o5)
            => Combine(Get(o1), Get(o2), Get(o3), Get(o4), Get(o5));

        public static int Get<T1, T2, T3, T4, T5, T6>(T1 o1, T2 o2, T3 o3, T4 o4, T5 o5, T6 o6)
            => Combine(Get(o1), Get(o2), Get(o3), Get(o4), Get(o5), Get(o6));

        public static int Get<T1, T2, T3, T4, T5, T6, T7>(T1 o1, T2 o2, T3 o3, T4 o4, T5 o5, T6 o6, T7 o7)
            => Combine(Get(o1), Get(o2), Get(o3), Get(o4), Get(o5), Get(o6), Get(o7));

        public static int Get<T1, T2, T3, T4, T5, T6, T7, T8>(T1 o1, T2 o2, T3 o3, T4 o4, T5 o5, T6 o6, T7 o7, T8 o8)
            => Combine(Get(o1), Get(o2), Get(o3), Get(o4), Get(o5), Get(o6), Get(o7), Get(o8));

        public static int Get<T>(IEnumerable<T> objects)
            => CollectionUtil.IsNullOrEmpty(objects) ? 0 : Combine(objects.Select((Func<T, int>)Get));

        public static int Get<T>(T value) => Get(value, null);

        /// <summary>
        ///     Get hash code of specified value with specified comparer if possible.
        /// </summary>
        /// <remarks>
        ///     This method is not recommanded when performance is critical. Actually you'd better only use it in an immutable type
        ///     wherever the hash code only calculated once in the constructor.
        /// </remarks>
        public static int Get<T>(T value, IEqualityComparer<T> comparer)
        {
            if (comparer != null)
                return comparer.GetHashCode(value);

            return typeof(T).IsValueType // Avoid boxing of value type.
                ? value.GetHashCode()
                : ((object)value == null ? 0 : value.GetHashCode());
        }

        #endregion Gets

        #region Combine

        public static int Combine(int h1, int h2)
        {
            if (h1 == 0)
                return h2;
            if (h2 == 0)
                return h1;
            return unchecked(((h1 << 5) + h1) ^ h2);
        }

        public static int Combine(int h1, int h2, int h3) => Combine(Combine(h1, h2), h3);

        public static int Combine(int h1, int h2, int h3, int h4) => Combine(Combine(h1, h2, h3), h4);

        public static int Combine(int h1, int h2, int h3, int h4, int h5) => Combine(Combine(h1, h2, h3, h4), h5);

        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6)
            => Combine(Combine(h1, h2, h3, h4, h5), h6);

        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
            => Combine(Combine(h1, h2, h3, h4, h5, h6), h7);

        public static int Combine(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
            => Combine(Combine(h1, h2, h3, h4, h5, h6, h7), h8);

        public static int Combine(IEnumerable<int> values) => values.Aggregate(Combine);

        #endregion Combine

        #region Disabled Overrides

#pragma warning disable CS0809
        [Obsolete("GetHashCode for HashCode struct is not available.", true)]
        public override int GetHashCode() =>
            throw new NotSupportedException("GetHashCode for HashCode struct is not available.");
#pragma warning restore CS0809

        #endregion Disabled Overrides
    }
}