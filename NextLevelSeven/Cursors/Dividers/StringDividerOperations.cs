using System.Collections.Generic;
using System.Text;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    /// Common string and char[] splitting operations used by StringDivider and StringSubDivider classes.
    /// </summary>
    static internal class StringDividerOperations
    {
        /// <summary>
        /// Get divisions without bounds.
        /// </summary>
        /// <param name="s">Characters to parse.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <returns>Divisions.</returns>
        static public List<StringDivision> GetDivisions(char[] s, char delimiter)
        {
            return (s == null)
                ? new List<StringDivision>()
                : GetDivisions(s, delimiter, new StringDivision(0, s.Length));
        }

        /// <summary>
        /// Get divisions within the specified division bounds.
        /// </summary>
        /// <param name="s">Characters to parse.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <param name="parent">Bounds within which to search.</param>
        /// <returns>Divisions within the bounds specified.</returns>
        static public List<StringDivision> GetDivisions(char[] s, char delimiter, StringDivision parent)
        {
            unchecked
            {
                var divisions = new List<StringDivision>();
                var length = 0;

                if (s == null)
                {
                    s = new char[0];
                }

                var offset = (parent == null) ? 0 : parent.Offset;
                var inputLength = (parent == null) ? s.Length : parent.Length;

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
        /// Get a string, with the appropriate number of delimiters and divisions to be addressed up to the index.
        /// </summary>
        /// <param name="s">String to pad.</param>
        /// <param name="index">Index to pad to.</param>
        /// <param name="delimiter">Delimiter to pad with.</param>
        /// <param name="divisions">Output of the divisions list.</param>
        /// <returns>String that has been padded as necessary.</returns>
        static public string GetPaddedString(string s, int index, char delimiter, out List<StringDivision> divisions)
        {
            unchecked
            {
                if (s == null)
                {
                    s = string.Empty;
                }

                divisions = GetDivisions(s.ToCharArray(), delimiter);

                if (delimiter == '\0')
                {
                    return s;
                }

                var divisionCount = divisions.Count;
                var stringLength = s.Length;
                var builder = new StringBuilder(s);
                var divisionsToAdd = (index - divisionCount) + 1;

                for (var i = 0; i < divisionsToAdd; i++)
                {
                    divisions.Add(new StringDivision(stringLength + 1, 0));
                    stringLength++;
                }

                if (divisionsToAdd > 0)
                {
                    builder.Append(new string(delimiter, divisionsToAdd));
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Get the resulting string that has part of itself replaced with another string.
        /// </summary>
        /// <param name="s">String to replace within.</param>
        /// <param name="offset">Offset to start the replace.</param>
        /// <param name="length">Length in characters to replace.</param>
        /// <param name="replacement">String to replace with.</param>
        /// <returns></returns>
        static public string GetSplicedString(string s, int offset, int length, string replacement)
        {
            unchecked
            {
                var builder = new StringBuilder();
                if (offset > 0)
                {
                    builder.Append(s.Substring(0, offset));
                }
                builder.Append(replacement);
                if (s != null && (offset + length < s.Length))
                {
                    builder.Append(s.Substring(offset + length));
                }
                return builder.ToString();
            }
        }
    }
}
