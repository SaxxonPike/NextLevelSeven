using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Represents an exception that would be raised for HL7 message-level errors.
    /// </summary>
    [Serializable]
    public class MessageException : ElementException
    {
        /// <summary>
        ///     Create a message exception.
        /// </summary>
        internal MessageException(ErrorCode code) : base(code)
        {
        }
    }
}