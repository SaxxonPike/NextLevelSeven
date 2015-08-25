using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    public interface IMessage : IElement
    {
        /// <summary>
        /// Get segments of a specific segment type.
        /// </summary>
        /// <param name="segmentType">The 3-character segment type to query for.</param>
        /// <returns>Segments that match the query.</returns>
        IEnumerable<ISegment> this[string segmentType] { get; }

        /// <summary>
        /// Get segments of a type that matches one of the specified segment types. They are returned in the order they are found in the message.
        /// </summary>
        /// <param name="segmentTypes">The 3-character segment types to query for.</param>
        /// <returns>Segments that match the query.</returns>
        IEnumerable<ISegment> this[IEnumerable<string> segmentTypes] { get; }

        /// <summary>
        /// Create a deep clone of the message.
        /// </summary>
        /// <returns>The new message.</returns>
        IMessage Clone();

        /// <summary>
        /// Get or set the message control ID.
        /// </summary>
        string ControlId { get; set; }

        /// <summary>
        /// Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        IElement GetField(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>
        /// Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segmentName">Segment name.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        IElement GetField(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1);

        /// <summary>
        /// Get or set the message processing ID.
        /// </summary>
        string ProcessingId { get; set; }

        /// <summary>
        /// Get or set the receiving application code.
        /// </summary>
        string ReceivingApplication { get; set; }

        /// <summary>
        /// Get or set the receiving facility code.
        /// </summary>
        string ReceivingFacility { get; set; }

        /// <summary>
        /// Get or set the security code.
        /// </summary>
        string Security { get; set; }

        /// <summary>
        /// Get all segments in the message.
        /// </summary>
        IEnumerable<ISegment> Segments { get; }

        /// <summary>
        /// Get or set the sending application code.
        /// </summary>
        string SendingApplication { get; set; }

        /// <summary>
        /// Get or set the sending facility code.
        /// </summary>
        string SendingFacility { get; set; }

        /// <summary>
        /// Get or set the date/time of the message.
        /// </summary>
        DateTimeOffset? Time { get; set; }

        /// <summary>
        /// Get or set the 3-character trigger event.
        /// </summary>
        string TriggerEvent { get; set; }

        /// <summary>
        /// Get or set the 3-character message type.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// Check for validity of the message. Returns true if the message can reasonably be parsed.
        /// </summary>
        /// <returns>True if the message can be parsed, false otherwise.</returns>
        bool Validate();

        /// <summary>
        /// Get the HL7 version number. If it does not exist, returns null.
        /// </summary>
        string Version { get; }
    }
}
