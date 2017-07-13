using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Core.Properties;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a textual HL7v2 message.</summary>
    internal sealed class MessageParser : Parser, IMessageParser
    {
        /// <summary>Segment cache.</summary>
        private readonly IndexedCache<SegmentParser> _segments;

        /// <summary>Create a message with a default MSH segment.</summary>
        public MessageParser()
        {
            _segments = new WeakReferenceCache<SegmentParser>(CreateSegment);
            Value = @"MSH|^~\&|";
        }

        /// <summary>
        ///     Delete a descendant element.
        /// </summary>
        /// <param name="index">Index to delete.</param>
        public override void Delete(int index)
        {
            ThrowIfEncodingSegmentIndex(index);
            base.Delete(index);
        }

        /// <summary>
        ///     Move a descendant from one index to another.
        /// </summary>
        /// <param name="sourceIndex">Index to move from.</param>
        /// <param name="targetIndex">Index to move to.</param>
        public override void Move(int sourceIndex, int targetIndex)
        {
            ThrowIfEncodingSegmentIndex(sourceIndex, targetIndex);
            base.Move(sourceIndex, targetIndex);
        }

        /// <summary>
        ///     Insert an element's value at the specified index.
        /// </summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index at which to insert.</param>
        /// <returns></returns>
        public override IElement Insert(int index, IElement element)
        {
            ThrowIfEncodingSegmentIndex(index);
            return base.Insert(index, element);
        }

        /// <summary>
        ///     Insert a descendant value at the specified index.
        /// </summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index at which to insert.</param>
        /// <returns></returns>
        public override IElement Insert(int index, string value)
        {
            ThrowIfEncodingSegmentIndex(index);
            return base.Insert(index, value);
        }

        /// <summary>Get the segment delimiter.</summary>
        public override char Delimiter => '\xD';

        /// <summary>Get the root message for this element.</summary>
        ISegmentParser IMessageParser.this[int index] => _segments[index];

        /// <summary>Check for validity of the message. Returns true if the message can reasonably be parsed.</summary>
        /// <returns>True if the message can be parsed, false otherwise.</returns>
        public bool Validate()
        {
            var value = Value;
            return value != null && value.StartsWith("MSH");
        }

        /// <summary>Get data from a specific place in the message. Depth is determined by how many indices are specified.</summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The occurrences of the specified element.</returns>
        public IEnumerable<string> GetValues(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return segment < 0 ? Values : _segments[segment].GetValues(field, repetition, component, subcomponent);
        }

        /// <summary>Get data from a specific place in the message. Depth is determined by how many indices are specified.</summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public string GetValue(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return segment < 0 ? Value : _segments[segment].GetValue(field, repetition, component, subcomponent);
        }

        /// <summary>Deep clone this message.</summary>
        /// <returns>Clone of the message.</returns>
        public override IElement Clone()
        {
            return CloneMessage();
        }

        /// <summary>Deep clone this message.</summary>
        /// <returns>Clone of the message.</returns>
        IMessage IMessage.Clone()
        {
            return CloneMessage();
        }

        /// <summary>Access message details as a property set.</summary>
        public IMessageDetails Details => new MessageDetails(this);

        /// <summary>Get all segments.</summary>
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

        /// <summary>Get all segments.</summary>
        IEnumerable<ISegment> IMessage.Segments => Segments;

        /// <summary>Get or set the value of this message.</summary>
        public override string Value
        {
            get => base.Value;
            set
            {
                if (value == null)
                {
                    throw new ElementException(ErrorCode.MessageDataMustNotBeNull);
                }
                if (value.Length < 8)
                {
                    throw new ElementException(ErrorCode.MessageDataIsTooShort);
                }
                if (!value.StartsWith("MSH"))
                {
                    throw new ElementException(ErrorCode.MessageDataMustStartWithMsh);
                }
                base.Value = NormalizeLineEndings(value);
            }
        }

        /// <summary>Get descendant element.</summary>
        /// <param name="index">Index of the element.</param>
        /// <returns></returns>
        protected override IElementParser GetDescendant(int index)
        {
            return _segments[index];
        }

        /// <summary>Create a segment object.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Segment object.</returns>
        private SegmentParser CreateSegment(int index)
        {
            if (index < 1)
            {
                throw new ElementException(ErrorCode.SegmentIndexMustBeGreaterThanZero);
            }

            var result = new SegmentParser(this, index - 1, index);
            return result;
        }

        /// <summary>Change all system line endings to HL7 line endings.</summary>
        /// <param name="message">String to transform.</param>
        /// <returns>Normalized string.</returns>
        private static string NormalizeLineEndings(string message)
        {
            return Hl7StringOperations.NormalizeLineEndings(message, ReadOnlyEncodingConfiguration.SegmentDelimiter);
        }

        /// <summary>Deep clone this message.</summary>
        /// <returns>Clone of the message.</returns>
        private MessageParser CloneMessage()
        {
            return new MessageParser
            {
                Value = Value,
                Index = Index
            };
        }

        /// <summary>
        ///     Enforce not being able to modify placement for encoding fields.
        /// </summary>
        private static void ThrowIfEncodingSegmentIndex(params int[] indices)
        {
            if (indices.Any(index => index <= 1))
            {
                throw new ElementException(ErrorCode.EncodingElementCannotBeMoved);
            }
        }
    }
}