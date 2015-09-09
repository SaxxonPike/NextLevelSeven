using System;
using System.Collections;
using System.Collections.Generic;

namespace NextLevelSeven.Cursors.Dividers
{
    abstract internal class StringDividerBase : IStringDivider
    {
        /// <summary>
        ///     This event is raised whenever the value is changed. This event does not propagate to the parent string divider.
        /// </summary>
        public event EventHandler ValueChanged;

        abstract public string this[int index] { get; set; }

        abstract public char[] BaseValue { get; }

        abstract public int Count { get; }

        /// <summary>
        ///     Get the delimiter character.
        /// </summary>
        public char Delimiter { get; protected set; }

        abstract public IReadOnlyList<StringDivision> Divisions { get; }

        /// <summary>
        ///     Index inside the parent.
        /// </summary>
        public int Index { get; set; }

        public readonly object SyncRoot = new object();

        abstract public string Value { get; set; }

        abstract public char[] ValueChars { get; set; }

        public int Version { get; protected set; }

        /// <summary>
        /// Delete descendant text.
        /// </summary>
        /// <param name="index">Index to delete.</param>
        public void Delete(int index)
        {
            var d = GetSubDivision(index);
            if (!d.Valid)
            {
                return;
            }

            Delete((d.Offset + d.Length >= BaseValue.Length)
                ? d
                : new StringDivision(d.Offset, d.Length + 1));
        }

        public abstract void Delete(StringDivision division);

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
        /// Raise the ValueChanged event.
        /// </summary>
        protected void RaiseValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }            
        }
    }
}
