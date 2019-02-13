using System;

namespace ShuHai
{
    public static class StringExtensions
    {
        public static bool Contains(this string self, string value, StringComparison comparisonType)
        {
            Ensure.Argument.NotNull(self, nameof(self));
            return self.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        ///     Get a string which is the first line of <paramref name="self" />.
        /// </summary>
        /// <returns> First line of <paramref name="self" />. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="self" /> is null. </exception>
        public static string FirstLine(this string self)
        {
            Ensure.Argument.NotNull(self, nameof(self));

            int lineFeedIndex = self.IndexOfAny(new[] { '\r', '\n' });
            return lineFeedIndex > 0 ? self.Substring(0, lineFeedIndex) : self;
        }

        public static string RemoveTail(this string self, int count)
        {
            Ensure.Argument.NotNull(self, nameof(self));

            int length = self.Length;
            Ensure.Argument.InRange(count, 0, length, nameof(count));

            return self.Remove(length - count, count);
        }
    }
}