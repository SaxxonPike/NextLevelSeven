using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    /// <summary>
    /// A generic immutable empty collection.
    /// </summary>
    /// <typeparam name="T">Type of items in the collection.</typeparam>
    sealed internal class EmptyEnumerable<T> : IEnumerable<T> where T : class
    {
        /// <summary>
        /// Get the empty collection.
        /// </summary>
        public static readonly EmptyEnumerable<T> Instance = new EmptyEnumerable<T>();

        /// <summary>
        /// Get the immutable empty collection enumerator.
        /// </summary>
        private static readonly EmptyEnumerator<T> Enumerator = new EmptyEnumerator<T>();

        /// <summary>
        /// Get an enumerator for the empty collection.
        /// </summary>
        /// <returns>An enumerator which enumerates over nothing.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return Enumerator;
        }

        /// <summary>
        /// Get an enumerator for the empty collection.
        /// </summary>
        /// <returns>An enumerator which enumerates over nothing.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Enumerator;
        }
    }
}
