using NUnit.Framework;

namespace ShuHai.Tests
{
    using IntInterval = Interval<int>;

    public class IntervalTests
    {
        [Test]
        public void Union()
        {
            Union(1, 4, 5, 8, 1, 8);
            Union(1, 4, 2, 5, 1, 5);
            Union(1, 4, 2, 3, 1, 4);
            Union(1, 4, -2, 0, -2, 4);
            Union(1, 4, -2, 3, -2, 4);
        }

        private static void Union(int i1Min, int i1Max, int i2Min, int i2Max, int rMin, int rMax)
        {
            IntInterval i1 = new IntInterval(i1Min, i1Max), i2 = new IntInterval(i2Min, i2Max);
            Assert.AreEqual(new IntInterval(rMin, rMax), IntInterval.Union(i1, i2));
        }

        [Test]
        public void Contains()
        {
            var interval = new IntInterval(1, 4);

            Assert.IsTrue(interval.Contains(2));
            Assert.IsTrue(interval.Contains(1));
            Assert.IsTrue(interval.Contains(4));
            Assert.IsTrue(interval.Contains(4));

            Assert.IsTrue(interval.Contains(2, false));
            Assert.IsFalse(interval.Contains(1, false));
            Assert.IsFalse(interval.Contains(4, false));
        }
    }
}