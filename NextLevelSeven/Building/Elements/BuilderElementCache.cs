using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     A cache wrapper that takes a typed IndexedCache and can interpret items as Builder.
    /// </summary>
    /// <typeparam name="TValue">Preferred type.</typeparam>
    internal class BuilderElementCache<TValue> : StrongReferenceCache<TValue>, IIndexedCache<Builder>
        where TValue : Builder
    {
        /// <summary>
        ///     Create a cache wrapper.
        /// </summary>
        /// <param name="factory">Method to create items that are not cached yet but requested.</param>
        public BuilderElementCache(ProxyFactory<int, TValue> factory) : base(factory)
        {
        }

        /// <summary>
        ///     Returns true if any elements are marked as existing.
        /// </summary>
        public bool AnyExists
        {
            get { return Cache.Any(kv => kv.Value.Exists); }
        }

        /// <summary>
        ///     Returns the highest index in the cache.
        /// </summary>
        public int MaxKey
        {
            get { return Cache.Max(kv => kv.Key); }
        }

        /// <summary>
        ///     Orders items in the cache by their key.
        /// </summary>
        public IOrderedEnumerable<KeyValuePair<int, TValue>> OrderedByKey
        {
            get { return Cache.OrderBy(kv => kv.Key); }
        }

        /// <summary>
        ///     Get an enumerator for this cache.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public new IEnumerator<KeyValuePair<int, Builder>> GetEnumerator()
        {
            return Cache.Select(kv => new KeyValuePair<int, Builder>(kv.Key, kv.Value)).GetEnumerator();
        }

        /// <summary>
        ///     Get or set a Builder within the cache.
        /// </summary>
        /// <param name="index">Index to get or set.</param>
        /// <returns>Element at the specified index.</returns>
        Builder IIndexedCache<Builder>.this[int index]
        {
            get => base[index];
            set => base[index] = (TValue) value;
        }
    }
}