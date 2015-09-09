using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Utility
{
    static public class EnumerableExtensions
    {
        /// <summary>
        /// Wrap an IEnumerable around a single item.
        /// </summary>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <param name="item">Item to wrap.</param>
        /// <returns>Wrapped item.</returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
