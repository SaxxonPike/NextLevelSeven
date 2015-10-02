using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;

namespace NextLevelSeven.Utility
{
    /// <summary>A cache that wraps around a dictionary, auto-requesting items that don't exist already.</summary>
    /// <typeparam name="TValue"></typeparam>
    internal class IndexedElementCache<TValue> : IIndexedElementCache<TValue> where TValue : class, IElement
    {
        /// <summary>Internal cache.</summary>
        protected readonly Dictionary<int, TValue> Cache = new Dictionary<int, TValue>();

        /// <summary>Factory method to generate keys that don't exist already.</summary>
        private readonly ProxyFactory<int, TValue> _factory;

        /// <summary>Create an indexed cache that uses the specified factory to generate items not already cached.</summary>
        /// <param name="factory"></param>
        public IndexedElementCache(ProxyFactory<int, TValue> factory)
        {
            _factory = factory;
        }

        /// <summary>Get or set an item in the cache.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Item in the cache.</returns>
        public TValue this[int index]
        {
            get
            {
                TValue value;
                if (Cache.TryGetValue(index, out value))
                {
                    return value;
                }
                value = _factory(index);
                Cache[index] = value;
                return value;
            }
            set { Cache[index] = value; }
        }

        /// <summary>Returns true if the specified key exists in the cache.</summary>
        /// <param name="index">Index to search for.</param>
        /// <returns></returns>
        public bool Contains(int index)
        {
            return Cache.ContainsKey(index);
        }

        /// <summary>Get the number of items in the cache.</summary>
        public int Count
        {
            get { return Cache.Count; }
        }

        /// <summary>Clear the cache.</summary>
        public void Clear()
        {
            Cache.Clear();
        }

        /// <summary>Remove an item from the cache.</summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>True, if removal was successful.</returns>
        public bool Remove(int index)
        {
            return Cache.Remove(index);
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            return Cache.GetEnumerator();
        }

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Cache.GetEnumerator();
        }
    }
}