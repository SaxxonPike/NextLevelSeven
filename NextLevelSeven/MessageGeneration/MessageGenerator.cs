using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.MessageGeneration
{
    /// <summary>
    /// A generic message generator for HL7v2 messages, which fills in most basic information when creating new messages. This is a static class.
    /// </summary>
    static public class MessageGenerator
    {
        static public IMessage Generate(string type, string triggerEvent, string controlId, string processingId = "P", string receivingApplication = null, string receivingFacility = null, string sendingApplication = null, string sendingFacility = null, string version = "2.3")
        {
            var message = new Message
            {
                Type = type,
                TriggerEvent = triggerEvent,
                ControlId = controlId,
                ProcessingId = processingId,
                Time = DateTimeOffset.Now
            };
            message.Receiver.Application = receivingApplication;
            message.Receiver.Facility = receivingFacility;
            message.Sender.Application = sendingApplication ?? Identity.ApplicationDefault;
            message.Sender.Facility = sendingFacility ?? Identity.FacilityDefault;
            return message;
        }
    }
}
