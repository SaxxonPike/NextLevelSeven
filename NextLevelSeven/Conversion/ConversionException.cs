using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Conversion
{
    /// <summary>
    ///     Represents an exception that would be thrown when an HL7 type conversion failed.
    /// </summary>
    [Serializable]
    public class ConversionException : ElementException
    {
        /// <summary>
        ///     Create a ConversionException with the specified error code.
        /// </summary>
        /// <param name="code">Error code.</param>
        internal ConversionException(ErrorCode code)
            : base(code)
        {
        }
    }
}
