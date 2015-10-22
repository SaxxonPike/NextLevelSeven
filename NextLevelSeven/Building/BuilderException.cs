using System;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Building
{
    /// <summary>Represents an exception that would be raised by a builder.</summary>
    [Serializable]
    public class BuilderException : ElementException
    {
        /// <summary>Create a BuilderException with the specified error code.</summary>
        /// <param name="code">Error code.</param>
        internal BuilderException(ErrorCode code)
            : base(code)
        {
        }
    }
}