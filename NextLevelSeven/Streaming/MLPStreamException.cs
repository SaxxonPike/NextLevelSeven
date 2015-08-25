using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Streaming
{
    public class MLPStreamException : Exception
    {
        private const string DefaultMessage = @"An error occurred while processing an MLP stream.";

        /// <summary>
        /// Create a generic MLP stream exception.
        /// </summary>
        public MLPStreamException() : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Create an MLP stream exception with the specified message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MLPStreamException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create an MLP stream exception with the specified message and inner exception that caused the exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception that caused the MLP exception.</param>
        public MLPStreamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
