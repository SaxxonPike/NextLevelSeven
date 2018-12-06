using System;
using System.Collections.Generic;
using System.Linq;

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
        private Memory<char> _valueChars;

        /// <summary>Create a divider for a specified string.</summary>
        /// <param name="s">String to divide.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        public RootStringDivider(IEnumerable<char> s, char delimiter)
        {
            Delimiter = delimiter;
            ValueChars = s?.ToArray() ?? new Memory<char>();
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
                return new string(ValueChars.Slice(d.Offset, d.Length).ToArray());
            }
            set => SetValue(index, value);
        }

        /// <summary>String that is operated upon, as a character array.</summary>
        public override ReadOnlyMemory<char> BaseValue => ValueChars;

        /// <summary>Get the number of divisions.</summary>
        public override int Count => Divisions.Count;

        /// <summary>Get the division offsets in the string.</summary>
        protected override List<StringDivision> Divisions
        {
            get
            {
                _divisions = _divisions ?? StringDividerOperations.GetDivisions(ValueChars.Span, Delimiter);
                return _divisions;
            }
        }

        /// <summary>Calculated value of all divisions separated by delimiters.</summary>
        public override string Value
        {
            get => IsNull ? null : new string(ValueChars.ToArray());
            set => ValueChars = new Memory<char>((value ?? string.Empty).ToCharArray());
        }

        /// <summary>Calculated value of all divisions separated by delimiters, as chars.</summary>
        private Memory<char> ValueChars
        {
            get => _valueChars;
            set => Initialize(value);
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
            Replace(d.Offset, d.Length, value.AsSpan());
        }

        /// <summary>Initialize the divider's value with the specified characters.</summary>
        /// <param name="s">Characters to set the value to.</param>
        private void Initialize(Memory<char> s)
        {
            _divisions = null;
            _valueChars = s;
            _isNull = s.Length == 0;
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
            var valueChars = _valueChars.Span;
            
            for (var i = start; i < end; i++)
            {
                if (valueChars[i] != delimiter)
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
            if (delimitersToAdd <= 0)
                return;
            
            var oldLength = _valueChars.Length;
            var newLength = oldLength + delimitersToAdd;
            if (oldLength != newLength)
            {
                var newChars = new char[oldLength + delimitersToAdd];
                var newMemory = new Memory<char>(newChars);
                _valueChars.Slice(0, end).CopyTo(newMemory);
                _valueChars.Slice(end).CopyTo(newMemory.Slice(end + delimitersToAdd));
                _valueChars = newMemory;                
                valueChars = _valueChars.Span;
            }

            if (divisions.Capacity < divisions.Count + delimitersToAdd)
                divisions.Capacity = divisions.Count + delimitersToAdd;
            
            while (delimitersToAdd > 0)
            {
                delimitersToAdd--;
                valueChars[end++] = delimiter;
                divisions.Add(new StringDivision(end, 0));
            }
        }

        public override void Replace(int start, int length, ReadOnlySpan<char> value)
        {
            if (value == null)
                value = StringDividerOperations.EmptyChars;

            var charsLength = _valueChars.Length;
            var valueLength = value.Length;
            var newLength = charsLength + (valueLength - length);
            var postLength = charsLength - (start + length);

            if (length != value.Length)
            {
                var newChars = new char[newLength];
                var newMemory = new Memory<char>(newChars);
            
                if (start > 0)
                    _valueChars.Slice(0, start).CopyTo(newMemory);
            
                if (postLength > 0)
                    _valueChars.Slice(start + length).CopyTo(newMemory.Slice(start + valueLength));
                
                value.CopyTo(newMemory.Slice(start).Span);
                _valueChars = newMemory;
            }
            else
            {
                value.CopyTo(_valueChars.Span.Slice(start, length));
            }
            
            _divisions = null;
            Version++;
        }

        public override void PadSubDivider(int index)
        {
            Pad(Delimiter, index, 0, _valueChars.Length, Divisions);
        }
    }
}