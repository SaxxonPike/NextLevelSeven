using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    public interface ISubcomponent
    {
        /// <summary>
        ///     Get data from a specific place in the field repetition. Depth is determined by how many indices are specified.
        /// </summary>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues();
    }
}
