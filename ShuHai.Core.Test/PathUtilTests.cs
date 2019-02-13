using NUnit.Framework;

namespace ShuHai.Tests
{
    public class PathUtilTests
    {
        [Test]
        public void Normalize()
        {
            Normalize(@"\Data/Managed/", @"\Data\Managed\", '\\');
            Normalize(@"\Data\Managed\", @"/Data/Managed/", '/');
            Normalize(@"Develop\\\//Data/\/object.json", @"Develop/Data/object.json", '/');
            Normalize(@"Develop\\\//Data//\object.json", @"Develop\Data\object.json", '\\');
        }

        private static void Normalize(string before, string after, char slash)
        {
            Assert.AreEqual(after, PathUtil.Normalize(before, slash));
        }

        [Test]
        public void MakeRelative()
        {
            MakeRelative(@"D:\Workspace\Proj1\", @"D:\Workspace\Proj1\Program.cs", @"Program.cs");
            MakeRelative(@"D:\Workspace\Proj1\", @"D:\Workspace\Proj2\Program.cs", @"..\Proj2\Program.cs");
        }

        private static void MakeRelative(string from, string to, string relative)
        {
            Assert.AreEqual(relative, PathUtil.MakeRelative(from, to));
        }

        [Test]
        public void AreEqual()
        {
            AreEqual(@"Develop\\\//Data/\/object.json", @"Develop/Data/object.json", true);
            AreEqual(@"D:/Develop\\\//Data/\/object.json", @"Develop/Data/object.json", false);
            AreEqual(@"D:\Workspace\Proj1\..\properties.lua", @"D:\Workspace\properties.lua", true);
        }

        private static void AreEqual(string path1, string path2, bool result)
        {
            Assert.AreEqual(result, PathUtil.AreEqual(path1, path2));
        }
    }
}