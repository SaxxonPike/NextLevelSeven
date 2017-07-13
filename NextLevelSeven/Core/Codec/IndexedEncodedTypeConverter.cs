using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Core.Codec
{
    /// <summary>A wrapper around ICodec to allow for indexing an element's descendants.</summary>
    /// <typeparam name="TDecoded">Type of the decoded value.</typeparam>
    internal sealed class IndexedEncodedTypeConverter<TDecoded> : IIndexedEncodedTypeConverter<TDecoded>
    {
        /// <summary>Referenced element.</summary>
        private readonly IElement _baseElement;

        /// <summary>Decoding function from HL7.</summary>
        private readonly ProxyConverter<string, TDecoded> _decoder;

        /// <summary>Encoding function to HL7.</summary>
        private readonly ProxyConverter<TDecoded, string> _encoder;

        /// <summary>Create a codec indexer.</summary>
        /// <param name="baseElement">Element to reference.</param>
        /// <param name="decoder">Decoding function from HL7.</param>
        /// <param name="encoder">Encoding function to HL7.</param>
        public IndexedEncodedTypeConverter(IElement baseElement, ProxyConverter<string, TDecoded> decoder,
            ProxyConverter<TDecoded, string> encoder)
        {
            _baseElement = baseElement;
            _decoder = decoder;
            _encoder = encoder;
        }

        /// <summary>
        ///     Get the number of items in the collection.
        /// </summary>
        public int Count => _baseElement.ValueCount;

        /// <summary>Get or set the items in the element as a collection.</summary>
        public IEnumerable<TDecoded> Items
        {
            get
            {
                var count = _baseElement.ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return this[i];
                }
            }
            set
            {
                _baseElement.Values = value.Select(v => _encoder(v));
            }
        }

        /// <summary>Get or set the element descendant's value as the codec indexer's type.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element descendant's value.</returns>
        public TDecoded this[int index]
        {
            get => _decoder(_baseElement[index].Value);
            set => _baseElement[index].Value = _encoder(value);
        }
    }
}