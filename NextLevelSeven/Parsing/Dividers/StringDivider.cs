using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Parsing.Dividers
{
    // NOTE: use List instead of Dictionary, it's faster.

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
        protected abstract List<StringDivision> Divisions { get; }

        /// <summary>Index inside the parent.</summary>
        protected int Index { get; set; }

        /// <summary>Get the value of all subdivisions stitched together with the delimiter.</summary>
        public abstract string Value { get; set; }

        /// <summary>Get the value as a character array.</summary>
        public abstract char[] ValueChars { get; protected set; }

        /// <summary>Get the version number of the divider, which is incremented each time it changes.</summary>
        public int Version { get; protected set; }

        /// <summary>Get the subdivision values.</summary>
        public abstract IEnumerable<string> Values { get; set; }

        /// <summary>Get the subdivision in which this division's item at the specified index resides.</summary>
        /// <param name="index">Index of the item to get.</param>
        /// <returns>Subdivision location.</returns>
        public StringDivision GetSubDivision(int index)
        {
            var d = Divisions;
            return index >= d.Count
                ? StringDivision.Invalid
                : d[index];
        }

        public abstract void Replace(int start, int length, char[] value);

        public abstract void Pad(char delimiter, int index, int start, int length, List<StringDivision> divisions);

        public abstract void PadSubDivider(int index);

        public void Delete(int index)
        {
            if (index >= Divisions.Count)
            {
                return;
            }
            var d = Divisions[index];
            var offset = d.Offset;
            var length = d.Length;
            if (index > 0)
            {
                offset--;
                length++;
            }
            else
            {
                var endOfDivision = Divisions.Max(e => e.Offset);
                if (offset < endOfDivision)
                {
                    length++;
                }
            }
            Replace(offset, length, StringDividerOperations.EmptyChars);
        }

        public void Insert(int index, string value)
        {
            if (index >= Count)
            {
                this[index] = value;
            }
            else
            {
                this[index] = string.Concat(value, new string(Delimiter, 1), this[index] ?? string.Empty);                
            }
        }

        public void Move(int sourceIndex, int targetIndex)
        {
            var value = this[sourceIndex];

            Delete(sourceIndex);
            Insert(targetIndex, value);
        }
    }
}