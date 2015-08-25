using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Streaming
{
    public class HL7StreamException : Exception
    {
        private const string DefaultMessage = @"An error occurred while processing an HL7 stream.";

        /// <summary>
        /// Create a generic HL7 stream exception.
        /// </summary>
        public HL7StreamException() : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Create an HL7 stream exception with the specified message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public HL7StreamException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create an HL7 stream exception with the specified message and inner exception that caused the exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception that caused the HL7 exception.</param>
        public HL7StreamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
