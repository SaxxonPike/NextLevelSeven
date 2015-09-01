using System;
using System.Collections;
using System.Collections.Generic;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    ///     A splitter which handles getting and setting delimited substrings within a parent string divider.
    /// </summary>
    internal sealed class StringSubDivider : IStringDivider
    {
        /// <summary>
        ///     [PERF] Cached divisions list.
        /// </summary>
        private IReadOnlyList<StringDivision> _divisions;

        /// <summary>
        ///     Create a subdivider for the specified string divider.
        /// </summary>
        /// <param name="baseDivider">Divider to reference.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <param name="parentIndex">Index within the parent to reference.</param>
        public StringSubDivider(IStringDivider baseDivider, char delimiter, int parentIndex)
        {
            BaseDivider = baseDivider;
            Index = parentIndex;
            Delimiter = delimiter;
        }

        /// <summary>
        ///     Create a subdivider for the specified string divider.
        /// </summary>
        /// <param name="baseDivider">Divider to reference.</param>
        /// <param name="baseDividerOffset">Index of the character to use as the delimiter from the subdivided value.</param>
        /// <param name="parentIndex">Index within the parent to reference.</param>
        public StringSubDivider(IStringDivider baseDivider, int baseDividerOffset, int parentIndex)
        {
            BaseDivider = baseDivider;
            Index = parentIndex;
            Delimiter = baseDivider.Value[baseDividerOffset];
        }

        /// <summary>
        ///     Parent divider.
        /// </summary>
        private IStringDivider BaseDivider { get; set; }

        /// <summary>
        ///     This event is raised whenever the value is changed. This event does not propagate to the parent string divider.
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
                var splits = Divisions;
                if (index >= splits.Count || index < 0)
                {
                    return null;
                }

                var split = splits[index];
                return new string(BaseValue, split.Offset, split.Length);
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
                    ValueChars = (index > 0)
                        ? StringDividerOperations.JoinChars(paddedString, StringDividerOperations.GetChars(value))
                        : value.ToCharArray();
                }
                else
                {
                    var d = divisions[index];
                    ValueChars = StringDividerOperations.GetSplicedString(paddedString, d.Offset, d.Length,
                        StringDividerOperations.GetChars(value));
                }

                if (ValueChanged != null)
                {
                    ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///     String that is operated upon, as a character array. This points to the parent divider's BaseValue.
        /// </summary>
        public char[] BaseValue
        {
            get { return BaseDivider.BaseValue; }
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
        /// <returns>String subdivider.</returns>
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
                if (_divisions == null || Version != BaseDivider.Version)
                {
                    Update();
                }
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
                ? StringDivision.Invalid
                : d[index];
        }

        /// <summary>
        ///     Index inside the parent. This will always be zero for a StringDivider because it is a root divider.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Calculated value of all divisions separated by delimiters.
        /// </summary>
        public string Value
        {
            get
            {
                var d = BaseDivider.GetSubDivision(Index);
                return (!d.Valid)
                    ? null
                    : new string(BaseValue, d.Offset, d.Length);
            }
            set { BaseDivider[Index] = value; }
        }

        /// <summary>
        ///     Calculated value of all divisions separated by delimiters, as characters.
        /// </summary>
        public char[] ValueChars
        {
            get
            {
                var d = BaseDivider.GetSubDivision(Index);
                return (!d.Valid)
                    ? null
                    : StringDividerOperations.CharSubstring(BaseValue, d.Offset, d.Length);
            }
            set { BaseDivider[Index] = new string(value); }
        }

        public int Version { get; private set; }

        /// <summary>
        ///     Get the internal value as a string.
        /// </summary>
        /// <returns>Value as a string.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        ///     [PERF] Refresh internal division cache.
        /// </summary>
        private void Update()
        {
            Version = BaseDivider.Version;
            _divisions = StringDividerOperations.GetDivisions(BaseValue, Delimiter, BaseDivider.GetSubDivision(Index));
        }
    }
}