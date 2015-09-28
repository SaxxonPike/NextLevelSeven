using NextLevelSeven.Building;
using NextLevelSeven.Building.Elements;
using NextLevelSeven.Parsing;
using NextLevelSeven.Parsing.Elements;

namespace NextLevelSeven.Core
{
    /// <summary>Factory class. A starting point for all HL7 message related activities.</summary>
    public static class Message
    {
        /// <summary>Create a message builder with a default MSH.</summary>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder Build()
        {
            return new MessageBuilder();
        }

        /// <summary>Create a message builder, initialized with the specified message data.</summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder Build(string message)
        {
            return new MessageBuilder
            {
                Value = message
            };
        }

        /// <summary>Create a message builder, initialized with the content of the specified message or other element.</summary>
        /// <param name="message">Message or other element data.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder Build(IElement message)
        {
            return new MessageBuilder
            {
                Value = message.Value
            };
        }

        /// <summary>Create a message builder, initialized with the specified formatted-string message data.</summary>
        /// <param name="message">Formatted string.</param>
        /// <param name="args">Arguments for the format.</param>
        /// <returns>New message builder.</returns>
        public static IMessageBuilder BuildFormat(string message, params object[] args)
        {
            return new MessageBuilder
            {
                Value = string.Format(message, args)
            };
        }

        /// <summary>Create a new message with a default MSH.</summary>
        /// <returns>New message.</returns>
        public static IMessageParser Parse()
        {
            return new MessageParser();
        }

        /// <summary>Create a new message, initialized with the specified message data.</summary>
        /// <param name="message">Message data.</param>
        /// <returns>New message parser.</returns>
        public static IMessageParser Parse(string message)
        {
            return new MessageParser
            {
                Value = message
            };
        }

        /// <summary>Create a new message, initialized with the content of the specified message or other element.</summary>
        /// <param name="message">Message or other element data.</param>
        /// <returns>New message parser.</returns>
        public static IMessageParser Parse(IElement message)
        {
            return new MessageParser
            {
                Value = message.Value
            };
        }

        /// <summary>Create a new message, initialized with the specified formatted-string message data.</summary>
        /// <param name="message">Formatted string.</param>
        /// <param name="args">Arguments for the format.</param>
        /// <returns>New message parser.</returns>
        public static IMessageParser ParseFormat(string message, params object[] args)
        {
            return new MessageParser
            {
                Value = string.Format(message, args)
            };
        }
    }
}