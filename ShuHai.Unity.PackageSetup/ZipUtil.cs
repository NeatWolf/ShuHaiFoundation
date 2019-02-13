using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace ShuHai.Unity.PackageSetup
{
    public static class ZipUtil
    {
        /// <summary>
        ///     Unpack zip archive at specified path.
        /// </summary>
        /// <param name="zipPath">Full path of the zip archive to unpack.</param>
        /// <param name="outputPath">Directory path of the unpacked files.</param>
        /// <param name="replaceIfExisted"></param>
        public static int Unzip(string zipPath, string outputPath, bool replaceIfExisted)
        {
            Ensure.Argument.NotNull(zipPath, nameof(zipPath));
            Ensure.Argument.NotNull(outputPath, nameof(outputPath));
            if (!File.Exists(zipPath))
                throw new FileNotFoundException("Archive not found.", zipPath);

            Directory.CreateDirectory(outputPath);

            using (var archiveStream = new FileStream(zipPath, FileMode.Open))
            using (var zipFile = new ZipFile(archiveStream))
            {
                int fileCount = 0;
                foreach (ZipEntry entry in zipFile)
                {
                    var entryPath = Path.Combine(outputPath, entry.Name);
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(entryPath);
                    }
                    else if (entry.IsFile)
                    {
                        if (!File.Exists(entryPath) || replaceIfExisted)
                        {
                            using (var entryFileExtractor = zipFile.GetInputStream(entry))
                            using (var entryFileWriter = new FileStream(entryPath, FileMode.Create))
                                entryFileExtractor.CopyTo(entryFileWriter, 4096);
                            fileCount++;
                        }
                    }
                }
                return fileCount;
            }
        }
    }
}