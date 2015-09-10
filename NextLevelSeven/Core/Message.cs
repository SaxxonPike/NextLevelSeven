using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Building;
using NextLevelSeven.Native;

namespace NextLevelSeven.Core
{
    /// <summary>
    /// Factory class. A starting point for all HL7 message related activities.
    /// </summary>
    static public class Message
    {
        /// <summary>
        /// Create a message builder with a default MSH.
        /// </summary>
        /// <returns>New message builder.</returns>
        static public IMessageBuilder Build()
        {
            return new MessageBuilder();
        }

        /// <summary>
        /// Create a message builder, initialized with the specified message data.
        /// </summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        static public IMessageBuilder Build(string message)
        {
            return new MessageBuilder(message);
        }

        /// <summary>
        /// Create a message builder, initialized with the specified message.
        /// </summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        static public IMessageBuilder Build(INativeMessage message)
        {
            return new MessageBuilder(message);
        }

        /// <summary>
        /// Create a new message with a default MSH.
        /// </summary>
        /// <returns>New message.</returns>
        static public INativeMessage Create()
        {
            return new NativeMessage();
        }

        /// <summary>
        /// Create a new message, initialized with the specified message data.
        /// </summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        static public INativeMessage Create(string message)
        {
            return new NativeMessage(message);
        }
    }
}
