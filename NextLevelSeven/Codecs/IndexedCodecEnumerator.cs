using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Codecs
{
    internal class IndexedCodecEnumerator<TDecoded> : IEnumerator<TDecoded>
    {
        public IndexedCodecEnumerator(IIndexedCodec<TDecoded> indexedCodec, int count)
        {
            IndexedCodec = indexedCodec;
            Count = count;
            Reset();
        }

        IIndexedCodec<TDecoded> IndexedCodec { get; set; }

        int Count
        {
            get;
            set;
        }

        public TDecoded Current
        {
            get { return IndexedCodec[Index]; }
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return IndexedCodec[Index]; }
        }

        int Index
        {
            get;
            set;
        }

        public bool MoveNext()
        {
            if (Index >= Count - 1)
            {
                return false;
            }
            Index++;
            return true;
        }

        public void Reset()
        {
            Index = -1;
        }
    }
}
