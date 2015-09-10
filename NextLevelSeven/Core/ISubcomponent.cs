using System.Collections.Generic;

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