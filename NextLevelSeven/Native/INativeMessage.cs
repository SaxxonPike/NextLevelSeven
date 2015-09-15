using System;
using System.Collections.Generic;
using NextLevelSeven.Core;

namespace NextLevelSeven.Native
{
    /// <summary>
    ///     Common interface for the highest level element in an HL7 message: the message itself.
    /// </summary>
    public interface INativeMessage : INativeElement, IMessage
    {
        /// <summary>
        ///     Get a descendant segment at the specified index. Indices match the HL7 specification, and are not necessarily
        ///     zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        new INativeSegment this[int index] { get; }

        /// <summary>
        ///     Get segments of a specific segment type.
        /// </summary>
        /// <param name="segmentType">The 3-character segment type to query for.</param>
        /// <returns>Segments that match the query.</returns>
        IEnumerable<INativeSegment> this[string segmentType] { get; }

        /// <summary>
        ///     Get segments of a type that matches one of the specified segment types. They are returned in the order they are
        ///     found in the message.
        /// </summary>
        /// <param name="segmentTypes">The 3-character segment types to query for.</param>
        /// <returns>Segments that match the query.</returns>
        IEnumerable<INativeSegment> this[IEnumerable<string> segmentTypes] { get; }

        /// <summary>
        ///     Get or set the message control ID.
        /// </summary>
        string ControlId { get; set; }

        /// <summary>
        ///     Get or set the message processing ID.
        /// </summary>
        string ProcessingId { get; set; }

        /// <summary>
        ///     Get the receiving application and facility information.
        /// </summary>
        IIdentity Receiver { get; }

        /// <summary>
        ///     Get or set the security code.
        /// </summary>
        string Security { get; set; }

        /// <summary>
        ///     Get all segments in the message.
        /// </summary>
        IEnumerable<INativeSegment> Segments { get; }

        /// <summary>
        ///     Get the sending application and facility information.
        /// </summary>
        IIdentity Sender { get; }

        /// <summary>
        ///     Get or set the date/time of the message.
        /// </summary>
        DateTimeOffset? Time { get; set; }

        /// <summary>
        ///     Get or set the 3-character trigger event.
        /// </summary>
        string TriggerEvent { get; set; }

        /// <summary>
        ///     Get or set the 3-character message type.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///     Get the HL7 version number. If it does not exist, returns null.
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Get an escaped version of the string, using encoding characters from this message.
        /// </summary>
        /// <param name="data">Data to escape.</param>
        /// <returns>Escaped data.</returns>
        string Escape(string data);

        /// <summary>
        ///     Get a string that has been unescaped from HL7.
        /// </summary>
        /// <param name="data">Data to unescape.</param>
        /// <returns>Unescaped string.</returns>
        string UnEscape(string data);

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        INativeElement GetField(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1);

        /// <summary>
        ///     Check for validity of the message. Returns true if the message can reasonably be parsed.
        /// </summary>
        /// <returns>True if the message can be parsed, false otherwise.</returns>
        bool Validate();
    }
}