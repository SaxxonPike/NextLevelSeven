using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    public interface ISegment : IElement
    {
        /// <summary>
        /// Get or set the three-character segment type.
        /// </summary>
        string Type { get; set; }
    }
}
