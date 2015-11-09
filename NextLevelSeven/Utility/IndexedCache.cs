using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NextLevelSeven.Utility
{
    /// <summary>A cache that wraps around a dictionary, auto-requesting items that don't exist already.</summary>
    /// <typeparam name="TValue"></typeparam>
    internal abstract class IndexedCache<TValue> : IIndexedCache<TValue> where TValue : class
    {
        /// <summary>Factory method to generate keys that don't exist already.</summary>
        protected readonly ProxyFactory<int, TValue> Factory;

        /// <summary>Create an indexed cache that uses the specified factory to generate items not already cached.</summary>
        /// <param name="factory"></param>
        protected IndexedCache(ProxyFactory<int, TValue> factory)
        {
            Factory = factory;
        }

        /// <summary>Get or set an item in the cache.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Item in the cache.</returns>
        public abstract TValue this[int index] { get; set; }

        /// <summary>Returns true if the specified key exists in the cache.</summary>
        /// <param name="index">Index to search for.</param>
        /// <returns></returns>
        public abstract bool Contains(int index);

        /// <summary>Get the number of items in the cache.</summary>
        public abstract int Count { get; }

        /// <summary>Clear the cache.</summary>
        public abstract void Clear();

        /// <summary>Remove an item from the cache.</summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>True, if removal was successful.</returns>
        public abstract bool Remove(int index);

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        public abstract IEnumerator<KeyValuePair<int, TValue>> GetEnumerator();

        /// <summary>Get an enumerator for the cache.</summary>
        /// <returns>Enumerator.</returns>
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}