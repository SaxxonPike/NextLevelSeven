using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Web
{
    /// <summary>
    ///     A queue of messages to be processed.
    /// </summary>
    public class MessageQueue
    {
        /// <summary>
        ///     Create an empty message queue.
        /// </summary>
        public MessageQueue()
        {
            Messages = new Queue<QueuedMessage>();
        }

        /// <summary>
        ///     Get the number of items in the queue.
        /// </summary>
        public int Count
        {
            get { return Messages.Count; }
        }

        /// <summary>
        ///     Get the current message queue.
        /// </summary>
        public Queue<QueuedMessage> Messages { get; private set; }

        /// <summary>
        ///     Remove an item from the front of the queue and get its message.
        /// </summary>
        /// <returns>Message that came from the dequeued item.</returns>
        public IMessage Dequeue()
        {
            if (Messages.Count > 0)
            {
                return Messages.Dequeue().Contents;
            }
            return null;
        }

        /// <summary>
        ///     Add an item to the back of the queue.
        /// </summary>
        /// <param name="message">Message to add to the queue.</param>
        public void Enqueue(IMessage message)
        {
            var queuedMessage = new QueuedMessage(message);
            Messages.Enqueue(queuedMessage);
        }

        /// <summary>
        ///     Increment the retry count on a queued message and, if the retry count is below the threshold, add to the end of the
        ///     queue.
        /// </summary>
        /// <param name="message">Message to potentially add to the queue.</param>
        protected void Retry(QueuedMessage message)
        {
            message.Retries++;
            if (message.Retries < 3)
            {
                Messages.Enqueue(message);
            }
            else
            {
                throw new MessageTransportException(ErrorCode.ExceededRetriesForMessage);
            }
        }
    }
}