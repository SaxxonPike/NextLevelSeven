using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Streaming
{
    /// <summary>
    /// An HL7StreamWriter that writes textual HL7 messages, separated by blank lines.
    /// </summary>
    public class HL7TextWriter : HL7StreamWriter
    {
        /// <summary>
        /// Create a textual HL7 message writer using the specified stream to output data.
        /// </summary>
        /// <param name="baseStream">Stream to use as an output.</param>
        public HL7TextWriter(Stream baseStream)
            : base(baseStream)
        {
            Writer = new StreamWriter(baseStream);
        }

        /// <summary>
        /// Write one textual HL7 message.
        /// </summary>
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

        /// <summary>
        /// If true, a blank line will be inserted before data is written.
        /// </summary>
        protected bool WriteBlankLine
        {
            get;
            private set;
        }

        /// <summary>
        /// TextWriter that operates on the base stream.
        /// </summary>
        protected TextWriter Writer
        {
            get;
            private set;
        }
    }
}
