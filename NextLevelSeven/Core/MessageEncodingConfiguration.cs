using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    sealed internal class MessageEncodingConfiguration : EncodingConfiguration
    {
        /// <summary>
        /// Create an encoding configuration from a message or segment.
        /// </summary>
        /// <param name="messageElement">Message or segment to pull the characters from.</param>
        public MessageEncodingConfiguration(IElement messageElement)
        {
            Message = messageElement;
        }

        /// <summary>
        /// Get the delimiter character used to split components.
        /// </summary>
        override public char ComponentDelimiter
        {
            get { return Message.Value[4]; }
            protected set { }
        }

        /// <summary>
        /// Get the escape character used to mark encoded sequences.
        /// </summary>
        override public char EscapeDelimiter
        {
            get { return Message.Value[6]; }
            protected set { }
        }

        /// <summary>
        /// Internal message reference.
        /// </summary>
        IElement Message
        {
            get;
            set;
        }

        /// <summary>
        /// Get the repetition character used to separate multiple data in the same field.
        /// </summary>
        override public char RepetitionDelimiter
        {
            get { return Message.Value[5]; }
            protected set { }
        }

        /// <summary>
        /// Get the delimiter character used to split subcomponents.
        /// </summary>
        override public char SubcomponentDelimiter
        {
            get { return Message.Value[7]; }
            protected set { }
        }

        /// <summary>
        /// Prevent the value initialization process.
        /// </summary>
        protected override void Initialize()
        {
        }
    }
}
