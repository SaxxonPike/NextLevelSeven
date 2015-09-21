using System;
using System.Collections.Generic;

namespace NextLevelSeven.Native.Dividers
{
    /// <summary>
    ///     Common string and char[] splitting operations used by StringDivider and StringSubDivider classes.
    /// </summary>
    internal static class StringDividerOperations
    {
        /// <summary>
        ///     Get a subset of a character array.
        /// </summary>
        /// <param name="s">Characters.</param>
        /// <param name="offset">Offset to start.</param>
        /// <returns>Extracted characters.</returns>
        public static char[] CharSubstring(char[] s, int offset)
        {
            return CharSubstring(s, offset, s.Length - offset);
        }

        /// <summary>
        ///     Get a subset of a character array.
        /// </summary>
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

        /// <summary>
        ///     Copy characters to a new array.
        /// </summary>
        /// <param name="s">Source.</param>
        /// <returns>Copied characters.</returns>
        public static char[] CopyChars(char[] s)
        {
            var length = s.Length;
            var result = new char[length];
            Array.Copy(s, result, length);
            return result;
        }

        /// <summary>
        ///     Attempts to convert a string to characters, or returns null if not possible.
        /// </summary>
        /// <param name="s">String to convert.</param>
        /// <returns>Converted characters.</returns>
        public static char[] GetChars(string s)
        {
            return (s == null) ? null : s.ToCharArray();
        }

        /// <summary>
        ///     Get divisions without bounds.
        /// </summary>
        /// <param name="s">Characters to parse.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <returns>Divisions.</returns>
        public static List<StringDivision> GetDivisions(char[] s, char delimiter)
        {
            return (s == null)
                ? new List<StringDivision>()
                : GetDivisions(s, delimiter, new StringDivision(0, s.Length));
        }

        /// <summary>
        ///     Get divisions within the specified division bounds.
        /// </summary>
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

                if (s == null)
                {
                    s = new char[0];
                }

                var offset = (!parent.Valid) ? 0 : parent.Offset;
                var inputLength = (!parent.Valid) ? s.Length : parent.Length;

                if (delimiter == '\0')
                {
                    divisions.Add(new StringDivision(offset, inputLength));
                    return divisions;
                }

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

        /// <summary>
        ///     Get a string, with the appropriate number of delimiters and divisions to be addressed up to the index.
        /// </summary>
        /// <param name="s">String to pad.</param>
        /// <param name="index">Index to pad to.</param>
        /// <param name="delimiter">Delimiter to pad with.</param>
        /// <param name="divisions">Output of the divisions list.</param>
        /// <returns>String that has been padded as necessary.</returns>
        public static char[] GetPaddedString(char[] s, int index, char delimiter, out List<StringDivision> divisions)
        {
            unchecked
            {
                if (s == null)
                {
                    s = new char[0];
                }

                if (delimiter == '\0')
                {
                    divisions = new List<StringDivision> {new StringDivision(0, s.Length)};
                    return s;
                }

                divisions = GetDivisions(s, delimiter);

                var divisionCount = divisions.Count;
                var stringLength = s.Length;
                var divisionsToAdd = (index - divisionCount) + 1;

                for (var i = 0; i < divisionsToAdd; i++)
                {
                    divisions.Add(new StringDivision(stringLength + 1, 0));
                    stringLength++;
                }

                if (divisionsToAdd > 0)
                {
                    var appendedOffset = s.Length;
                    var appendedValue = new char[appendedOffset + divisionsToAdd];
                    Array.Copy(s, appendedValue, s.Length);
                    for (var i = 0; i < divisionsToAdd; i++)
                    {
                        appendedValue[appendedOffset + i] = delimiter;
                    }
                    s = appendedValue;
                }

                return s;
            }
        }

        /// <summary>
        ///     Get the resulting string that has part of itself replaced with another string.
        /// </summary>
        /// <param name="s">String to replace within.</param>
        /// <param name="offset">Offset to start the replace.</param>
        /// <param name="length">Length in characters to replace.</param>
        /// <param name="replacement">String to replace with.</param>
        /// <returns></returns>
        public static char[] GetSplicedString(char[] s, int offset, int length, char[] replacement)
        {
            unchecked
            {
                if (s == null)
                {
                    return new char[0];
                }

                if (replacement == null)
                {
                    replacement = new char[0];
                }

                var preStringLength = offset;
                var postStringLength = s.Length - (length + offset);
                var replacementLength = replacement.Length;
                var totalLength = preStringLength + replacementLength + postStringLength;
                var result = new char[totalLength];

                if (preStringLength > 0)
                {
                    Array.Copy(s, 0, result, 0, preStringLength);
                }
                Array.Copy(replacement, 0, result, offset, replacementLength);
                if (postStringLength > 0)
                {
                    Array.Copy(s, offset + length, result, offset + replacementLength, postStringLength);
                }

                return result;
            }
        }

        /// <summary>
        ///     Join together character arrays to form a single character array.
        /// </summary>
        /// <param name="characters">Character arrays to join.</param>
        /// <returns>Joined character arrays.</returns>
        public static char[] JoinChars(params char[][] characters)
        {
            var totalLength = 0;
            var count = characters.Length;

            for (var i = 0; i < count; i++)
            {
                totalLength += characters[i].Length;
            }

            var result = new char[totalLength];
            var offset = 0;
            foreach (var ca in characters)
            {
                var length = ca.Length;
                Array.Copy(ca, 0, result, offset, length);
                offset += length;
            }

            return result;
        }

        /// <summary>
        ///     Join together character arrays to form a single character array, with delimiters.
        /// </summary>
        /// <param name="delimiter">Delimiter to use.</param>
        /// <param name="characters">Character arrays to join.</param>
        /// <returns>Joined character arrays.</returns>
        public static char[] JoinCharsWithDelimiter(char delimiter, params char[][] characters)
        {
            var totalLength = 0;
            var count = characters.Length;
            var delimitersToAdd = -1;

            for (var i = 0; i < count; i++)
            {
                totalLength += characters[i].Length;
                delimitersToAdd++;
            }

            var result = new char[totalLength];
            var offset = 0;
            foreach (var ca in characters)
            {
                var length = ca.Length;
                Array.Copy(ca, 0, result, offset, length);
                offset += length;
                if (delimitersToAdd > 0)
                {
                    result[offset++] = delimiter;
                    delimitersToAdd--;
                }
            }

            return result;
        }
    }
}