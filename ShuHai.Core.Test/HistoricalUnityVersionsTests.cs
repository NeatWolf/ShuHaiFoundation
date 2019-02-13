using System;
using NUnit.Framework;

namespace ShuHai.Tests
{
    using NumberingStyle = HistoricalUnityVersions.NumberingStyle;

    public class HistoricalUnityVersionsTests
    {
        [Test]
        public void NextMajorVersion()
        {
            NextMajorVersion(HistoricalUnityVersions.Unity_5_3, HistoricalUnityVersions.Unity_5_4);
            NextMajorVersion(HistoricalUnityVersions.Unity_5_6, HistoricalUnityVersions.Unity_2017_1);
            NextMajorVersion(HistoricalUnityVersions.Unity_2017_4, HistoricalUnityVersions.Unity_2018_1);
            NextMajorVersion(HistoricalUnityVersions.Unity_2018_2, HistoricalUnityVersions.Unity_2018_3);
        }

        private static void NextMajorVersion(UnityVersion current, UnityVersion next)
        {
            Assert.IsTrue(HistoricalUnityVersions.NextMajorVersion(current, out var actualNext));
            Assert.AreEqual(next, actualNext);
        }

        [Test]
        public void CycleNumberingStyleOf()
        {
            Assert.AreEqual(NumberingStyle.Traditional, HistoricalUnityVersions.CycleNumberingStyleOf(2));
            Assert.AreEqual(NumberingStyle.Traditional, HistoricalUnityVersions.CycleNumberingStyleOf(5));
            Assert.AreEqual(NumberingStyle.DateBased, HistoricalUnityVersions.CycleNumberingStyleOf(2017));
            Assert.AreEqual(NumberingStyle.DateBased, HistoricalUnityVersions.CycleNumberingStyleOf(2018));

            Assert.Throws<ArgumentOutOfRangeException>(() => HistoricalUnityVersions.CycleNumberingStyleOf(100));
            Assert.Throws<ArgumentOutOfRangeException>(() => HistoricalUnityVersions.CycleNumberingStyleOf(3211));
        }

        [Test]
        public void NextCycleNumberOf()
        {
            NextCycleNumberOf(3, 4, true);
            NextCycleNumberOf(5, 2017, true);
            NextCycleNumberOf(2017, 2018, true);

            ushort latestCycle = HistoricalUnityVersions.LatestCycle, futureCycle = (ushort)(latestCycle + 1);
            NextCycleNumberOf(latestCycle, futureCycle, false);
        }

        private static void NextCycleNumberOf(ushort cycle, ushort next, bool succeed)
        {
            bool actualSucceed = HistoricalUnityVersions.NextCycleNumberOf(cycle, out var actualNext);
            Assert.AreEqual(succeed, actualSucceed);
            if (actualSucceed)
                Assert.AreEqual(next, actualNext);
        }
    }
}