using System.Collections.Generic;

namespace NextLevelSeven.Native.Dividers
{
    /// <summary>
    ///     A root-level splitter which handles getting and setting delimited substrings within a string.
    /// </summary>
    internal sealed class StringDivider : StringDividerBase
    {
        /// <summary>
        ///     [PERF] Cached divisions list.
        /// </summary>
        private IReadOnlyList<StringDivision> _divisions;

        private char[] _valueChars;

        /// <summary>
        ///     Create a divider for a specified string.
        /// </summary>
        /// <param name="s">String to divide.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        public StringDivider(string s, char delimiter)
        {
            Delimiter = delimiter;
            ValueChars = StringDividerOperations.GetChars(s);
        }

        /// <summary>
        ///     Get or set the substring at the specified index.
        /// </summary>
        /// <param name="index">Index of the string to get or set.</param>
        /// <returns>Substring.</returns>
        public override string this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    return null;
                }
                if (ValueChars == null)
                {
                    return null;
                }
                var d = Divisions[index];
                return new string(ValueChars, d.Offset, d.Length);
            }
            set { SetValue(index, value); }
        }

        /// <summary>
        ///     String that is operated upon, as a character array.
        /// </summary>
        public override char[] BaseValue
        {
            get { return ValueChars; }
        }

        /// <summary>
        ///     Get the number of divisions.
        /// </summary>
        public override int Count
        {
            get { return Divisions.Count; }
        }

        /// <summary>
        ///     Get the division offsets in the string.
        /// </summary>
        public override IReadOnlyList<StringDivision> Divisions
        {
            get
            {
                _divisions = _divisions ?? StringDividerOperations.GetDivisions(ValueChars, Delimiter);
                return _divisions;
            }
        }

        /// <summary>
        ///     Calculated value of all divisions separated by delimiters.
        /// </summary>
        public override string Value
        {
            get { return new string(ValueChars); }
            set { ValueChars = StringDividerOperations.GetChars(value); }
        }

        /// <summary>
        ///     Calculated value of all divisions separated by delimiters, as chars.
        /// </summary>
        public override char[] ValueChars
        {
            get { return _valueChars; }
            set { Initialize(value); }
        }

        private void SetValue(int index, string value)
        {
            if (index < 0)
            {
                return;
            }

            List<StringDivision> divisions;
            var paddedString = StringDividerOperations.GetPaddedString(ValueChars, index, Delimiter, out divisions);
            if (index >= divisions.Count)
            {
                Initialize((index >= divisions.Count)
                    ? StringDividerOperations.JoinCharsWithDelimiter(Delimiter, paddedString,
                        StringDividerOperations.GetChars(value))
                    : StringDividerOperations.GetChars(value));
            }
            else
            {
                var d = divisions[index];
                Initialize(StringDividerOperations.GetSplicedString(paddedString, d.Offset, d.Length,
                    StringDividerOperations.GetChars(value)));
            }
        }

        /// <summary>
        ///     Delete a division of text.
        /// </summary>
        /// <param name="division"></param>
        public override void Delete(StringDivision division)
        {
            ValueChars = StringDividerOperations.GetSplicedString(ValueChars, division.Offset, division.Length,
                new char[0]);
        }

        /// <summary>
        ///     Initialize the divider's value with the specified characters.
        /// </summary>
        /// <param name="s">Characters to set the value to.</param>
        private void Initialize(char[] s)
        {
            lock (SyncRoot)
            {
                _divisions = null;
                _valueChars = s;
                Version++;
            }

            RaiseValueChanged();
        }

        /// <summary>
        ///     Get the internal value as a string.
        /// </summary>
        /// <returns>Value as a string.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}