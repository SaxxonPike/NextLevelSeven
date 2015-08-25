using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Codecs
{
    public interface IIndexedCodec<TDecoded> : IEnumerable<TDecoded>
    {
        /// <summary>
        /// Get or set element data at the specified index.
        /// </summary>
        TDecoded this[int index] { get; set; }
    }
}
