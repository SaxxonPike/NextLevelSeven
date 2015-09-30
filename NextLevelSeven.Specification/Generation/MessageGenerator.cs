using System;
using NextLevelSeven.Building;
using NextLevelSeven.Core;

namespace NextLevelSeven.Specification.Generation
{
    /// <summary>A generic message generator for HL7v2 messages, which fills in most basic information when creating new messages. This is a static class.</summary>
    public static class MessageGenerator
    {
        /// <summary>Generate an HL7 message with the given parameters.</summary>
        /// <param name="type">Type of message. (MSH-9-1)</param>
        /// <param name="triggerEvent">Trigger event of message. (MSH-9-2)</param>
        /// <param name="controlId">Control ID. (MSH-10 app-specific)</param>
        /// <param name="processingId">Processing ID. (MSH-11)</param>
        /// <param name="receivingApplication">Receiving application. (MSH-5 app-specific)</param>
        /// <param name="receivingFacility">Receiving facility. (MSH-6 app-specific)</param>
        /// <param name="sendingApplication">Sending application. (MSH-3 app-specific)</param>
        /// <param name="sendingFacility">Sending facility. (MSH-4 app-specific)</param>
        /// <param name="version">Version number. Defaults to 2.3 since statistically it is the most used version. (MSH-12)</param>
        /// <returns></returns>
        public static IMessageBuilder Generate(string type, string triggerEvent, string controlId,
            string processingId = "P", string receivingApplication = null, string receivingFacility = null,
            string sendingApplication = null, string sendingFacility = null, string version = "2.3")
        {
            var message = Message.Build();
            var details = message.Details;
            details.Type = type;
            details.TriggerEvent = triggerEvent;
            details.ControlId = controlId;
            details.ProcessingId = processingId;
            details.Time = DateTimeOffset.Now;
            details.Version = version;
            details.Receiver.Application = receivingApplication;
            details.Receiver.Facility = receivingFacility;
            details.Sender.Application = sendingApplication ?? ElementDefaults.Application;
            details.Sender.Facility = sendingFacility ?? ElementDefaults.Facility;
            return message;
        }
    }
}