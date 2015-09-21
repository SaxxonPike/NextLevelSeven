using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    /// <summary>
    ///     Interface to an indexed cache.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public interface IIndexedCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        ///     Get or set an item with the specified key.
        /// </summary>
        /// <param name="index">Key.</param>
        /// <returns>Value belonging to the key.</returns>
        TValue this[TKey index] { get; set; }

        /// <summary>
        ///     Determine if the specified key is in the cache.
        /// </summary>
        /// <param name="index">Key.</param>
        /// <returns>True, if the key is present.</returns>
        bool Contains(TKey index);

        /// <summary>
        ///     Get the number of items in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Clear the cache.
        /// </summary>
        void Clear();

        /// <summary>
        ///     Remove a value with the specific key.
        /// </summary>
        /// <param name="index">Key to remove.</param>
        /// <returns>True, if the key was found.</returns>
        bool Remove(TKey index);
    }
}
