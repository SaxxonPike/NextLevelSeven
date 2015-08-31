using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    /// Represents an exception that would be raised when an MLP-specific error occurs.
    /// </summary>
    public class MLPStreamException : Exception
    {
        /// <summary>
        /// Create an MLP stream exception.
        /// </summary>
        public MLPStreamException(ErrorCode code) : base(ErrorMessages.Get(code))
        {
        }
    }
}
