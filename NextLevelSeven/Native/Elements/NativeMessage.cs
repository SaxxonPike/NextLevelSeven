using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Core.Properties;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Native.Elements
{
    /// <summary>
    ///     Represents a textual HL7v2 message.
    /// </summary>
    internal sealed class NativeMessage : NativeElement, INativeMessage
    {
        private readonly IndexedCache<int, NativeSegment> _cache;

        /// <summary>
        ///     Internal backing field for encoding configuration.
        /// </summary>
        private readonly EncodingConfigurationBase _encodingConfiguration;

        /// <summary>
        ///     Cached Guid.
        /// </summary>
        private Guid _keyGuid;

        /// <summary>
        ///     Create a message with a default MSH segment.
        /// </summary>
        public NativeMessage()
        {
            _cache = new IndexedCache<int, NativeSegment>(CreateSegment);
            _encodingConfiguration = new NativeMessageEncodingConfiguration(this);
            Value = @"MSH|^~\&|";
        }

        /// <summary>
        ///     Create a message using an HL7 data string.
        /// </summary>
        /// <param name="message">Message data to interpret.</param>
        public NativeMessage(string message)
        {
            _cache = new IndexedCache<int, NativeSegment>(CreateSegment);
            if (message == null)
            {
                throw new MessageException(ErrorCode.MessageDataMustNotBeNull);
            }
            if (!message.StartsWith("MSH"))
            {
                throw new MessageException(ErrorCode.MessageDataMustStartWithMsh);
            }
            if (message.Length < 8)
            {
                throw new MessageException(ErrorCode.MessageDataIsTooShort);
            }
            _encodingConfiguration = new NativeMessageEncodingConfiguration(this);
            Value = SanitizeLineEndings(message);
        }

        /// <summary>
        ///     Create a message using an HL7 element's content.
        /// </summary>
        /// <param name="message">Message or other element data to interpret.</param>
        public NativeMessage(IElement message)
            : this(message.Value)
        {
        }

        /// <summary>
        ///     Get the encoding configuration for the message.
        /// </summary>
        public override EncodingConfigurationBase EncodingConfiguration
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
        ///     Get all segments in the message.
        /// </summary>
        public IEnumerable<INativeSegment> Segments
        {
            get { return new NativeSegmentEnumerable(this); }
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
        ///     Deep clone this message.
        /// </summary>
        /// <returns>Clone of the message.</returns>
        public override IElement Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Deep clone this message.
        /// </summary>
        /// <returns>Clone of the message.</returns>
        IMessage IMessage.Clone()
        {
            return CloneInternal();
        }

        /// <summary>
        ///     Access message details as a property set.
        /// </summary>
        public IMessageDetails Details
        {
            get { return new MessageDetails(this); }
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
            return GetSegment(index);
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
            return _cache[index];
        }

        /// <summary>
        ///     Create a segment object.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Segment object.</returns>
        private NativeSegment CreateSegment(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SegmentIndexMustBeGreaterThanZero));
            }

            var result = new NativeSegment(this, index - 1, index);
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
        ///     Get the hash code for this element.
        /// </summary>
        /// <returns>Hash code of the value's string.</returns>
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
        ///     Deep clone this message.
        /// </summary>
        /// <returns>Clone of the message.</returns>
        private NativeMessage CloneInternal()
        {
            return new NativeMessage(Value) {Index = Index};
        }
    }
}