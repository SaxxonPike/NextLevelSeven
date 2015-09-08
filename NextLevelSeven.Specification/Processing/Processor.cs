using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Specification.Processing
{
    public class Processor
    {
        private const string EmptyKey = "\x1";

        public event ProcessorEventHandler OnMessageUnhandled;

        private readonly Dictionary<string, Dictionary<string, HashSet<ProcessorEventHandler>>> _handlers =
            new Dictionary<string, Dictionary<string, HashSet<ProcessorEventHandler>>>();

        public readonly object SyncRoot = new object();

        private IEnumerable<ProcessorEventHandler> GetHandlers(string messageType, string messageTriggerEvent)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                messageType = EmptyKey;
            }
            if (string.IsNullOrEmpty(messageTriggerEvent))
            {
                messageTriggerEvent = EmptyKey;
            }
            var messageTypeNonGenericHandlers = GetNonGenericHandlers(messageType, messageTriggerEvent);
            var messageTypeGenericHandlers = (_handlers[messageType].ContainsKey(EmptyKey))
                ? _handlers[messageType][EmptyKey]
                : Enumerable.Empty<ProcessorEventHandler>();

            return messageTypeNonGenericHandlers.Concat(messageTypeGenericHandlers);
        }

        private HashSet<ProcessorEventHandler> GetNonGenericHandlers(string messageType, string messageTriggerEvent)
        {
            if (string.IsNullOrEmpty(messageType))
            {
                messageType = EmptyKey;
            }
            if (string.IsNullOrEmpty(messageTriggerEvent))
            {
                messageTriggerEvent = EmptyKey;
            }
            if (!_handlers.ContainsKey(messageType))
            {
                _handlers[messageType] = new Dictionary<string, HashSet<ProcessorEventHandler>>();
            }
            var messageTypeHandlers = _handlers[messageType];

            if (!messageTypeHandlers.ContainsKey(messageTriggerEvent))
            {
                messageTypeHandlers[messageTriggerEvent] = new HashSet<ProcessorEventHandler>();
            }
            return messageTypeHandlers[messageTriggerEvent];
        }

        public void Process(IMessage message)
        {
            var handlers = GetHandlers(message.Type, message.TriggerEvent);
            var handled = false;
            var args = new ProcessorEventArgs(message);

            foreach (var handler in handlers)
            {
                handled = true;
                handler(this, args);
            }

            if (!handled && OnMessageUnhandled != null)
            {
                OnMessageUnhandled(this, args);
            }
        }

        public void Register(string messageType, string messageTriggerEvent, ProcessorEventHandler handler)
        {
            lock (SyncRoot)
            {
                GetNonGenericHandlers(messageType ?? EmptyKey, messageTriggerEvent ?? EmptyKey).Add(handler);
            }
        }

        public void UnRegister(ProcessorEventHandler handler)
        {
            // do not use GetNonGenericHandlers as it can modify the collections
            lock (SyncRoot)
            {
                foreach (var mth in _handlers)
                {
                    foreach (var mteh in mth.Value)
                    {
                        mteh.Value.RemoveWhere(h => h == handler);
                    }
                }                
            }
        }
    }
}
