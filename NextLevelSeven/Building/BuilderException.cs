using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Represents an exception that would be raised by a builder such as a MessageBuilder.
    /// </summary>
    [Serializable]
    public class BuilderException : Exception
    {
        /// <summary>
        ///     Create a BuilderException with the specified error code.
        /// </summary>
        /// <param name="code">Error code.</param>
        internal BuilderException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}