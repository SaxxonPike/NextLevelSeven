using System.Collections.Generic;

namespace NextLevelSeven.Cursors.Dividers
{
    /// <summary>
    ///     A splitter which handles getting and setting delimited substrings within a parent string divider.
    /// </summary>
    internal sealed class StringSubDivider : StringDividerBase
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
        ///     Get or set the substring at the specified index.
        /// </summary>
        /// <param name="index">Index of the string to get or set.</param>
        /// <returns>Substring.</returns>
        override public string this[int index]
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

                RaiseValueChanged();
            }
        }

        /// <summary>
        ///     String that is operated upon, as a character array. This points to the parent divider's BaseValue.
        /// </summary>
        override public char[] BaseValue
        {
            get { return BaseDivider.BaseValue; }
        }

        /// <summary>
        ///     Get the number of divisions.
        /// </summary>
        override public int Count
        {
            get { return Divisions.Count; }
        }

        /// <summary>
        /// Delete a division of text.
        /// </summary>
        /// <param name="division"></param>
        override public void Delete(StringDivision division)
        {
            BaseDivider.Delete(division);
        }

        /// <summary>
        ///     Get the division offsets in the string.
        /// </summary>
        override public IReadOnlyList<StringDivision> Divisions
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
        ///     Calculated value of all divisions separated by delimiters.
        /// </summary>
        override public string Value
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
        override public char[] ValueChars
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