using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Codecs
{
    internal class IndexedCodec<TDecoded> : IIndexedCodec<TDecoded>
    {
        public IndexedCodec(IElement baseElement, Func<string, TDecoded> decoder, Func<TDecoded, string> encoder)
        {
            BaseElement = baseElement;
            Decoder = decoder;
            Encoder = encoder;
        }

        IElement BaseElement
        {
            get;
            set;
        }

        Func<string, TDecoded> Decoder
        {
            get;
            set;
        }

        Func<TDecoded, string> Encoder
        {
            get;
            set;
        }

        public TDecoded this[int index]
        {
            get { return Decoder(BaseElement[index].Value); }
            set { BaseElement[index].Value = Encoder(value); }
        }

        public IEnumerator<TDecoded> GetEnumerator()
        {
            return new IndexedCodecEnumerator<TDecoded>(this, BaseElement.DescendantCount);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
