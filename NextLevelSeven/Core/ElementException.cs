using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Represents an exception that would be raised for HL7 element specific errors.
    /// </summary>
    public class ElementException : Exception
    {
        private const string DefaultMessage = @"An error occurred while performing this operation on an element.";

        /// <summary>
        /// Create a generic element exception.
        /// </summary>
        public ElementException() : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Create an element exception with the specified message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public ElementException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create an element exception with the specified message and inner exception that caused the exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception that caused the element exception.</param>
        public ElementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
