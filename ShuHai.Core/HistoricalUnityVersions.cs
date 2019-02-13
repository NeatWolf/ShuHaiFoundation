using System;
using System.Collections.Generic;

namespace ShuHai
{
    using ByteInterval = Interval<byte>;
    using UShortInterval = Interval<ushort>;
    using NumberingStyleTraits = EnumTraits<HistoricalUnityVersions.NumberingStyle>;

    public static class HistoricalUnityVersions
    {
        #region Related Versions

        /// <summary>
        ///     Get the next major version of specified version.
        ///     For example, next major version of Unity-5.4 is Unity-5.5 and next major version of Unity-5.6 is Unity-2017.1.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the next major version successfully fetched; otherwise, <see langword="false" />
        ///     if the specified version is latest major version.
        /// </returns>
        /// <remarks>
        ///     Note that the next version is affected by interval settings field <see cref="majorIntervals" />, it decides whether
        ///     the current instance is latest version, and whether the next version exist.
        /// </remarks>
        public static bool NextMajorVersion(UnityVersion ofMe, out UnityVersion next)
        {
            Ensure.Argument.NotNull(ofMe, nameof(ofMe));

            if (!majorIntervals.TryGetValue(ofMe.Cycle, out var majorInterval))
                return FirstMajorVersionOfNextCycle(ofMe, out next);

            var nextMajor = (byte)((ofMe.Major ?? 0) + 1);
            if (!majorInterval.Contains(nextMajor))
                return FirstMajorVersionOfNextCycle(ofMe, out next);

            next = new UnityVersion(ofMe.Cycle, nextMajor);
            return true;
        }

        public static bool FirstMajorVersionOfNextCycle(UnityVersion ofMe, out UnityVersion version)
        {
            Ensure.Argument.NotNull(ofMe, nameof(ofMe));

            if (NextCycleNumberOf(ofMe.Cycle, out ushort nextCycle))
            {
                var majorInterval = majorIntervals[nextCycle];
                version = new UnityVersion(nextCycle, majorInterval.Min);
                return true;
            }

            version = null;
            return false;
        }

        #endregion Related Versions

        #region Numbering Intervals

        public enum NumberingStyle
        {
            Traditional,
            DateBased
        }

        public static bool IsValidCycleNumber(ushort cycle) { return majorIntervals.TryGetValue(cycle, out _); }

        public static bool IsValidMajorNumber(ushort cycle, byte major)
        {
            return majorIntervals.TryGetValue(cycle, out ByteInterval majorInterval)
                && majorInterval.Contains(major);
        }

        /// <summary>
        ///     Extend major version interval of specified versioning cycle. This is useful when default version intervals are not
        ///     up to date.
        /// </summary>
        /// <param name="style">Numbering style of the specified <paramref name="cycle" /> number.</param>
        /// <param name="cycle">Cycle number of the major version interval to extend.</param>
        /// <param name="interval">Major version interval to extend to.</param>
        public static void ExtendMajorInterval(NumberingStyle style, ushort cycle, ByteInterval interval)
        {
            ExtendCycleInterval(style, cycle);

            if (majorIntervals.TryGetValue(cycle, out var existedInterval))
                interval = ByteInterval.Union(interval, existedInterval);
            majorIntervals[cycle] = interval;
        }

        /// <summary>
        ///     Mapping from cycle number to its corresponding major number interval, The collection contains all released
        ///     major versions of Unity.
        /// </summary>
        private static readonly Dictionary<ushort, ByteInterval>
            majorIntervals = new Dictionary<ushort, ByteInterval>();

        /// <summary>
        ///     Initialize contents of <see cref="majorIntervals" />.
        ///     The default values are come from Unity offical sites:
        ///     https://unity3d.com/cn/unity/whats-new/archive,
        ///     https://unity3d.com/cn/get-unity/download/archive,
        ///     https://unity3d.com/cn/unity/roadmap.
        /// </summary>
        private static void InitializeMajorIntervals()
        {
            ExtendMajorInterval(NumberingStyle.Traditional, 1, new ByteInterval(0, 6));
            ExtendMajorInterval(NumberingStyle.Traditional, 2, new ByteInterval(0, 6));
            ExtendMajorInterval(NumberingStyle.Traditional, 3, new ByteInterval(0, 5));
            ExtendMajorInterval(NumberingStyle.Traditional, 4, new ByteInterval(0, 7));
            ExtendMajorInterval(NumberingStyle.Traditional, 5, new ByteInterval(0, 6));
            ExtendMajorInterval(NumberingStyle.DateBased, 2017, new ByteInterval(1, 4));
            ExtendMajorInterval(NumberingStyle.DateBased, 2018, new ByteInterval(1, 3));
            ExtendMajorInterval(NumberingStyle.DateBased, 2019, new ByteInterval(1, 1));
        }

        #region Cycle

        public static ushort LatestCycle => GetCycleInterval(NumberingStyleTraits.ValueCount - 1).Max;

        public static NumberingStyle CycleNumberingStyleOf(ushort cycle)
        {
            for (int i = 0; i < cycleIntervals.Length; ++i)
            {
                if (GetCycleInterval(i).Contains(cycle))
                    return (NumberingStyle)i;
            }
            throw new ArgumentOutOfRangeException(nameof(cycle));
        }

        public static bool NextCycleNumberOf(ushort cycle, out ushort next)
        {
            int intervalIndex = -1;
            for (int i = 0; i < cycleIntervals.Length; ++i)
            {
                if (GetCycleInterval(i).Contains(cycle))
                {
                    intervalIndex = i;
                    break;
                }
            }

            if (intervalIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(cycle));

            var interval = GetCycleInterval(intervalIndex);
            next = (ushort)(cycle + 1);
            if (interval.Contains(next))
                return true;

            intervalIndex++;
            if (intervalIndex < cycleIntervals.Length)
            {
                next = GetCycleInterval(intervalIndex).Min;
                return true;
            }

            next = cycle;
            return false;
        }

        private static readonly UShortInterval?[] cycleIntervals = new UShortInterval?[NumberingStyleTraits.ValueCount];

        private static void ExtendCycleInterval(NumberingStyle style, ushort cycle)
        {
            var interval = cycleIntervals[(int)style] ?? new UShortInterval(cycle, cycle);
            interval.Extend(cycle);
            cycleIntervals[(int)style] = interval;
        }

        private static UShortInterval GetCycleInterval(NumberingStyle style) => GetCycleInterval((int)style);

        private static UShortInterval GetCycleInterval(int index)
        {
            var interval = cycleIntervals[index];
            if (interval == null)
                throw new InvalidOperationException("Cycle interval not initialized.");
            return interval.Value;
        }

        #endregion Cycle

        #endregion Numbering Intervals

        #region Predefined Instances

        public static readonly UnityVersion Unity_4_0 = new UnityVersion(4, 0);
        public static readonly UnityVersion Unity_4_1 = new UnityVersion(4, 1);
        public static readonly UnityVersion Unity_4_2 = new UnityVersion(4, 2);
        public static readonly UnityVersion Unity_4_3 = new UnityVersion(4, 3);
        public static readonly UnityVersion Unity_4_4 = new UnityVersion(4, 4);
        public static readonly UnityVersion Unity_4_5 = new UnityVersion(4, 5);
        public static readonly UnityVersion Unity_4_6 = new UnityVersion(4, 6);
        public static readonly UnityVersion Unity_4_7 = new UnityVersion(4, 7);

        public static readonly UnityVersion Unity_5_0 = new UnityVersion(5, 0);
        public static readonly UnityVersion Unity_5_1 = new UnityVersion(5, 1);
        public static readonly UnityVersion Unity_5_2 = new UnityVersion(5, 2);
        public static readonly UnityVersion Unity_5_3 = new UnityVersion(5, 3);
        public static readonly UnityVersion Unity_5_4 = new UnityVersion(5, 4);
        public static readonly UnityVersion Unity_5_5 = new UnityVersion(5, 5);
        public static readonly UnityVersion Unity_5_6 = new UnityVersion(5, 6);

        public static readonly UnityVersion Unity_2017_1 = new UnityVersion(2017, 1);
        public static readonly UnityVersion Unity_2017_2 = new UnityVersion(2017, 2);
        public static readonly UnityVersion Unity_2017_3 = new UnityVersion(2017, 3);
        public static readonly UnityVersion Unity_2017_4 = new UnityVersion(2017, 4);

        public static readonly UnityVersion Unity_2018_1 = new UnityVersion(2018, 1);
        public static readonly UnityVersion Unity_2018_2 = new UnityVersion(2018, 2);
        public static readonly UnityVersion Unity_2018_3 = new UnityVersion(2018, 3);

        public static readonly UnityVersion Unity_2019_1 = new UnityVersion(2019, 1);

        #endregion Predefined Instances

        static HistoricalUnityVersions() { InitializeMajorIntervals(); }
    }
}