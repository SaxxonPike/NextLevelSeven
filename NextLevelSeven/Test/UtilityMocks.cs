using System;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Test
{
    /// <summary>
    ///     Contains mock classes for the Utility namespace for use in testing.
    /// </summary>
    static public class UtilityMocks
    {
        /// <summary>
        ///     Create an indexed cache, using the specified factory to lazily create values that don't already exist.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="factoryMethod">Method that will create new values.</param>
        /// <returns>Indexed cache of the specified types and the specified factory.</returns>
        static public IIndexedCache<TKey, TValue> GetIndexedCache<TKey, TValue>(Func<TKey, TValue> factoryMethod)
        {
            return new IndexedCache<TKey, TValue>(new ProxyFactory<TKey, TValue>(factoryMethod));
        }

        /// <summary>
        ///     Create a wrapper enumerable with the specified proxy methods and start index (optional.)
        /// </summary>
        /// <typeparam name="TItem">Type of items referenced by the IEnumerable.</typeparam>
        /// <param name="getter">Method to get indexed items.</param>
        /// <param name="setter">Method to set indexed items.</param>
        /// <param name="count">Method to get the number of items present.</param>
        /// <param name="startIndex">Starting index of items.</param>
        /// <returns>An enumerable that wraps an indexable interface.</returns>
        static public IEnumerableIndexable<int, TItem> GetWrapperEnumerable<TItem>(Func<int, TItem> getter, Action<int, TItem> setter,
            Func<int> count, int startIndex = 0)
        {
            return new WrapperEnumerable<TItem>(new ProxyGetter<int, TItem>(getter), new ProxySetter<int, TItem>(setter),
                new ProxyGetter<int>(count), startIndex);
        }
    }
}
