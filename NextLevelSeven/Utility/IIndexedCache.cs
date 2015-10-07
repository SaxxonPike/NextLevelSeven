using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    /// <summary>Interface to an indexed cache.</summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public interface IIndexedCache<TValue> : IEnumerable<KeyValuePair<int, TValue>>
        where TValue : class
    {
        /// <summary>Get or set an item with the specified key.</summary>
        /// <param name="index">Key.</param>
        /// <returns>Value belonging to the key.</returns>
        TValue this[int index] { get; set; }

        /// <summary>Get the number of items in the cache.</summary>
        int Count { get; }

        /// <summary>Determine if the specified key is in the cache.</summary>
        /// <param name="index">Key.</param>
        /// <returns>True, if the key is present.</returns>
        bool Contains(int index);

        /// <summary>Clear the cache.</summary>
        void Clear();

        /// <summary>Remove a value with the specific key.</summary>
        /// <param name="index">Key to remove.</param>
        /// <returns>True, if the key was found.</returns>
        bool Remove(int index);
    }
}