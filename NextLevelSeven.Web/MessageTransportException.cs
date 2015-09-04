using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     Represents an exception that would be thrown by a web HL7 message transport.
    /// </summary>
    [Serializable]
    public class MessageTransportException : Exception
    {
        /// <summary>
        ///     Create a generic message transport exception.
        /// </summary>
        public MessageTransportException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}