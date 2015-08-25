using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Web
{
    public class MessageSenderException : Exception
    {
        private const string DefaultMessage = @"An error occurred during transmission.";

        /// <summary>
        /// Create a generic message sender exception.
        /// </summary>
        public MessageSenderException()
            : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Create a message sender exception with the specified message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MessageSenderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Create a message sender exception with the specified message and inner exception that caused the exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception that caused the message sender exception.</param>
        public MessageSenderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
