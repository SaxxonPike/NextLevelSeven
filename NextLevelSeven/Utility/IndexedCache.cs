using System.Collections;
using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    /// <summary>A cache that wraps around a dictionary, auto-requesting items that don't exist already.</summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal sealed class IndexedCache<TKey, TValue> : IIndexedCache<TKey, TValue>
        where TValue : class
    {
        /// <summary>Internal cache.</summary>
        private readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        /// <summary>Factory method to generate keys that don't exist already.</summary>
        private readonly ProxyFactory<TKey, TValue> _factory;

        /// <summary>Create an indexed cache that uses the specified factory to generate items not already cached.</summary>
        /// <param name="factory"></param>
        public IndexedCache(ProxyFactory<TKey, TValue> factory)
        {
            _factory = factory;
        }

        /// <summary>Get or set an item in the cache.</summary>
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
            set
            {
                _cache[index] = value;
            }
        }

        /// <summary>Returns true if the specified key exists in the cache.</summary>
        /// <param name="index">Index to search for.</param>
        /// <returns></returns>
        public bool Contains(TKey index)
        {
            return _cache.ContainsKey(index);
        }

        /// <summary>Get the number of items in the cache.</summary>
        public int Count
        {
            get
            {
                return _cache.Count;
            }
        }

        /// <summary>Clear the cache.</summary>
        public void Clear()
        {
            _cache.Clear();
        }

        /// <summary>Remove an item from the cache.</summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>True, if removal was successful.</returns>
        public bool Remove(TKey index)
        {
            return _cache.Remove(index);
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _cache.GetEnumerator();
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cache.GetEnumerator();
        }
    }
}