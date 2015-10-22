using System.Collections.Generic;

namespace NextLevelSeven.Utility
{
    /// <summary>Extensions for IEnumerable.</summary>
    internal static class EnumerableExtensions
    {
        /// <summary>Wrap an IEnumerable around a single item.</summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="item">Item to wrap.</param>
        /// <returns>Wrapped item.</returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}