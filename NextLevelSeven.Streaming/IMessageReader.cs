using System.Collections.Generic;
using NextLevelSeven.Parsing;

namespace NextLevelSeven.Streaming
{
    /// <summary>HL7 message reader interface.</summary>
    public interface IMessageReader
    {
        /// <summary>Read one message. Returns null if no more messages are available.</summary>
        /// <returns>The message that was read.</returns>
        IMessageParser Read();

        /// <summary>Read all messages. When no messages are available, returns an empty collection.</summary>
        /// <returns>The messages that were read.</returns>
        IEnumerable<IMessageParser> ReadAll();
    }
}