using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Web
{
    /// <summary>
    /// An IMessage with additional information about retries.
    /// </summary>
    public class QueuedMessage
    {
        /// <summary>
        /// Create a queued message with the specified number of retries.
        /// </summary>
        /// <param name="contents">Message contents.</param>
        /// <param name="retries">Number of previous attempts at processing this message.</param>
        public QueuedMessage(IMessage contents, int retries = 0)
        {
            Contents = contents;
            Retries = retries;
        }

        /// <summary>
        /// Get the message contents.
        /// </summary>
        public readonly IMessage Contents;

        /// <summary>
        /// Get or set the number of previous attempts to process this message.
        /// </summary>
        public int Retries;
    }
}
