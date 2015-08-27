using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Codecs
{
    /// <summary>
    /// A wrapper around Codec to allow for indexing an element's descendants.
    /// </summary>
    /// <typeparam name="TDecoded">Type of the decoded value.</typeparam>
    internal class IndexedCodec<TDecoded> : IIndexedCodec<TDecoded>
    {
        /// <summary>
        /// Create a codec indexer.
        /// </summary>
        /// <param name="baseElement">Element to reference.</param>
        /// <param name="decoder">Decoding function from HL7.</param>
        /// <param name="encoder">Encoding function to HL7.</param>
        public IndexedCodec(IElement baseElement, Func<string, TDecoded> decoder, Func<TDecoded, string> encoder)
        {
            BaseElement = baseElement;
            Decoder = decoder;
            Encoder = encoder;
        }

        /// <summary>
        /// Referenced element.
        /// </summary>
        IElement BaseElement
        {
            get;
            set;
        }

        /// <summary>
        /// Decoding function from HL7.
        /// </summary>
        Func<string, TDecoded> Decoder
        {
            get;
            set;
        }

        /// <summary>
        /// Encoding function to HL7.
        /// </summary>
        Func<TDecoded, string> Encoder
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the element descendant's value as the codec indexer's type.
        /// </summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element descendant's value.</returns>
        public TDecoded this[int index]
        {
            get { return Decoder(BaseElement[index].Value); }
            set { BaseElement[index].Value = Encoder(value); }
        }

        /// <summary>
        /// Get .NET standard generic enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<TDecoded> GetEnumerator()
        {
            return new IndexedCodecEnumerator<TDecoded>(this, BaseElement.DescendantCount);
        }

        /// <summary>
        /// Get .NET standard object enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
