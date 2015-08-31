using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    /// Extensions to HL7 stream readers.
    /// </summary>
    static public class StreamReaderExtensions
    {
        /// <summary>
        /// Read a message, skipping any ElementException and MessageException raised.
        /// </summary>
        /// <param name="reader">Reader to read with.</param>
        /// <returns>Message that was read, or null if none were found.</returns>
        static public IMessage ReadAndSkipMessageErrors(this HL7StreamReader reader)
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
        /// Read all messages, skipping any ElementException and MessageException raised.
        /// </summary>
        /// <param name="reader">Reader to read with.</param>
        /// <returns>Messages that were read.</returns>
        static public IEnumerable<IMessage> ReadAllAndSkipMessageErrors(this HL7StreamReader reader)
        {
            var messages = new List<IMessage>();

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
