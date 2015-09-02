using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    public sealed class MessageBuilder : BuilderBase
    {
        private readonly EncodingConfiguration _encodingConfiguration;
        private readonly Dictionary<int, SegmentBuilder> _segmentBuilders = new Dictionary<int, SegmentBuilder>();

        public MessageBuilder()
        {
            ComponentDelimiter = '^';
            EscapeDelimiter = '\\';
            RepetitionDelimiter = '~';
            SubcomponentDelimiter = '&';
            _encodingConfiguration = new BuilderEncodingConfiguration(this);
            Fields(1, "MSH", new string('|', 1),
                new string(new[] {ComponentDelimiter, RepetitionDelimiter, EscapeDelimiter, SubcomponentDelimiter}));
        }

        public MessageBuilder(string baseMessage)
        {
            _encodingConfiguration = new BuilderEncodingConfiguration(this);
            Message(baseMessage);
        }

        public SegmentBuilder this[int index]
        {
            get
            {
                if (!_segmentBuilders.ContainsKey(index))
                {
                    _segmentBuilders[index] = new SegmentBuilder(_encodingConfiguration);
                }
                return _segmentBuilders[index];
            }
        }

        public char ComponentDelimiter { get; set; }

        public int Count
        {
            get { return _segmentBuilders.Max(kv => kv.Key); }
        }

        public char EscapeDelimiter { get; set; }
        public char RepetitionDelimiter { get; set; }
        public char SubcomponentDelimiter { get; set; }

        public IEnumerableIndexable<int, string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].ToString(),
                    (index, data) => Segment(index, data),
                    () => Count,
                    1);
            }
        }

        public MessageBuilder Component(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            string value)
        {
            this[segmentIndex].Component(fieldIndex, repetition, componentIndex, value);
            return this;
        }

        public MessageBuilder Components(int segmentIndex, int fieldIndex, int repetition, params string[] components)
        {
            this[segmentIndex].Components(fieldIndex, repetition, components);
            return this;
        }

        public MessageBuilder Components(int segmentIndex, int fieldIndex, int repetition, int startIndex,
            params string[] components)
        {
            this[segmentIndex].Components(fieldIndex, repetition, startIndex, components);
            return this;
        }

        public MessageBuilder Field(int segmentIndex, int fieldIndex, int repetition, string value)
        {
            this[segmentIndex].Field(fieldIndex, repetition, value);
            return this;
        }

        public MessageBuilder Fields(int segmentIndex, params string[] fields)
        {
            this[segmentIndex].Fields(fields);
            return this;
        }

        public MessageBuilder Fields(int segmentIndex, int startIndex, params string[] fields)
        {
            this[segmentIndex].Fields(startIndex, fields);
            return this;
        }

        public MessageBuilder FieldRepetition(int segmentIndex, int fieldIndex, int repetition, string value)
        {
            this[segmentIndex].FieldRepetition(fieldIndex, repetition, value);
            return this;
        }

        public MessageBuilder FieldRepetitions(int segmentIndex, int fieldIndex, params string[] repetitions)
        {
            this[segmentIndex].FieldRepetitions(fieldIndex, repetitions);
            return this;
        }

        public MessageBuilder FieldRepetitions(int segmentIndex, int fieldIndex, int startIndex,
            params string[] repetitions)
        {
            this[segmentIndex].FieldRepetitions(fieldIndex, startIndex, repetitions);
            return this;
        }

        public MessageBuilder Message(string value)
        {
            value = value ?? string.Empty;

            var length = value.Length;
            ComponentDelimiter = (length >= 5) ? value[5] : '^';
            EscapeDelimiter = (length >= 6) ? value[6] : '\\';
            RepetitionDelimiter = (length >= 7) ? value[7] : '~';
            SubcomponentDelimiter = (length >= 8) ? value[8] : '&';

            _segmentBuilders.Clear();
            value = value.Replace("\r\n", "\xD");
            var index = 1;

            foreach (var segment in value.Split('\xD'))
            {
                Segment(index++, segment);
            }

            return this;
        }

        public MessageBuilder Segment(int segmentIndex, string value)
        {
            this[segmentIndex].Segment(value);
            return this;
        }

        public MessageBuilder Segments(params string[] segments)
        {
            Message(string.Join("\xD", segments));
            return this;
        }

        public MessageBuilder Segments(int startIndex, params string[] segments)
        {
            var index = startIndex;
            foreach (var segment in segments)
            {
                Segment(index++, segment);
            }
            return this;
        }

        public MessageBuilder Subcomponent(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            int subcomponentIndex,
            string value)
        {
            this[segmentIndex].Subcomponent(fieldIndex, repetition, componentIndex, subcomponentIndex, value);
            return this;
        }

        public MessageBuilder Subcomponents(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents)
        {
            this[segmentIndex].Subcomponents(fieldIndex, repetition, componentIndex, subcomponents);
            return this;
        }

        public MessageBuilder Subcomponents(int segmentIndex, int fieldIndex, int repetition, int componentIndex,
            int startIndex, params string[] subcomponents)
        {
            this[segmentIndex].Subcomponents(fieldIndex, repetition, componentIndex, startIndex, subcomponents);
            return this;
        }

        public IMessage ToMessage()
        {
            return new Message(ToString());
        }

        public override string ToString()
        {
            var result = string.Join("\xD",
                _segmentBuilders.OrderBy(i => i.Key).Select(i => i.Value.ToString()));
            if (result == null || !result.StartsWith("MSH"))
            {
                throw new BuilderException(ErrorCode.MessageDataMustStartWithMsh);
            }
            return result;
        }
    }
}