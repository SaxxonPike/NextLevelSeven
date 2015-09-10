using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Represents an exception that would be raised for HL7 message-level errors.
    /// </summary>
    [Serializable]
    public class MessageException : Exception
    {
        /// <summary>
        ///     Create an element exception.
        /// </summary>
        internal MessageException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}