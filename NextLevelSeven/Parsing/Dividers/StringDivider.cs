using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>Base class for string dividers.</summary>
    internal abstract class StringDivider
    {
        /// <summary>Get or set the string division at the specified index.</summary>
        /// <param name="index">Index of the string.</param>
        /// <returns></returns>
        public abstract string this[int index] { get; set; }

        /// <summary>Base value which cursors will operate on.</summary>
        public abstract char[] BaseValue { get; }

        /// <summary>Returns true if the base value is null.</summary>
        public abstract bool IsNull { get; }

        /// <summary>Return the number of string subdivisions.</summary>
        public abstract int Count { get; }

        /// <summary>Get the delimiter character.</summary>
        public char Delimiter { get; protected set; }

        /// <summary>Get the list of subdivisions.</summary>
        public abstract IReadOnlyList<StringDivision> Divisions { get; }

        /// <summary>Index inside the parent.</summary>
        public int Index { get; set; }

        /// <summary>Get the value of all subdivisions stitched together with the delimiter.</summary>
        public abstract string Value { get; set; }

        /// <summary>Get the value as a character array.</summary>
        public abstract char[] ValueChars { get; set; }

        /// <summary>Get the version number of the divider, which is incremented each time it changes.</summary>
        public int Version { get; protected set; }

        /// <summary>Get the subdivision values.</summary>
        public abstract IEnumerable<string> Values { get; set; }

        /// <summary>This event is raised whenever the value is changed. This event does not propagate to the parent string divider.</summary>
        public event EventHandler ValueChanged;

        /// <summary>Create a subdivision.</summary>
        /// <param name="index">Index of the subdivider in the parent divider.</param>
        /// <param name="delimiter">Delimiter to be used by the subdivider.</param>
        /// <returns>String subdivider.</returns>
        public StringDivider Divide(int index, char delimiter)
        {
            return new DescendantStringDivider(this, delimiter, index);
        }

        /// <summary>Get the subdivision in which this division's item at the specified index resides.</summary>
        /// <param name="index">Index of the item to get.</param>
        /// <returns>Subdivision location.</returns>
        public StringDivision GetSubDivision(int index)
        {
            if (index < 0)
            {
                return StringDivision.Invalid;
            }

            var d = Divisions;
            return (index >= d.Count) ? StringDivision.Invalid : d[index];
        }

        /// <summary>Raise the ValueChanged event.</summary>
        protected void RaiseValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
    }
}