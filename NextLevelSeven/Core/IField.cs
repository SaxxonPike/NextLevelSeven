using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    public interface IField
    {
        /// <summary>
        ///     Get data from a specific place in the field. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The the specified element.</returns>
        string GetValue(int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>
        ///     Get data from a specific place in the field. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1);
    }
}
