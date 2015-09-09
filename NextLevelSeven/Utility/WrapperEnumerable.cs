using System;
using System.Collections.Generic;
using System.Linq;

namespace NextLevelSeven.Utility
{
    sealed public class WrapperEnumerable<TItem> : IEnumerableIndexable<int, TItem>
    {
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

        public IEnumerator<TItem> GetEnumerator()
        {
            return Enumerable.Range(_startIndex, _count()).Select(index => _read(index)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public TItem this[int index]
        {
            get { return _read(index); }
            set { _write(index, value); }
        }
    }
}
