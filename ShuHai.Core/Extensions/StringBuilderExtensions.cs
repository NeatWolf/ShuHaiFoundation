using System;
using System.Text;

namespace ShuHai
{
    public static class StringBuilderExtensions
    {
        #region Remove

        /// <summary>
        ///     Remove specified number of tail characters.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to remove from. </param>
        /// <param name="count"> Number of characters to remove. </param>
        public static StringBuilder RemoveTail(this StringBuilder self, int count)
        {
            Ensure.Argument.NotNull(self, nameof(self));

            int len = self.Length;
            Ensure.Argument.InRange(count, 0, len, nameof(count));

            self.Remove(len - count, count);
            return self;
        }

        /// <summary>
        ///     Remove all tail line feed characters.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to remove from. </param>
        public static StringBuilder RemoveTailLineFeed(this StringBuilder self)
        {
            Ensure.Argument.NotNull(self, nameof(self));

            int length = self.Length, lastIndex = length - 1;
            if (length > 0 && self[lastIndex] == '\n')
            {
                int nextToLastIndex = lastIndex - 1;
                if (self[nextToLastIndex] == '\r') // The line feed is "\r\n"
                    self.Remove(nextToLastIndex, 2);
                else
                    self.Remove(lastIndex, 1);
            }
            return self;
        }

        #endregion

        #region Indented Append

        /// <summary>
        ///     Appends a character with indentation which defined by specified indent level and unit.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to append to. </param>
        /// <param name="value"> The character to append. </param>
        /// <param name="indentLevel"> Number of unit string to append. </param>
        /// <param name="indentUnit"> Unit string of each indent level. </param>
        /// <returns> The <paramref name="self" /> instance. </returns>
        public static StringBuilder IndentedAppend(
            this StringBuilder self, char value, byte indentLevel, string indentUnit)
        {
            if (indentLevel == 0)
                return self.Append(value);

            self.AppendHeadIndent(indentLevel, indentUnit);

            char last = self[self.Length - 1];
            if (last == '\n' && value != '\n')
                self.AppendIndent(indentLevel, indentUnit);
            self.Append(value);
            return self;
        }

        /// <summary>
        ///     Appends an array of characters with indentation which defined by specified indent level and unit.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to append to. </param>
        /// <param name="value"> The array of characters to append. </param>
        /// <param name="startIndex"> The starting position in <paramref name="value" />. </param>
        /// <param name="charCount"> The number of characters to append. </param>
        /// <param name="indentLevel"> Number of unit string to append. </param>
        /// <param name="indentUnit"> Unit string of each indent level. </param>
        /// <returns> The <paramref name="self" /> instance. </returns>
        public static StringBuilder IndentedAppend(this StringBuilder self,
            char[] value, int startIndex, int charCount, byte indentLevel, string indentUnit)
        {
            if (indentLevel == 0)
                return self.Append(value, startIndex, charCount);

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount));

            if (value == null)
            {
                if (startIndex == 0 && charCount == 0)
                    return self;
                throw new ArgumentNullException(nameof(value));
            }

            if (charCount > value.Length - startIndex)
                throw new ArgumentOutOfRangeException(nameof(charCount));

            if (charCount == 0)
                return self;

            self.AppendHeadIndent(indentLevel, indentUnit);

            int count = value.Length;
            int endIndex = startIndex + charCount;
            for (int i = startIndex; i < endIndex; ++i)
            {
                var c = value[i];
                self.Append(c);
                if (c == '\n' && i != count - 1)
                    self.AppendIndent(indentLevel, indentUnit);
            }
            return self;
        }

        /// <summary>
        ///     Appends an array of characters with indentation which defined by specified indent level and unit.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to append to. </param>
        /// <param name="value"> The array of characters to append. </param>
        /// <param name="indentLevel"> Number of unit string to append. </param>
        /// <param name="indentUnit"> Unit string of each indent level. </param>
        /// <returns> The <paramref name="self" /> instance. </returns>
        public static StringBuilder IndentedAppend(
            this StringBuilder self, char[] value, byte indentLevel, string indentUnit)
        {
            if (indentLevel == 0)
                return self.Append(value);

            if (CollectionUtil.IsNullOrEmpty(value))
                return self;

            self.AppendHeadIndent(indentLevel, indentUnit);

            int count = value.Length;
            for (int i = 0; i < count; ++i)
            {
                var c = value[i];
                self.Append(c);
                if (c == '\n' && i != count - 1)
                    self.AppendIndent(indentLevel, indentUnit);
            }
            return self;
        }

        /// <summary>
        ///     Appends a string with indentation which defined by specified indent level and unit.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to append to. </param>
        /// <param name="value"> The string to append. </param>
        /// <param name="indentLevel"> Number of unit string to append. </param>
        /// <param name="indentUnit"> Unit string of each indent level. </param>
        /// <returns> The <paramref name="self" /> instance. </returns>
        public static StringBuilder IndentedAppend(
            this StringBuilder self, string value, byte indentLevel, string indentUnit)
        {
            if (indentLevel == 0)
                return self.Append(value);

            if (string.IsNullOrEmpty(value))
                return self;

            self.AppendHeadIndent(indentLevel, indentUnit);

            int count = value.Length;
            for (int i = 0; i < count; ++i)
            {
                var c = value[i];
                self.Append(c);
                if (c == '\n' && i != count - 1)
                    self.AppendIndent(indentLevel, indentUnit);
            }
            return self;
        }

        /// <summary>
        ///     Appends an indent string combined by specified level and unit if the <see cref="StringBuilder" /> is empty.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to append to. </param>
        /// <param name="indentLevel"> Number of unit string to append. </param>
        /// <param name="indentUnit"> Unit string of each indent level. </param>
        /// <returns> The <paramref name="self" /> instance. </returns>
        public static StringBuilder AppendHeadIndent(this StringBuilder self, byte indentLevel, string indentUnit)
        {
            if (self.Length == 0 || self[self.Length - 1] == '\n')
                self.AppendIndent(indentLevel, indentUnit);
            return self;
        }

        /// <summary>
        ///     Appends an indent string combined by specified level and unit.
        /// </summary>
        /// <param name="self"> The <see cref="StringBuilder" /> to append to. </param>
        /// <param name="indentLevel"> Number of unit string to append. </param>
        /// <param name="indentUnit"> Unit string of each indent level. </param>
        /// <returns> The <paramref name="self" /> instance. </returns>
        public static StringBuilder AppendIndent(this StringBuilder self, byte indentLevel, string indentUnit)
        {
            for (int i = 0; i < indentLevel; ++i)
                self.Append(indentUnit);
            return self;
        }

        #endregion Indentation
    }
}