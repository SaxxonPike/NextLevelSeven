using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Codecs
{
    /// <summary>
    /// Allow for enumeration of a codec indexer's values.
    /// </summary>
    /// <typeparam name="TDecoded">Underlying type.</typeparam>
    internal class IndexedCodecEnumerator<TDecoded> : IEnumerator<TDecoded>
    {
        public IndexedCodecEnumerator(IIndexedCodec<TDecoded> indexedCodec, int count)
        {
            IndexedCodec = indexedCodec;
            Count = count;
            Reset();
        }

        /// <summary>
        /// Parent codec indexer.
        /// </summary>
        IIndexedCodec<TDecoded> IndexedCodec { get; set; }

        /// <summary>
        /// Number of items.
        /// </summary>
        int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Currently selected item.
        /// </summary>
        public TDecoded Current
        {
            get { return IndexedCodec[Index]; }
        }

        /// <summary>
        /// Unused- required by interface.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Currently selected item (for old IEnumerator.)
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return IndexedCodec[Index]; }
        }

        /// <summary>
        /// Currently selected index.
        /// </summary>
        int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Move to the next index in the collection.
        /// </summary>
        /// <returns>True if there are more elements, false if not.</returns>
        public bool MoveNext()
        {
            if (Index >= Count - 1)
            {
                return false;
            }
            Index++;
            return true;
        }

        /// <summary>
        /// Reset the index.
        /// </summary>
        public void Reset()
        {
            Index = -1;
        }
    }
}
