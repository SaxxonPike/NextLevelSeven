using System.Collections.Generic;

namespace NextLevelSeven.Codecs
{
    public interface IIndexedCodec<TDecoded> : IEnumerable<TDecoded>
    {
        /// <summary>
        ///     Get or set element data at the specified index.
        /// </summary>
        TDecoded this[int index] { get; set; }
    }
}