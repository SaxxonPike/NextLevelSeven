using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Specification;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a textual HL7v2 message.
    /// </summary>
    internal sealed class NativeMessage : NativeElement, INativeMessage
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();
        private readonly EncodingConfiguration _encodingConfiguration;
        private Guid _keyGuid;

        /// <summary>
        ///     Create a message with a default MSH segment.
        /// </summary>
        public NativeMessage()
        {
            _encodingConfiguration = new NativeMessageEncodingConfiguration(this);
            Value = @"MSH|^~\&|";
        }

        /// <summary>
        ///     Create a message using an HL7 data string.
        /// </summary>
        /// <param name="message">Message data to interpret.</param>
        public NativeMessage(string message)
        {
            if (message == null)
            {
                throw new MessageException(ErrorCode.MessageDataMustNotBeNull);
            }
            if (!message.StartsWith("MSH"))
            {
                throw new MessageException(ErrorCode.MessageDataMustStartWithMsh);
            }
            if (message.Length < 9)
            {
                throw new MessageException(ErrorCode.MessageDataIsTooShort);
            }
            _encodingConfiguration = new NativeMessageEncodingConfiguration(this);
            Value = SanitizeLineEndings(message);
        }

        /// <summary>
        ///     Get the first MSH segment.
        /// </summary>
        private INativeElement Msh
        {
            get { return this["MSH"].First(); }
        }

        /// <summary>
        ///     Get the encoding configuration for the message.
        /// </summary>
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfiguration; }
        }

        /// <summary>
        ///     Get the key GUID.
        /// </summary>
        private Guid KeyGuid
        {
            get
            {
                if (_keyGuid == Guid.Empty)
                {
                    _keyGuid = Guid.NewGuid();
                }
                return _keyGuid;
            }
        }

        /// <summary>
        ///     Get segments of a specific segment type.
        /// </summary>
        /// <param name="segmentType">The 3-character segment type to query for.</param>
        /// <returns>Segments that match the query.</returns>
        public IEnumerable<INativeSegment> this[string segmentType]
        {
            get { return Segments.Where(s => s.Type == segmentType); }
        }

        /// <summary>
        ///     Get segments of a type that matches one of the specified segment types. They are returned in the order they are
        ///     found in the message.
        /// </summary>
        /// <param name="segmentTypes">The 3-character segment types to query for.</param>
        /// <returns>Segments that match the query.</returns>
        public IEnumerable<INativeSegment> this[IEnumerable<string> segmentTypes]
        {
            get { return Segments.Where(s => segmentTypes.Contains(s.Type)); }
        }

        /// <summary>
        ///     Get or set the message Control ID from MSH-10.
        /// </summary>
        public string ControlId
        {
            get { return Msh[10].Value; }
            set { Msh[10].Value = value; }
        }

        /// <summary>
        ///     Get the segment delimiter.
        /// </summary>
        public override char Delimiter
        {
            get { return '\xD'; }
        }

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public INativeElement GetField(int segment, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            if (field < 0)
            {
                return this[segment];
            }
            if (repetition < 0)
            {
                return this[segment][field];
            }
            if (component < 0)
            {
                return this[segment][field][repetition];
            }
            return subcomponent < 0
                ? this[segment][field][repetition][component]
                : this[segment][field][repetition][component][subcomponent];
        }

        /// <summary>
        ///     Returns a unique identifier for the message.
        /// </summary>
        public override string Key
        {
            get { return KeyGuid.ToString(); }
        }

        /// <summary>
        ///     Get the root message for this element.
        /// </summary>
        public override INativeMessage Message
        {
            get { return this; }
        }

        /// <summary>
        ///     Get the root message for this element.
        /// </summary>
        INativeSegment INativeMessage.this[int index]
        {
            get { return GetSegment(index); }
        }

        /// <summary>
        ///     Get or set the message Processing ID from MSH-11.
        /// </summary>
        public string ProcessingId
        {
            get { return Msh[11].Value; }
            set { Msh[11].Value = value; }
        }

        /// <summary>
        ///     Get the receiving application and facility.
        /// </summary>
        public IIdentity Receiver
        {
            get { return new NativeIdentity(Msh, 5, 6); }
        }

        /// <summary>
        ///     Get or set the message Security from MSH-8.
        /// </summary>
        public string Security
        {
            get { return Msh[8].Value; }
            set { Msh[8].Value = value; }
        }

        /// <summary>
        ///     Get all segments in the message.
        /// </summary>
        public IEnumerable<INativeSegment> Segments
        {
            get { return new NativeSegmentEnumerable(this); }
        }

        /// <summary>
        ///     Get the sending application and facility.
        /// </summary>
        public IIdentity Sender
        {
            get { return new NativeIdentity(Msh, 3, 4); }
        }

        /// <summary>
        ///     Get or set the message Time from MSH-7.
        /// </summary>
        public DateTimeOffset? Time
        {
            get { return NativeCodec.ConvertToDateTime(Msh[7].Value); }
            set { Msh[7].Value = NativeCodec.ConvertFromDateTime(value); }
        }

        /// <summary>
        ///     Get or set the message Trigger Event from MSH-9-2.
        /// </summary>
        public string TriggerEvent
        {
            get { return Msh[9][0][2].Value; }
            set { Msh[9][0][2].Value = value; }
        }

        /// <summary>
        ///     Get or set the message Trigger Event from MSH-9-1.
        /// </summary>
        public string Type
        {
            get { return Msh[9][0][1].Value; }
            set { Msh[9][0][1].Value = value; }
        }

        /// <summary>
        ///     Check for validity of the message. Returns true if the message can reasonably be parsed.
        /// </summary>
        /// <returns>True if the message can be parsed, false otherwise.</returns>
        public bool Validate()
        {
            var value = Value;
            return value != null && value.StartsWith("MSH");
        }

        /// <summary>
        ///     Get or set the message Version from MSH-12.
        /// </summary>
        public string Version
        {
            get { return Msh[12].Value; }
            set { Msh[12].Value = value; }
        }

        /// <summary>
        ///     Get an escaped version of the string, using encoding characters from this message.
        /// </summary>
        /// <param name="data">Data to escape.</param>
        /// <returns>Escaped data.</returns>
        public string Escape(string data)
        {
            return _encodingConfiguration.Escape(data);
        }

        /// <summary>
        ///     Get a string that has been unescaped from HL7.
        /// </summary>
        /// <param name="data">Data to unescape.</param>
        /// <returns>Unescaped string.</returns>
        public string UnEscape(string data)
        {
            return _encodingConfiguration.UnEscape(data);
        }

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        public IEnumerable<string> GetValues(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return segment < 0
                ? Values
                : GetSegment(segment).GetValues(field, repetition, component, subcomponent);
        }

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public string GetValue(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return segment < 0
                ? Value
                : GetSegment(segment).GetValue(field, repetition, component, subcomponent);
        }

        /// <summary>
        ///     Create a message with a default MSH segment.
        /// </summary>
        public static NativeMessage Create()
        {
            return new NativeMessage();
        }

        /// <summary>
        ///     Create a message using an HL7 data string.
        /// </summary>
        /// <param name="message">Message data to interpret.</param>
        public static NativeMessage Create(string message)
        {
            return new NativeMessage(message);
        }

        /// <summary>
        ///     Get descendant element.
        /// </summary>
        /// <param name="index">Index of the element.</param>
        /// <returns></returns>
        public override INativeElement GetDescendant(int index)
        {
            return _cache.ContainsKey(index)
                ? _cache[index]
                : GetSegment(index);
        }

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segmentName">Segment name.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public INativeElement GetField(string segmentName, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return GetFields(segmentName, field, repetition, component, subcomponent).FirstOrDefault();
        }

        /// <summary>
        ///     Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segmentName">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public IEnumerable<INativeElement> GetFields(string segmentName, int field = -1, int repetition = -1,
            int component = -1, int subcomponent = -1)
        {
            var matches = Segments.Where(s => s.Type == segmentName);
            if (field < 0)
            {
                return matches;
            }
            if (repetition < 0)
            {
                return matches.Select(m => m[field]);
            }
            if (component < 0)
            {
                return matches.Select(m => m[field][repetition]);
            }
            return (subcomponent < 0)
                ? matches.Select(m => (INativeElement) m[field][repetition][component])
                : matches.Select(m => (INativeElement) m[field][repetition][component][subcomponent]);
        }

        /// <summary>
        ///     Get a descendant segment.
        /// </summary>
        /// <param name="index">Index of the segment.</param>
        /// <returns></returns>
        public INativeSegment GetSegment(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SegmentIndexMustBeGreaterThanZero));
            }

            var result = new NativeSegment(this, index - 1, index);
            _cache[index] = result;
            return result;
        }

        /// <summary>
        ///     Determines whether this object is equivalent to another object.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>True, if objects are considered to be equivalent.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            return SanitizeLineEndings(obj.ToString()) == ToString();
        }

        /// <summary>
        ///     Get this message's hash code.
        /// </summary>
        /// <returns>Hash code for the message.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        ///     Change all system line endings to HL7 line endings.
        /// </summary>
        /// <param name="message">String to transform.</param>
        /// <returns>Sanitized string.</returns>
        private static string SanitizeLineEndings(string message)
        {
            return message == null
                ? null
                : message.Replace(Environment.NewLine, "\xD");
        }

        /// <summary>
        ///     Get the string representation of the message.
        /// </summary>
        /// <returns>Message as a string.</returns>
        public override string ToString()
        {
            return DescendantDivider == null
                ? string.Empty
                : DescendantDivider.Value;
        }

        override public IElement Clone()
        {
            return CloneInternal();
        }

        IMessage IMessage.Clone()
        {
            return CloneInternal();
        }

        private NativeMessage CloneInternal()
        {
            return new NativeMessage(Value) {Index = Index};
        }
    }
}