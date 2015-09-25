using System;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Parsing
{
    /// <summary>Represents an exception that would be raised by a parser.</summary>
    [Serializable]
    public class ParserException : ElementException
    {
        /// <summary>Create a ParserException with the specified error code.</summary>
        /// <param name="code">Error code.</param>
        internal ParserException(ErrorCode code)
            : base(code)
        {
        }
    }
}