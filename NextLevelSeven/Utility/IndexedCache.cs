using System;
using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    /// <summary>
    ///     A cache that wraps around a dictionary, auto-requesting items that don't exist already.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal sealed class IndexedCache<TKey, TValue>
    {
        /// <summary>
        ///     Internal cache.
        /// </summary>
        private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        /// <summary>
        ///     Factory method to generate keys that don't exist already.
        /// </summary>
        private readonly Func<TKey, TValue> _factory;

        /// <summary>
        ///     Create an indexed cache that uses the specified factory to generate items not already cached.
        /// </summary>
        /// <param name="factory"></param>
        public IndexedCache(Func<TKey, TValue> factory)
        {
            _factory = factory;
        }

        /// <summary>
        ///     Get or set an item in the cache.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Item in the cache.</returns>
        public TValue this[TKey index]
        {
            get
            {
                TValue value;
                if (_cache.TryGetValue(index, out value))
                {
                    return value;
                }
                value = _factory(index);
                _cache[index] = value;
                return value;
            }
            set { _cache[index] = value; }
        }

        /// <summary>
        ///     Get the number of items in the cache.
        /// </summary>
        public int Count
        {
            get { return _cache.Count; }
        }

        /// <summary>
        ///     Clear the cache.
        /// </summary>
        public void Clear()
        {
        }
    }
}