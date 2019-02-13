using System;
using System.IO;

namespace ShuHai.Unity.PackageSetup
{
    public static class Extensions
    {
        public static void CopyTo(this Stream self, Stream destination, int bufferSize = DefaultCopyBufferSize)
        {
            Ensure.Argument.NotNull(self, nameof(self));
            Ensure.Argument.NotNull(destination, nameof(destination));
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (!self.CanRead && !self.CanWrite)
                throw new ObjectDisposedException(null, "Stream is already disposed.");
            if (!destination.CanRead && !destination.CanWrite)
                throw new ObjectDisposedException(nameof(destination), "Destination stream is already disposed.");
            if (!self.CanRead)
                throw new NotSupportedException("Strema is unreadable.");
            if (!destination.CanWrite)
                throw new NotSupportedException("Destination is unwritable.");

            var buffer = new byte[bufferSize];
            int read;
            while ((read = self.Read(buffer, 0, buffer.Length)) != 0)
                destination.Write(buffer, 0, read);
        }

        private const int DefaultCopyBufferSize = 81920;
    }
}