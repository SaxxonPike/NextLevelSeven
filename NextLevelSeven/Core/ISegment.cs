using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Common interface for the second depth level of an HL7 message, which contains segment type information.
    /// </summary>
    public interface ISegment : IElement
    {
        /// <summary>
        /// Create a detached clone of the segment with no ancestors.
        /// </summary>
        /// <returns></returns>
        new ISegment CloneDetached();

        /// <summary>
        /// Get or set the three-character segment type.
        /// </summary>
        string Type { get; set; }
    }
}
