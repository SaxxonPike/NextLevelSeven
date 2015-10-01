using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    internal class BuilderElementCache<TValue> : IndexedElementCache<TValue>, IIndexedElementCache<Builder> where TValue : Builder
    {
        public BuilderElementCache(ProxyFactory<int, TValue> factory) : base(factory)
        {
        }

        new public IEnumerator<KeyValuePair<int, Builder>> GetEnumerator()
        {
            return Cache.Select(kv => new KeyValuePair<int, Builder>(kv.Key, kv.Value)).GetEnumerator();
        }

        Builder IIndexedElementCache<Builder>.this[int index]
        {
            get { return base[index]; }
            set { base[index] = (TValue)value; }
        }
    }
}
