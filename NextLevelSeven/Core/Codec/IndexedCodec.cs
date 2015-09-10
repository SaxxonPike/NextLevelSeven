using System;
using System.Collections;
using System.Collections.Generic;
using NextLevelSeven.Native;

namespace NextLevelSeven.Core.Codec
{
    /// <summary>
    ///     A wrapper around ICodec to allow for indexing an element's descendants.
    /// </summary>
    /// <typeparam name="TDecoded">Type of the decoded value.</typeparam>
    internal sealed class IndexedCodec<TDecoded> : IIndexedCodec<TDecoded>
    {
        /// <summary>
        ///     Create a codec indexer.
        /// </summary>
        /// <param name="baseElement">Element to reference.</param>
        /// <param name="decoder">Decoding function from HL7.</param>
        /// <param name="encoder">Encoding function to HL7.</param>
        public IndexedCodec(INativeElement baseElement, Func<string, TDecoded> decoder, Func<TDecoded, string> encoder)
        {
            BaseElement = baseElement;
            Decoder = decoder;
            Encoder = encoder;
        }

        /// <summary>
        ///     Referenced element.
        /// </summary>
        private INativeElement BaseElement { get; set; }

        /// <summary>
        ///     Decoding function from HL7.
        /// </summary>
        private Func<string, TDecoded> Decoder { get; set; }

        /// <summary>
        ///     Encoding function to HL7.
        /// </summary>
        private Func<TDecoded, string> Encoder { get; set; }

        /// <summary>
        ///     Get or set the element descendant's value as the codec indexer's type.
        /// </summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element descendant's value.</returns>
        public TDecoded this[int index]
        {
            get { return Decoder(BaseElement[index].Value); }
            set { BaseElement[index].Value = Encoder(value); }
        }

        /// <summary>
        ///     Get .NET standard generic enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<TDecoded> GetEnumerator()
        {
            return new IndexedCodecEnumerator(this, BaseElement.DescendantCount);
        }

        /// <summary>
        ///     Get .NET standard object enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Allow for enumeration of a codec indexer's values.
        /// </summary>
        private sealed class IndexedCodecEnumerator : IEnumerator<TDecoded>
        {
            public IndexedCodecEnumerator(IIndexedCodec<TDecoded> indexedCodec, int count)
            {
                IndexedCodec = indexedCodec;
                Count = count;
                Reset();
            }

            /// <summary>
            ///     Parent codec indexer.
            /// </summary>
            private IIndexedCodec<TDecoded> IndexedCodec { get; set; }

            /// <summary>
            ///     Number of items.
            /// </summary>
            private int Count { get; set; }

            /// <summary>
            ///     Currently selected index.
            /// </summary>
            private int Index { get; set; }

            /// <summary>
            ///     Currently selected item.
            /// </summary>
            public TDecoded Current
            {
                get { return IndexedCodec[Index]; }
            }

            /// <summary>
            ///     Unused- required by interface.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            ///     Currently selected item (for old IEnumerator.)
            /// </summary>
            object IEnumerator.Current
            {
                get { return IndexedCodec[Index]; }
            }

            /// <summary>
            ///     Move to the next index in the collection.
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
            ///     Reset the index.
            /// </summary>
            public void Reset()
            {
                Index = -1;
            }
        }
    }
}