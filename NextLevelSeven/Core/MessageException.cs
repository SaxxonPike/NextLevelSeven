using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Represents an exception that would be raised for HL7 message-level errors.
    /// </summary>
    public class MessageException : Exception
    {
        /// <summary>
        /// Create an element exception.
        /// </summary>
        public MessageException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}
