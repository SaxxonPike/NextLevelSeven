using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Core.Properties;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Represents an HL7 message as discrete parts, which can be quickly modified and exported.</summary>
    internal sealed class MessageBuilder : Builder, IMessageBuilder
    {
        /// <summary>Descendant segments.</summary>
        private readonly BuilderElementCache<SegmentBuilder> _segments;

        /// <summary>Create a message builder with default MSH segment containing only encoding characters.</summary>
        public MessageBuilder()
        {
            _segments = new BuilderElementCache<SegmentBuilder>(CreateSegmentBuilder);
            ComponentDelimiter = '^';
            EscapeCharacter = '\\';
            RepetitionDelimiter = '~';
            SubcomponentDelimiter = '&';
            FieldDelimiter = '|';
            SetFields(1, "MSH", new string(FieldDelimiter, 1), new string(new[]
            {
                ComponentDelimiter, RepetitionDelimiter, EscapeCharacter, SubcomponentDelimiter
            }));
        }

        /// <summary>Get a descendant segment builder.</summary>
        /// <param name="index">Index within the message to get the builder from.</param>
        /// <returns>Segment builder for the specified index.</returns>
        public new ISegmentBuilder this[int index] => _segments[index];

        /// <summary>Get the number of segments in the message.</summary>
        public override int ValueCount => _segments.MaxKey;

        /// <summary>Get or set segment content within this message.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _segments[i].Value;
                }
            }
            set => SetSegments(value.ToArray());
        }

        /// <summary>Get or set the message string.</summary>
        public override string Value
        {
            get
            {
                var result = string.Join(ReadOnlyEncodingConfiguration.SegmentDelimiterString,
                    _segments.OrderedByKey
                        .Select(i => i.Value.Value ?? string.Empty));

                return result.Length == 0
                    ? null
                    : result;
            }
            set => SetMessage(value);
        }

        /// <summary>Set a component's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetComponent(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            string value)
        {
            _segments[segmentIndex].SetComponent(fieldIndex, repetition, componentIndex, value);
            return this;
        }

        /// <summary>Replace all component values within a field repetition.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetComponents(int segmentIndex, int fieldIndex, int repetition,
            params string[] components)
        {
            _segments[segmentIndex].SetComponents(fieldIndex, repetition, components);
            return this;
        }

        /// <summary>Set a sequence of components within a field repetition, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetComponents(int segmentIndex, int fieldIndex, int repetition, int startIndex,
            params string[] components)
        {
            _segments[segmentIndex].SetComponents(fieldIndex, repetition, startIndex, components);
            return this;
        }

        /// <summary>Set a field's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetField(int segmentIndex, int fieldIndex, string value)
        {
            _segments[segmentIndex].SetField(fieldIndex, value);
            return this;
        }

        /// <summary>Replace all field values within a segment.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetFields(int segmentIndex, params string[] fields)
        {
            _segments[segmentIndex].SetFields(fields);
            return this;
        }

        /// <summary>Set a sequence of fields within a segment, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="startIndex">Field index to begin replacing at.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetFields(int segmentIndex, int startIndex, params string[] fields)
        {
            _segments[segmentIndex].SetFields(startIndex, fields);
            return this;
        }

        /// <summary>Set a field repetition's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetFieldRepetition(int segmentIndex, int fieldIndex, int repetition, string value)
        {
            _segments[segmentIndex].SetFieldRepetition(fieldIndex, repetition, value);
            return this;
        }

        /// <summary>Replace all field repetitions within a field.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetFieldRepetitions(int segmentIndex, int fieldIndex, params string[] repetitions)
        {
            _segments[segmentIndex].SetFieldRepetitions(fieldIndex, repetitions);
            return this;
        }

        /// <summary>Set a sequence of field repetitions within a field, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetFieldRepetitions(int segmentIndex, int fieldIndex, int startIndex,
            params string[] repetitions)
        {
            _segments[segmentIndex].SetFieldRepetitions(fieldIndex, startIndex, repetitions);
            return this;
        }

        /// <summary>Set this message's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetMessage(string value)
        {
            if (value == null)
            {
                throw new ElementException(ErrorCode.MessageDataMustNotBeNull);
            }
            value = NormalizeLineEndings(value);

            var length = value.Length;
            if (length < 8)
            {
                throw new ElementException(ErrorCode.MessageDataIsTooShort);
            }

            if (!value.StartsWith("MSH"))
            {
                throw new ElementException(ErrorCode.MessageDataMustStartWithMsh);
            }

            ComponentDelimiter = length >= 5
                ? value[4]
                : '^';

            EscapeCharacter = length >= 6 
                ? value[5]
                : '\\';

            FieldDelimiter = length >= 3 
                ? value[3]
                : '|';

            RepetitionDelimiter = length >= 7 
                ? value[6]
                : '~';

            SubcomponentDelimiter = length >= 8 
                ? value[7]
                : '&';

            _segments.Clear();
            var index = 1;

            foreach (var segment in value.Split(ReadOnlyEncodingConfiguration.SegmentDelimiter))
            {
                SetSegment(index++, segment);
            }

            return this;
        }

        /// <summary>Set a segment's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetSegment(int segmentIndex, string value)
        {
            _segments[segmentIndex].SetSegment(value);
            return this;
        }

        /// <summary>Replace all segments within this message.</summary>
        /// <param name="segments">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetSegments(params string[] segments)
        {
            _segments.Clear();
            if (segments == null)
            {
                return this;
            }

            SetMessage(string.Join(ReadOnlyEncodingConfiguration.SegmentDelimiterString, segments));
            return this;
        }

        /// <summary>Set a sequence of segments within this message, beginning at the specified start index.</summary>
        /// <param name="startIndex">Segment index to begin replacing at.</param>
        /// <param name="segments">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetSegments(int startIndex, params string[] segments)
        {
            var index = startIndex;
            foreach (var segment in segments)
            {
                SetSegment(index++, segment);
            }
            return this;
        }

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetSubcomponent(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            int subcomponentIndex, string value)
        {
            _segments[segmentIndex].SetSubcomponent(fieldIndex, repetition, componentIndex, subcomponentIndex, value);
            return this;
        }

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetSubcomponents(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents)
        {
            _segments[segmentIndex].SetSubcomponents(fieldIndex, repetition, componentIndex, subcomponents);
            return this;
        }

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="segmentIndex">Segment index.</param>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This MessageBuilder, for chaining purposes.</returns>
        public IMessageBuilder SetSubcomponents(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            int startIndex, params string[] subcomponents)
        {
            _segments[segmentIndex].SetSubcomponents(fieldIndex, repetition, componentIndex, startIndex, subcomponents);
            return this;
        }

        /// <summary>Get the values at the specific location in the message.</summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified location. Returns null if not found.</returns>
        public IEnumerable<string> GetValues(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return segment < 0 ? Values : _segments[segment].GetValues(field, repetition, component, subcomponent);
        }

        /// <summary>Get the value at the specific location in the message.</summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition index.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Value at the specified location. Returns null if not found.</returns>
        public string GetValue(int segment = -1, int field = -1, int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return segment < 0 ? Value : _segments[segment].GetValue(field, repetition, component, subcomponent);
        }

        /// <summary>Deep clone the message builder.</summary>
        /// <returns>Clone of the message builder.</returns>
        public override IElement Clone()
        {
            return CloneMessage();
        }

        /// <summary>Deep clone the message.</summary>
        /// <returns>Clone of the message.</returns>
        IMessage IMessage.Clone()
        {
            return CloneMessage();
        }

        /// <summary>Get the message delimiter.</summary>
        public override char Delimiter => ReadOnlyEncodingConfiguration.SegmentDelimiter;

        /// <summary>Get a wrapper which can manipulate message details.</summary>
        public IMessageDetails Details => new MessageDetails(this);

        /// <summary>Get the message's segments.</summary>
        IEnumerable<ISegment> IMessage.Segments
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

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists => _segments.AnyExists;

        /// <summary>Deep clone the message.</summary>
        /// <returns>Clone of the message.</returns>
        private MessageBuilder CloneMessage()
        {
            return new MessageBuilder
            {
                Value = Value
            };
        }

        /// <summary>Change all system line endings to HL7 line endings.</summary>
        /// <param name="message">String to transform.</param>
        /// <returns>Normalized string.</returns>
        private static string NormalizeLineEndings(string message)
        {
            return Hl7StringOperations.NormalizeLineEndings(message, ReadOnlyEncodingConfiguration.SegmentDelimiter);
        }

        /// <summary>Create a segment builder object.</summary>
        /// <param name="index">Index to create the object for.</param>
        /// <returns>Segment builder object.</returns>
        private SegmentBuilder CreateSegmentBuilder(int index)
        {
            return new SegmentBuilder(this, index);
        }

        /// <summary>Get the element at the specified index.</summary>
        /// <param name="index">Desired index.</param>
        /// <returns>Element at the index.</returns>
        protected override IElement GetGenericElement(int index)
        {
            return _segments[index];
        }

        protected override IIndexedCache<Builder> GetCache()
        {
            return _segments;
        }
    }
}