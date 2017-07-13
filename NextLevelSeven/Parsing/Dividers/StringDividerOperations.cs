using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>Common string and char[] splitting operations used by StringDivider and StringSubDivider classes.</summary>
    internal static class StringDividerOperations
    {
        public static readonly char[] EmptyChars = new char[0];

        /// <summary>Get a subset of a character array.</summary>
        /// <param name="s">Characters.</param>
        /// <param name="offset">Offset to start.</param>
        /// <param name="length">Length of characters.</param>
        /// <returns>Extracted characters.</returns>
        public static char[] CharSubstring(char[] s, int offset, int length)
        {
            var result = new char[length];
            Array.Copy(s, offset, result, 0, length);
            return result;
        }

        /// <summary>Attempts to convert a string to characters, or returns null if not possible.</summary>
        /// <param name="s">String to convert.</param>
        /// <returns>Converted characters.</returns>
        public static char[] GetChars(string s)
        {
            return s?.ToCharArray();
        }

        /// <summary>Get divisions without bounds.</summary>
        /// <param name="s">Characters to parse.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <returns>Divisions.</returns>
        public static List<StringDivision> GetDivisions(char[] s, char delimiter)
        {
            return s == null
                ? new List<StringDivision>()
                : GetDivisions(s, delimiter, new StringDivision(0, s.Length));
        }

        /// <summary>Get divisions within the specified division bounds.</summary>
        /// <param name="s">Characters to parse.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <param name="parent">Bounds within which to search.</param>
        /// <returns>Divisions within the bounds specified.</returns>
        public static List<StringDivision> GetDivisions(char[] s, char delimiter, StringDivision parent)
        {
            unchecked
            {
                var divisions = new List<StringDivision>();
                var length = 0;

                var offset = !parent.Valid
                    ? 0
                    : parent.Offset;

                var inputLength = !parent.Valid
                    ? s.Length
                    : parent.Length;

                var endIndex = offset + inputLength;
                for (var index = offset; index < endIndex; index++)
                {
                    if (s[index] == delimiter)
                    {
                        divisions.Add(new StringDivision(offset, length));
                        length = 0;
                        offset = index + 1;
                    }
                    else
                    {
                        length++;
                    }
                }

                divisions.Add(new StringDivision(offset, length));
                return divisions;
            }
        }
    }
}