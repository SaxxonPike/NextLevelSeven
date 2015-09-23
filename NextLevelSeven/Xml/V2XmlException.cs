using System;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Xml
{
    /// <summary>
    ///     Represents an exception that would be raised by a parser.
    /// </summary>
    [Serializable]
    public class V2XmlException : ElementException
    {
        /// <summary>
        ///     Create a V2XmlException with the specified error code.
        /// </summary>
        /// <param name="code">Error code.</param>
        internal V2XmlException(ErrorCode code)
            : base(code)
        {
        }
    }
}