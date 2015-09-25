using System;

namespace NextLevelSeven.Core.Properties
{
    /// <summary>An interface that wraps properties around an HL7 message's fields.</summary>
    public interface IMessageDetails
    {
        /// <summary>Get or set the message control ID.</summary>
        string ControlId { get; set; }

        /// <summary>Get or set the message processing ID.</summary>
        string ProcessingId { get; set; }

        /// <summary>Get the receiving application and facility information.</summary>
        IIdentity Receiver { get; }

        /// <summary>Get or set the security code.</summary>
        string Security { get; set; }

        /// <summary>Get the sending application and facility information.</summary>
        IIdentity Sender { get; }

        /// <summary>Get or set the date/time of the message.</summary>
        DateTimeOffset? Time { get; set; }

        /// <summary>Get or set the 3-character trigger event.</summary>
        string TriggerEvent { get; set; }

        /// <summary>Get or set the 3-character message type.</summary>
        string Type { get; set; }

        /// <summary>Get or set the HL7 version number. If it does not exist, returns null.</summary>
        string Version { get; set; }
    }
}