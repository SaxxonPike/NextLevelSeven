using System;
using System.Collections;
using System.Collections.Generic;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    ///     A root-level splitter which handles getting and setting delimited substrings within a string.
    /// </summary>
    internal sealed class StringDivider : IStringDivider
    {
        public readonly object SyncRoot = new object();

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
        ///     This event is raised whenever the value is changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        ///     Get or set the substring at the specified index.
        /// </summary>
        /// <param name="index">Index of the string to get or set.</param>
        /// <returns>Substring.</returns>
        public string this[int index]
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
            set
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
        }

        /// <summary>
        ///     String that is operated upon, as a character array.
        /// </summary>
        public char[] BaseValue
        {
            get { return ValueChars; }
        }

        /// <summary>
        ///     Get the number of divisions.
        /// </summary>
        public int Count
        {
            get { return Divisions.Count; }
        }

        /// <summary>
        ///     Get the delimiter character.
        /// </summary>
        public char Delimiter { get; private set; }

        /// <summary>
        ///     Create a subdivision.
        /// </summary>
        /// <param name="index">Index of the subdivider in the parent divider.</param>
        /// <param name="delimiter">Delimiter to be used by the subdivider.</param>
        /// <returns></returns>
        public IStringDivider Divide(int index, char delimiter)
        {
            return new StringSubDivider(this, delimiter, index);
        }

        /// <summary>
        ///     Get the division offsets in the string.
        /// </summary>
        public IReadOnlyList<StringDivision> Divisions
        {
            get
            {
                _divisions = _divisions ?? StringDividerOperations.GetDivisions(ValueChars, Delimiter);
                return _divisions;
            }
        }

        /// <summary>
        ///     Get an enumerator for divided strings.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return new StringDividerEnumerator(this);
        }

        /// <summary>
        ///     Get an enumerator for divided strings.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new StringDividerEnumerator(this);
        }

        /// <summary>
        ///     Get the subdivision in which this division's item at the specified index resides.
        /// </summary>
        /// <param name="index">Index of the item to get.</param>
        /// <returns>Subdivision location.</returns>
        public StringDivision GetSubDivision(int index)
        {
            if (index < 0)
            {
                return StringDivision.Invalid;
            }

            var d = Divisions;
            return (index >= d.Count)
                ? new StringDivision(ValueChars.Length, 0)
                : d[index];
        }

        /// <summary>
        ///     Index inside the parent. This will always be zero for a StringDivider because it is a root divider.
        /// </summary>
        public int Index
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        ///     Calculated value of all divisions separated by delimiters.
        /// </summary>
        public string Value
        {
            get { return new string(ValueChars); }
            set { ValueChars = StringDividerOperations.GetChars(value); }
        }

        /// <summary>
        ///     Calculated value of all divisions separated by delimiters, as chars.
        /// </summary>
        public char[] ValueChars
        {
            get { return _valueChars; }
            set { Initialize(value); }
        }


        /// <summary>
        ///     Version number of the value. Each time the value is changed, this increments to incidate it has changed.
        /// </summary>
        public int Version { get; private set; }

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

            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
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