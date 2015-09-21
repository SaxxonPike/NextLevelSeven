using System.Collections;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    /// <summary>
    ///     An enumerator for implementations of IStringDivider.
    /// </summary>
    internal sealed class StringDividerEnumerator : IEnumerator<string>
    {
        /// <summary>
        ///     Create an enumerator for the specified IStringDivider.
        /// </summary>
        /// <param name="divider">Divider to enumerate over.</param>
        public StringDividerEnumerator(IStringDivider divider)
        {
            Divider = divider;
            Reset();
        }

        /// <summary>
        ///     Referenced string divider.
        /// </summary>
        private IStringDivider Divider { get; set; }

        /// <summary>
        ///     Currently selected index.
        /// </summary>
        private int Index { get; set; }

        /// <summary>
        ///     Currently selected item.
        /// </summary>
        public string Current
        {
            get
            {
                var division = Divider.Divisions[Index];
                return new string(Divider.ValueChars, division.Offset, division.Length);
            }
        }

        /// <summary>
        ///     Does nothing.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        ///     Currently selected item.
        /// </summary>
        object IEnumerator.Current
        {
            get { return Current; }
        }

        /// <summary>
        ///     Move to the next item. Returns true if successful, and false if there are no further items.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (Index >= Divider.Count - 1)
            {
                return false;
            }
            Index++;
            return true;
        }

        /// <summary>
        ///     Reset the enumerator to the beginning.
        /// </summary>
        public void Reset()
        {
            Index = -1;
        }
    }
}