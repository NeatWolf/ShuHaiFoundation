using System;
using NUnit.Framework;

namespace ShuHai.Tests
{
    public class UnityVersionTests
    {
        [Test]
        public void Parse()
        {
            Parse(New(2017), "Unity-2017");
            Parse(New(2017), "2017");
            Parse(New(5, 4), "5.4");
            Parse(New(2017, 4, 16), "2017.4.16");
            Parse(New(2017, 4, 16, UnityVersion.Type.Final), "2017.4.16f0");
            Parse(New(2017, 4, 16, UnityVersion.Type.Final, 2), "2017.4.16f2");
        }

        private static void Parse(UnityVersion obj, string text) { Assert.AreEqual(obj, UnityVersion.Parse(text)); }

        [Test]
        public new void ToString()
        {
            ToString("Unity-2017", New(2017), false);
            ToString("5", New(5), true);
            ToString("5.4", New(5, 4), true);
            ToString("2017.4.16", New(2017, 4, 16), true);
            ToString("2017.4.16a0", New(2017, 4, 16, UnityVersion.Type.Alpha), true);
            ToString("2017.4.16rc2", New(2017, 4, 16, UnityVersion.Type.ReleaseCandidate, 2), true);
        }

        private static void ToString(string text, UnityVersion obj, bool numberOnly)
        {
            Assert.AreEqual(text, obj.ToString(numberOnly));
        }

        [Test]
        public void Equals()
        {
            Equals(New(5, 6, 3, UnityVersion.Type.Patch, 1), New(5, 6, 3, UnityVersion.Type.Patch, 1), true);
            Equals(New(5, 6, 3, UnityVersion.Type.Patch, 1), New(5, 6, 3, UnityVersion.Type.Patch), false);
            Equals(New(5, 6, 3, UnityVersion.Type.Patch), New(5, 6, 3), false);
            Equals(New(5, 6, 3, null, 2), New(5, 6, 3), true);
            Equals(New(5, 6), New(5, 6, 0), false);
            Equals(New(2018, 1), New(5, 6), false);
            Equals(New(5, 6), null, false);
            Equals(null, New(5, 6), false);
        }

        private static void Equals(UnityVersion l, UnityVersion r, bool equals)
        {
            if (!ReferenceEquals(l, null))
                Assert.AreEqual(l.Equals(r), equals);
            if (!ReferenceEquals(r, null))
                Assert.AreEqual(r.Equals(l), equals);
        }

        [Test]
        public void CompareTo()
        {
            CompareTo(New(5, 5), New(5, 4, 2), 1);
            CompareTo(New(5, 5), New(5, 5), 0);
            CompareTo(New(5, 5), New(5, 5, 0), -1);
            CompareTo(New(5, 5), New(5, 5, 2), -1);
            CompareTo(New(5, 5), New(2017, 4, 16), -1);
            CompareTo(New(2017, 4, 16, UnityVersion.Type.Final), New(2017, 4, 16), 1);
            CompareTo(New(2017, 4, 16, UnityVersion.Type.Final), null, 1);
        }

        private static void CompareTo(UnityVersion l, UnityVersion r, int result)
        {
            if (!ReferenceEquals(l, null))
                Assert.AreEqual(Math.Sign(l.CompareTo(r)), Math.Sign(result));
            if (!ReferenceEquals(r, null))
                Assert.AreEqual(Math.Sign(r.CompareTo(l)), -Math.Sign(result));
        }

        private static UnityVersion New(ushort cycle, byte? major = null,
            byte? minor = null, UnityVersion.Type? releaseType = null, byte releaseNumber = 0)
        {
            return new UnityVersion(cycle, major, minor, releaseType, releaseNumber);
        }
    }
}