using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>Common string and char[] splitting operations used by StringDivider and StringSubDivider classes.</summary>
    internal static class StringDividerOperations
    {
        public static readonly char[] EmptyChars = new char[0];

        /// <summary>Get divisions without bounds.</summary>
        /// <param name="s">Characters to parse.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <returns>Divisions.</returns>
        public static List<StringDivision> GetDivisions(ReadOnlySpan<char> s, char delimiter)
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
        public static List<StringDivision> GetDivisions(ReadOnlySpan<char> s, char delimiter, StringDivision parent)
        {
            unchecked
            {
                var length = 0;

                var offset = !parent.Valid
                    ? 0
                    : parent.Offset;

                var inputLength = !parent.Valid
                    ? s.Length
                    : parent.Length;

                var endIndex = offset + inputLength;

                // Precalculate the number of delimiters (perf)
                var delimiterCount = 0;
                for (var index = offset; index < endIndex; index++)
                    if (s[index] == delimiter)
                        delimiterCount++;
                
                var divisions = new List<StringDivision>(delimiterCount);

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