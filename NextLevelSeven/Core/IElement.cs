using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// An interface to an HL7 element.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Get the index of the element.
        /// </summary>
        int Index { get; }
        
        /// <summary>
        /// Get or set the complete value of the element.
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Get or set the subvalues of the element.
        /// </summary>
        IEnumerable<string> Values { get; set; }
    }
}
