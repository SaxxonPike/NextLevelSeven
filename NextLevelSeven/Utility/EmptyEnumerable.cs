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
        private static readonly EmptyEnumerator Enumerator = new EmptyEnumerator();

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

        /// <summary>
        /// An enumerator for the EmptyEnumerable.
        /// </summary>
        sealed internal class EmptyEnumerator: IEnumerator<T>
        {
            /// <summary>
            /// Returns null.
            /// </summary>
            public T Current
            {
                get { return null; }
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Returns null.
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get { return null; }
            }

            /// <summary>
            /// Returns false, since the collection is empty.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                return false;
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Reset()
            {
            }
        }
    }
}
