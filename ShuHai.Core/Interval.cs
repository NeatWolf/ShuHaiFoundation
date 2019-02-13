using System;

namespace ShuHai
{
    public struct Interval<T>
        where T : IComparable<T>
    {
        public T Min;
        public T Max;

        public bool IsValid => Max.CompareTo(Min) >= 0;

        public Interval(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public void Extend(T boundary)
        {
            if (boundary.LessThan(Min))
                Min = boundary;
            else if (boundary.GreaterThan(Max))
                Max = boundary;
        }

        public void Extend(Interval<T> other)
        {
            if (other.Min.LessThan(Min))
                Min = other.Min;
            if (other.Max.GreaterThan(Max))
                Max = other.Max;
        }

        public static Interval<T> Union(Interval<T> i1, Interval<T> i2)
            => new Interval<T>(ComparableMath.Min(i1.Min, i2.Min), ComparableMath.Max(i1.Max, i2.Max));

        public bool Contains(T value, bool closed = true) => Contains(Min, Max, value, closed);

        public static bool Contains(T min, T max, T value, bool closed = true)
        {
            return closed
                ? value.GreaterThanOrEqualTo(min) && value.LessThanOrEqualTo(max)
                : value.GreaterThan(min) && value.LessThan(max);
        }

        public override string ToString() => $"[{Min},{Max}]";
    }
}