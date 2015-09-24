using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    internal abstract class StringDividerBase : IStringDivider
    {
        /// <summary>
        ///     This event is raised whenever the value is changed. This event does not propagate to the parent string divider.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        ///     Get or set the string division at the specified index.
        /// </summary>
        /// <param name="index">Index of the string.</param>
        /// <returns></returns>
        public abstract string this[int index] { get; set; }

        public abstract char[] BaseValue { get; }

        public abstract bool IsNull { get; }

        public abstract int Count { get; }

        /// <summary>
        ///     Get the delimiter character.
        /// </summary>
        public char Delimiter { get; protected set; }

        public abstract IReadOnlyList<StringDivision> Divisions { get; }

        /// <summary>
        ///     Index inside the parent.
        /// </summary>
        public int Index { get; set; }

        public abstract string Value { get; set; }

        public abstract char[] ValueChars { get; set; }

        public int Version { get; protected set; }

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
        ///     Raise the ValueChanged event.
        /// </summary>
        protected void RaiseValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }

        public abstract IEnumerable<string> Values { get; set; }
    }
}