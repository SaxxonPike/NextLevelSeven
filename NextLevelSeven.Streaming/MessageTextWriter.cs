using System.IO;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    /// <summary>A writer that writes textual HL7 messages, separated by blank lines.</summary>
    public class MessageTextWriter : MessageStreamWriter
    {
        /// <summary>TextWriter that operates on the base stream.</summary>
        protected readonly TextWriter Writer;

        /// <summary>Create a textual HL7 message writer using the specified stream to output data.</summary>
        /// <param name="baseStream">Stream to use as an output.</param>
        public MessageTextWriter(Stream baseStream)
            : base(baseStream)
        {
            Writer = new StreamWriter(baseStream);
        }

        /// <summary>If true, a blank line will be inserted before data is written.</summary>
        protected bool WriteBlankLine { get; private set; }

        /// <summary>Write one textual HL7 message.</summary>
        /// <param name="message">Message to write.</param>
        public override void Write(IMessage message)
        {
            if (WriteBlankLine)
            {
                Writer.WriteLine();
            }
            else
            {
                WriteBlankLine = true;
            }
            Writer.Write(Interpret(message));
        }
    }
}