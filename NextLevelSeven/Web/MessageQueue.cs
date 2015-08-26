using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Web
{
    public class MessageQueue
    {
        public MessageQueue()
        {
            Messages = new Queue<QueuedMessage>();
        }

        public int Count
        {
            get
            {
                return Messages.Count;
            }
        }

        public IMessage Dequeue()
        {
            if (Messages.Count > 0)
            {
                return Messages.Dequeue().Contents;
            }
            return null;
        }
        
        public void Enqueue(IMessage message)
        {
            var queuedMessage = new QueuedMessage(message);
            Messages.Enqueue(queuedMessage);
        }

        public Queue<QueuedMessage> Messages
        {
            get;
            private set;
        }

        protected void Retry(QueuedMessage message)
        {
            message.Retries++;
            if (message.Retries < 3)
            {
                Messages.Enqueue(message);
            }
            else
            {
                throw new MessageSenderException(@"Exceeded retries for message.");
            }
        }
    }
}
