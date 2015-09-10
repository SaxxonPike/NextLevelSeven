using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    public interface IBuilder
    {
        /// <summary>
        ///     Get the number of subcomponents in this component, including subcomponents with no content.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get or set the content string.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        ///     Get or set content within this component.
        /// </summary>
        IEnumerableIndexable<int, string> Values { get; }
    }
}
