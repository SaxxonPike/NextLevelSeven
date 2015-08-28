using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Represents an exception that would be raised for HL7 message-level errors.
    /// </summary>
    public class MessageException : Exception
    {
        private const string DefaultMessage = @"An error occurred while performing this operation on a message.";

        /// <summary>
        /// Create a generic element exception.
        /// </summary>
        public MessageException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Create a message exception with the specified string.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MessageException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create a message exception with the specified string and inner exception that caused the exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception that caused the message exception.</param>
        public MessageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
