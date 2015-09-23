using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     Represents an exception that would be thrown when a stream-related occurs.
    /// </summary>
    [Serializable]
    public class StreamException : Exception
    {
        /// <summary>
        ///     Create a StreamException.
        /// </summary>
        internal StreamException(ErrorCode code)
            : base(ErrorMessages.Get(code))
        {
        }
    }
}