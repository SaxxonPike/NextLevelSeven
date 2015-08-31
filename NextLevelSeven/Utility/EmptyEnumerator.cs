using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    /// <summary>
    /// An enumerator for the EmptyEnumerable.
    /// </summary>
    /// <typeparam name="T">Type of items to be enumerated over.</typeparam>
    sealed internal class EmptyEnumerator<T> : IEnumerator<T> where T : class
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
