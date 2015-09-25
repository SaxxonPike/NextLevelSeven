using System.Collections.Generic;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Core.Codec
{
    /// <summary>A wrapper around ICodec to allow for indexing an element's descendants.</summary>
    /// <typeparam name="TDecoded">Type of the decoded value.</typeparam>
    internal sealed class IndexedEncodedTypeConverter<TDecoded> : IIndexedEncodedTypeConverter<TDecoded>
    {
        /// <summary>Create a codec indexer.</summary>
        /// <param name="baseElement">Element to reference.</param>
        /// <param name="decoder">Decoding function from HL7.</param>
        /// <param name="encoder">Encoding function to HL7.</param>
        public IndexedEncodedTypeConverter(IElement baseElement, ProxyConverter<string, TDecoded> decoder,
            ProxyConverter<TDecoded, string> encoder)
        {
            BaseElement = baseElement;
            Decoder = decoder;
            Encoder = encoder;
        }

        /// <summary>Referenced element.</summary>
        private IElement BaseElement
        {
            get;
            set;
        }

        /// <summary>Decoding function from HL7.</summary>
        private ProxyConverter<string, TDecoded> Decoder
        {
            get;
            set;
        }

        /// <summary>Encoding function to HL7.</summary>
        private ProxyConverter<TDecoded, string> Encoder
        {
            get;
            set;
        }

        /// <summary>Get the items in the element as a collection.</summary>
        public IEnumerable<TDecoded> Items
        {
            get
            {
                var count = BaseElement.ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return this[i];
                }
            }
        }

        /// <summary>Get or set the element descendant's value as the codec indexer's type.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element descendant's value.</returns>
        public TDecoded this[int index]
        {
            get
            {
                return Decoder(BaseElement[index].Value);
            }
            set
            {
                BaseElement[index].Value = Encoder(value);
            }
        }
    }
}