using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    /// Represents an exception that would be raised for HL7 streaming errors.
    /// </summary>
    public class HL7StreamException : Exception
    {
        /// <summary>
        /// Create an HL7 stream exception.
        /// </summary>
        public HL7StreamException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}
