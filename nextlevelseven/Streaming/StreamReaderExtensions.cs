using System;
using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    ///     Extensions to HL7 stream readers.
    /// </summary>
    public static class StreamReaderExtensions
    {
        /// <summary>
        ///     Read a message, skipping any ElementException and MessageException raised.
        /// </summary>
        /// <param name="reader">Reader to read with.</param>
        /// <returns>Message that was read, or null if none were found.</returns>
        public static INativeMessage ReadAndSkipMessageErrors(this MessageStreamReader reader)
        {
            try
            {
                return reader.Read();
            }
            catch (Exception ex)
            {
                if (ex is MessageException || ex is ElementException)
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        ///     Read all messages, skipping any ElementException and MessageException raised.
        /// </summary>
        /// <param name="reader">Reader to read with.</param>
        /// <returns>Messages that were read.</returns>
        public static IEnumerable<INativeMessage> ReadAllAndSkipMessageErrors(this MessageStreamReader reader)
        {
            var messages = new List<INativeMessage>();

            while (true)
            {
                try
                {
                    var message = reader.Read();
                    if (message == null)
                    {
                        return messages;
                    }
                    messages.Add(message);
                }
                catch (Exception ex)
                {
                    if (ex is MessageException || ex is ElementException)
                    {
                        messages.Add(null);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}