using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>A root-level splitter which handles getting and setting delimited substrings within a string.</summary>
    internal sealed class RootStringDivider : StringDivider
    {
        /// <summary>[PERF] Cached divisions list.</summary>
        private List<StringDivision> _divisions;

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
                var divisions = Divisions;
                if (index >= divisions.Count)
                {
                    return string.Empty;
                }
                var d = Divisions[index];
                return new string(ValueChars, d.Offset, d.Length);
            }
            set => SetValue(index, value);
        }

        /// <summary>String that is operated upon, as a character array.</summary>
        public override char[] BaseValue => ValueChars;

        /// <summary>Get the number of divisions.</summary>
        public override int Count => Divisions.Count;

        /// <summary>Get the division offsets in the string.</summary>
        protected override List<StringDivision> Divisions
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
            get => IsNull ? null : new string(ValueChars);
            set => ValueChars = StringDividerOperations.GetChars(value);
        }

        /// <summary>Calculated value of all divisions separated by delimiters, as chars.</summary>
        public override char[] ValueChars
        {
            get => _valueChars;
            protected set => Initialize(value);
        }

        /// <summary>Returns true if the divider base value is null.</summary>
        public override bool IsNull => _isNull;

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
            set => Value = Delimiter == '\0'
                ? string.Concat(value) 
                : string.Join(new string(Delimiter, 1), value);
        }

        /// <summary>Set the string value at the specified index.</summary>
        /// <param name="index">Index to change.</param>
        /// <param name="value">New value.</param>
        private void SetValue(int index, string value)
        {
            Pad(Delimiter, index, 0, _valueChars.Length, Divisions);
            var d = Divisions[index];
            Replace(d.Offset, d.Length, StringDividerOperations.GetChars(value));
        }

        /// <summary>Initialize the divider's value with the specified characters.</summary>
        /// <param name="s">Characters to set the value to.</param>
        private void Initialize(char[] s)
        {
            _divisions = null;
            _valueChars = s;
            _isNull = s == null || s.Length == 0;
            Version++;
        }

        public override void Pad(char delimiter, int index, int start, int length, List<StringDivision> divisions)
        {
            if (index == 0)
            {
                return;
            }

            var delimiterCount = 0;
            var end = start + length;
            for (var i = start; i < end; i++)
            {
                if (_valueChars[i] != delimiter)
                {
                    continue;
                }
                delimiterCount++;
                if (delimiterCount >= index)
                {
                    return;
                }
            }

            var delimitersToAdd = index - delimiterCount;
            var oldLength = _valueChars.Length;

            Array.Resize(ref _valueChars, oldLength + delimitersToAdd);
            if (oldLength > end)
            {
                Array.Copy(_valueChars, end, _valueChars, end + delimitersToAdd, oldLength - end);
            }
            if (divisions.Capacity < divisions.Count + delimitersToAdd)
            {
                divisions.Capacity = divisions.Count + delimitersToAdd;
            }
            while (delimitersToAdd > 0)
            {
                delimitersToAdd--;
                _valueChars[end++] = delimiter;
                divisions.Add(new StringDivision(end, 0));
            }
        }

        public override void Replace(int start, int length, char[] value)
        {
            if (value == null)
            {
                value = StringDividerOperations.EmptyChars;
            }

            var charsLength = _valueChars.Length;
            var valueLength = value.Length;
            var newLength = charsLength + (valueLength - length);
            var postLength = charsLength - (start + length);

            if (start >= charsLength)
            {
                newLength = start + valueLength;
                Array.Resize(ref _valueChars, newLength);
            }
            else if (length > valueLength)
            {
                if (postLength > 0)
                {
                    Array.Copy(_valueChars, start + length, _valueChars, start + valueLength, postLength);
                }
                Array.Resize(ref _valueChars, newLength);
            }
            else if (length < valueLength)
            {
                Array.Resize(ref _valueChars, newLength);
                if (postLength > 0)
                {
                    Array.Copy(_valueChars, start + length, _valueChars, start + valueLength, postLength);
                }
            }
            Array.Copy(value, 0, _valueChars, start, valueLength);
            _divisions = null;
            Version++;
        }

        public override void PadSubDivider(int index)
        {
            Pad(Delimiter, index, 0, _valueChars.Length, Divisions);
        }
    }
}