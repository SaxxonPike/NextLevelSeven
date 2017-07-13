using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>A splitter which handles getting and setting delimited substrings within a parent string divider.</summary>
    internal sealed class DescendantStringDivider : StringDivider
    {
        /// <summary>Parent divider.</summary>
        private readonly StringDivider _baseDivider;

        /// <summary>[PERF] Cached divisions list.</summary>
        private List<StringDivision> _divisions;

        /// <summary>Create a subdivider for the specified string divider.</summary>
        /// <param name="baseDivider">Divider to reference.</param>
        /// <param name="delimiter">Delimiter to search for.</param>
        /// <param name="parentIndex">Index within the parent to reference.</param>
        public DescendantStringDivider(StringDivider baseDivider, char delimiter, int parentIndex)
        {
            _baseDivider = baseDivider;
            Index = parentIndex;
            Delimiter = delimiter;
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
                if (index >= Divisions.Count)
                {
                    return string.Empty;
                }

                var split = Divisions[index];
                return split.Length == 0 ? null : new string(BaseValue, split.Offset, split.Length);
            }
            set => SetValue(index, value);
        }

        public override bool IsNull => _baseDivider.IsNull || ValueChars == null || ValueChars.Length == 0;

        /// <summary>String that is operated upon, as a character array. This points to the parent divider's BaseValue.</summary>
        public override char[] BaseValue => _baseDivider.BaseValue;

        /// <summary>Get the number of divisions.</summary>
        public override int Count => Divisions.Count;

        /// <summary>Get the division offsets in the string.</summary>
        protected override List<StringDivision> Divisions
        {
            get
            {
                if (_divisions == null || Version != _baseDivider.Version)
                {
                    Update();
                }
                return _divisions;
            }
        }

        /// <summary>Calculated value of all divisions separated by delimiters.</summary>
        public override string Value
        {
            get
            {
                var d = _baseDivider.GetSubDivision(Index);
                return !d.Valid || d.Length == 0
                    ? null
                    : new string(BaseValue, d.Offset, d.Length);
            }
            set => _baseDivider[Index] = value;
        }

        /// <summary>Calculated value of all divisions separated by delimiters, as characters.</summary>
        public override char[] ValueChars
        {
            get
            {
                var d = _baseDivider.GetSubDivision(Index);
                return !d.Valid || d.Length == 0
                    ? null
                    : StringDividerOperations.CharSubstring(BaseValue, d.Offset, d.Length);
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                /* should not be called */
            }
        }

        /// <summary>Get or set the subdivided values.</summary>
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

        /// <summary>Set the value at the specified subdivision index.</summary>
        /// <param name="index">Index to set.</param>
        /// <param name="value">New value.</param>
        private void SetValue(int index, string value)
        {
            PadSubDivider(index);
            var d = Divisions[index];
            Replace(d.Offset, d.Length, StringDividerOperations.GetChars(value));
        }

        /// <summary>[PERF] Refresh internal division cache.</summary>
        private void Update()
        {
            Version = _baseDivider.Version;
            var baseDivision = _baseDivider.GetSubDivision(Index);
            if (baseDivision.Valid)
            {
                _divisions = StringDividerOperations.GetDivisions(BaseValue, Delimiter,
                    _baseDivider.GetSubDivision(Index));
            }
            else
            {
                _divisions = new List<StringDivision>();
            }
        }

        public override void Replace(int start, int length, char[] value)
        {
            _baseDivider.Replace(start, length, value);
            _divisions = null;
        }

        public override void Pad(char delimiter, int index, int start, int length, List<StringDivision> divisions)
        {
            _baseDivider.Pad(delimiter, index, start, length, divisions);
        }

        public override void PadSubDivider(int index)
        {
            _baseDivider.PadSubDivider(Index);
            var d = _baseDivider.GetSubDivision(Index);
            if (_divisions == null)
            {
                Update();
            }
            _baseDivider.Pad(Delimiter, index, d.Offset, d.Length, _divisions);
        }
    }
}