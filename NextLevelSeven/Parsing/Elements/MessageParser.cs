using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Core.Properties;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents a textual HL7v2 message.
    /// </summary>
    internal sealed class MessageParser : Parser, IMessageParser
    {
        /// <summary>
        ///     Segment cache.
        /// </summary>
        private readonly IndexedCache<int, SegmentParser> _segments;

        /// <summary>
        ///     Create a message with a default MSH segment.
        /// </summary>
        public MessageParser()
        {
            _segments = new IndexedCache<int, SegmentParser>(CreateSegment);
            Value = @"MSH|^~\&|";
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
        public IElementParser GetElement(int segment, int field = -1, int repetition = -1, int component = -1,
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
        ///     Get the root message for this element.
        /// </summary>
        ISegmentParser IMessageParser.this[int index]
        {
            get { return GetSegment(index); }
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
            return EncodingOperations.Escape(Encoding, data);
        }

        /// <summary>
        ///     Get a string that has been unescaped from HL7.
        /// </summary>
        /// <param name="data">Data to unescape.</param>
        /// <returns>Unescaped string.</returns>
        public string UnEscape(string data)
        {
            return EncodingOperations.UnEscape(Encoding, data);
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
        ///     Get all segments.
        /// </summary>
        public IEnumerable<ISegmentParser> Segments
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _segments[i];
                }
            }
        }

        /// <summary>
        ///     Get all segments.
        /// </summary>
        IEnumerable<ISegment> IMessage.Segments
        {
            get { return Segments; }
        }

        /// <summary>
        ///     Get or set the value of this message.
        /// </summary>
        public override string Value
        {
            get { return base.Value; }
            set
            {
                if (value == null)
                {
                    throw new ParserException(ErrorCode.MessageDataMustNotBeNull);
                }
                if (value.Length < 8)
                {
                    throw new ParserException(ErrorCode.MessageDataIsTooShort);
                }
                if (!value.StartsWith("MSH"))
                {
                    throw new ParserException(ErrorCode.MessageDataMustStartWithMsh);
                }
                base.Value = SanitizeLineEndings(value);
            }
        }

        /// <summary>
        ///     Create a message with a default MSH segment.
        /// </summary>
        public static MessageParser Create()
        {
            return new MessageParser();
        }

        /// <summary>
        ///     Create a message using an HL7 data string.
        /// </summary>
        /// <param name="message">Message data to interpret.</param>
        public static MessageParser Create(string message)
        {
            return new MessageParser {Value = message};
        }

        /// <summary>
        ///     Get descendant element.
        /// </summary>
        /// <param name="index">Index of the element.</param>
        /// <returns></returns>
        public override IElementParser GetDescendant(int index)
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
        public IElementParser GetField(string segmentName, int field = -1, int repetition = -1, int component = -1,
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
        public IEnumerable<IElementParser> GetFields(string segmentName, int field = -1, int repetition = -1,
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
                ? (IEnumerable<IElementParser>) matches.Select(m => m[field][repetition][component])
                : matches.Select(m => m[field][repetition][component][subcomponent]);
        }

        /// <summary>
        ///     Get a descendant segment.
        /// </summary>
        /// <param name="index">Index of the segment.</param>
        /// <returns></returns>
        public ISegmentParser GetSegment(int index)
        {
            return _segments[index];
        }

        /// <summary>
        ///     Create a segment object.
        /// </summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Segment object.</returns>
        private SegmentParser CreateSegment(int index)
        {
            if (index < 1)
            {
                throw new ParserException(ErrorCode.SegmentIndexMustBeGreaterThanZero);
            }

            var result = new SegmentParser(this, index - 1, index);
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
        private string SanitizeLineEndings(string message)
        {
            return message == null
                ? null
                : message.Replace(Environment.NewLine, EncodingConfiguration.SegmentDelimiterString);
        }

        /// <summary>
        ///     Deep clone this message.
        /// </summary>
        /// <returns>Clone of the message.</returns>
        private MessageParser CloneInternal()
        {
            return new MessageParser {Value = Value, Index = Index};
        }
    }
}