using System;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Utility
{
    /// <summary>
    /// An IEnumerable+IIndexable wrapper around a method set, using a numeric index.
    /// </summary>
    /// <typeparam name="TItem">Type of contained items.</typeparam>
    sealed internal class WrapperEnumerable<TItem> : IEnumerableIndexable<int, TItem>
    {
        /// <summary>
        /// Create a wrapper.
        /// </summary>
        /// <param name="read">Function to read values at a specified index.</param>
        /// <param name="write">Function to write values at a specified index.</param>
        /// <param name="count">Function to get the number of items contained.</param>
        /// <param name="startIndex">Index where items begin. Defaults to zero.</param>
        internal WrapperEnumerable(Func<int, TItem> read, Action<int, TItem> write, Func<int> count, int startIndex = 0)
        {
            _count = count;
            _read = read;
            _startIndex = startIndex;
            _write = write;
        }

        private readonly Func<int> _count;
        private readonly Func<int, TItem> _read;
        private readonly int _startIndex;
        private readonly Action<int, TItem> _write;

        /// <summary>
        /// Get the enumerator for this enumerable wrapper.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return Enumerable.Range(_startIndex, _count()).Select(index => _read(index)).GetEnumerator();
        }

        /// <summary>
        /// Get the enumerator for this enumerable wrapper.
        /// </summary>
        /// <returns>Enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Get or set the item at the specified index.
        /// </summary>
        /// <param name="index">Index of the item.</param>
        /// <returns>Item at the specified index.</returns>
        public TItem this[int index]
        {
            get { return _read(index); }
            set { _write(index, value); }
        }

        /// <summary>
        /// Copy the contained items to another array, starting at the specified index.
        /// </summary>
        /// <param name="array">Array to copy to.</param>
        /// <param name="arrayIndex">Index on the target array to start.</param>
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            var count = _count();

            for (var i = 0; i < count; i++)
            {
                array[i] = _read(i + _startIndex);
            }
        }

        /// <summary>
        /// Copy the contained items to a new array.
        /// </summary>
        /// <returns>Array containing the items.</returns>
        public TItem[] ToArray()
        {
            var result = new TItem[_count()];
            CopyTo(result, 0);
            return result;
        }
    }
}
