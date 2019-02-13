using System;

namespace ShuHai
{
    public static class ComparableMath
    {
        public static T Min<T>(T value1, T value2) where T : IComparable<T>
            => value1.LessThan(value2) ? value1 : value2;

        public static T Max<T>(T value1, T value2) where T : IComparable<T>
            => value1.GreaterThan(value2) ? value1 : value2;
    }
}