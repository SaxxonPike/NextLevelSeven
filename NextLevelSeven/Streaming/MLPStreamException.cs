using System;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     Represents an exception that would be raised when an MLP-specific error occurs.
    /// </summary>
    [Serializable]
    public class MlpStreamException : Exception
    {
        /// <summary>
        ///     Create an MLP stream exception.
        /// </summary>
        public MlpStreamException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}