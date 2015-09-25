using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>A root-level splitter which handles getting and setting delimited substrings within a string.</summary>
    internal sealed class RootStringDivider : StringDivider
    {
        /// <summary>[PERF] Cached divisions list.</summary>
        private IReadOnlyList<StringDivision> _divisions;

        /// <summary>Internally keep track if the base value is null.</summary>
        private bool _isNull;

        /// <summary>Internal character store.</summary>
        private char[] _valueChars;

        /// <summary>Create a divider for a specified string.</summary>
        /// <param name="s">String to divide.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        public RootStringDivider(string s, char delimiter)
        {
            Delimiter = delimiter;
            ValueChars = StringDividerOperations.GetChars(s);
        }

        /// <summary>Get or set the substring at the specified index.</summary>
        /// <param name="index">Index of the string to get or set.</param>
        /// <returns>Substring.</returns>
        public override string this[int index]
        {
            get
            {
                if (IsNull)
                {
                    return null;
                }
                if (index < 0 || index >= Count)
                {
                    return null;
                }
                var d = Divisions[index];
                return new string(ValueChars, d.Offset, d.Length);
            }
            set { SetValue(index, value); }
        }

        /// <summary>String that is operated upon, as a character array.</summary>
        public override char[] BaseValue
        {
            get { return ValueChars; }
        }

        /// <summary>Get the number of divisions.</summary>
        public override int Count
        {
            get { return Divisions.Count; }
        }

        /// <summary>Get the division offsets in the string.</summary>
        public override IReadOnlyList<StringDivision> Divisions
        {
            get
            {
                _divisions = _divisions ?? StringDividerOperations.GetDivisions(ValueChars, Delimiter);
                return _divisions;
            }
        }

        /// <summary>Calculated value of all divisions separated by delimiters.</summary>
        public override string Value
        {
            get { return IsNull ? null : new string(ValueChars); }
            set { ValueChars = StringDividerOperations.GetChars(value); }
        }

        /// <summary>Calculated value of all divisions separated by delimiters, as chars.</summary>
        public override char[] ValueChars
        {
            get { return _valueChars; }
            set { Initialize(value); }
        }

        /// <summary>Returns true if the divider base value is null.</summary>
        public override bool IsNull
        {
            get { return _isNull; }
        }

        /// <summary>Get the internal values.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = Divisions.Count;
                for (var i = 0; i < count; i++)
                {
                    yield return this[i];
                }
            }
            set { Value = (Delimiter == '\0') ? string.Concat(value) : string.Join(new string(Delimiter, 1), value); }
        }

        /// <summary>Set the string value at the specified index.</summary>
        /// <param name="index">Index to change.</param>
        /// <param name="value">New value.</param>
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

        /// <summary>Initialize the divider's value with the specified characters.</summary>
        /// <param name="s">Characters to set the value to.</param>
        private void Initialize(char[] s)
        {
            _divisions = null;
            _valueChars = s;
            _isNull = ValueChars == null;
            Version++;

            RaiseValueChanged();
        }

        /// <summary>Get the internal value as a string.</summary>
        /// <returns>Value as a string.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}