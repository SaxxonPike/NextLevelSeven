using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     Represents an exception that would be raised when an MLP-specific error occurs.
    /// </summary>
    [Serializable]
    public class MlpStreamException : StreamException
    {
        /// <summary>
        ///     Create an MLP stream exception.
        /// </summary>
        internal MlpStreamException(ErrorCode code)
            : base(code)
        {
        }
    }
}