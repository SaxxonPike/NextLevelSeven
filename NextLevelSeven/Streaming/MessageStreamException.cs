using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     Represents an exception that would be raised for HL7 streaming errors.
    /// </summary>
    public class MessageStreamException : Exception
    {
        /// <summary>
        ///     Create an HL7 stream exception.
        /// </summary>
        public MessageStreamException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}