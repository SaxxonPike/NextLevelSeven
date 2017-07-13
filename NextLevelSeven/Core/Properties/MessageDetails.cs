using System;

namespace NextLevelSeven.Core.Properties
{
    /// <summary>Wraps message details around a message.</summary>
    internal sealed class MessageDetails : IMessageDetails
    {
        /// <summary>Message to wrap properties around.</summary>
        private readonly IMessage _message;

        /// <summary>Create a message detail wrapper for a message.</summary>
        /// <param name="message"></param>
        public MessageDetails(IMessage message)
        {
            _message = message;
        }

        /// <summary>Get or set the message control ID.</summary>
        public string ControlId
        {
            get => _message[1][10].Value;
            set => _message[1][10].Value = value;
        }

        /// <summary>Get or set the message processing ID.</summary>
        public string ProcessingId
        {
            get => _message[1][11].Value;
            set => _message[1][11].Value = value;
        }

        /// <summary>Get the receiving application and facility information.</summary>
        public IIdentity Receiver => new ProxyIdentity(_message[1], 5, 6);

        /// <summary>Get or set the security code.</summary>
        public string Security
        {
            get => _message[1][8].Value;
            set => _message[1][8].Value = value;
        }

        /// <summary>Get the sending application and facility information.</summary>
        public IIdentity Sender => new ProxyIdentity(_message[1], 3, 4);

        /// <summary>Get or set the date/time of the message.</summary>
        public DateTimeOffset? Time
        {
            get => _message[1][7].Converter.AsDateTime;
            set => _message[1][7].Converter.AsDateTime = value;
        }

        /// <summary>Get or set the 3-character trigger event.</summary>
        public string TriggerEvent
        {
            get => _message[1][9][1][2].Value;
            set => _message[1][9][1][2].Value = value;
        }

        /// <summary>Get or set the 3-character message type.</summary>
        public string Type
        {
            get => _message[1][9][1][1].Value;
            set => _message[1][9][1][1].Value = value;
        }

        /// <summary>Get or set the HL7 version number. If it does not exist, returns null.</summary>
        public string Version
        {
            get => _message[1][12].Value;
            set => _message[1][12].Value = value;
        }
    }
}