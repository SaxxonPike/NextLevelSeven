using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Represents an exception that would be raised for HL7 message-level errors.
    /// </summary>
    public class MessageException : Exception
    {
        /// <summary>
        ///     Create an element exception.
        /// </summary>
        public MessageException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}