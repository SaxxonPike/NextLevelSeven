using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    public sealed class SegmentBuilder : BuilderBase
    {
        private readonly EncodingConfiguration _encodingConfiguration;
        private readonly Dictionary<int, FieldBuilder> _fieldBuilders = new Dictionary<int, FieldBuilder>();

        internal SegmentBuilder(EncodingConfiguration encodingConfiguration)
        {
            _encodingConfiguration = encodingConfiguration;
            FieldDelimiter = '|';
        }

        public FieldBuilder this[int index]
        {
            get
            {
                if (!_fieldBuilders.ContainsKey(index))
                {
                    _fieldBuilders[index] = new FieldBuilder(_encodingConfiguration);
                }
                return _fieldBuilders[index];
            }
        }

        public int Count
        {
            get { return (_fieldBuilders.Count > 0) ? _fieldBuilders.Max(kv => kv.Key) + 1 : 0; }
        }

        public char FieldDelimiter { get; set; }

        public string Type
        {
            get { return this[0].ToString(); }
            set { Field(0, 0, value); }
        }

        public IEnumerableIndexable<int, string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].ToString(),
                    (index, data) => Field(index, data),
                    () => Count,
                    0);
            }
        }

        public SegmentBuilder Component(int fieldIndex, int repetition, int componentIndex, string value)
        {
            this[fieldIndex].Component(repetition, componentIndex, value);
            return this;
        }

        public SegmentBuilder Components(int fieldIndex, int repetition, params string[] components)
        {
            this[fieldIndex].Components(repetition, components);
            return this;
        }

        public SegmentBuilder Components(int fieldIndex, int repetition, int startIndex, params string[] components)
        {
            this[fieldIndex].Components(repetition, startIndex, components);
            return this;
        }

        public SegmentBuilder Field(int fieldIndex, string value)
        {
            if (_fieldBuilders.ContainsKey(fieldIndex))
            {
                _fieldBuilders.Remove(fieldIndex);
            }
            Field(fieldIndex, 1, value);
            return this;
        }

        public SegmentBuilder Field(int fieldIndex, int repetition, string value)
        {
            this[fieldIndex].FieldRepetition(repetition, value);
            return this;
        }

        public SegmentBuilder Fields(params string[] fields)
        {
            _fieldBuilders.Clear();
            var index = 0;

            foreach (var field in fields)
            {
                Field(index++, field);
            }

            return this;
        }

        public SegmentBuilder Fields(int startIndex, params string[] fields)
        {
            var index = startIndex;
            foreach (var field in fields)
            {
                Field(index++, field);
            }
            return this;
        }

        public SegmentBuilder FieldRepetition(int fieldIndex, int repetition, string value)
        {
            this[fieldIndex].FieldRepetition(repetition, value);
            return this;
        }

        public SegmentBuilder FieldRepetitions(int fieldIndex, params string[] repetitions)
        {
            this[fieldIndex].FieldRepetitions(repetitions);
            return this;
        }

        public SegmentBuilder FieldRepetitions(int fieldIndex, int startIndex, params string[] repetitions)
        {
            this[fieldIndex].FieldRepetitions(startIndex, repetitions);
            return this;
        }

        public SegmentBuilder Segment(string value)
        {
            if (value.Length > 3)
            {
                FieldDelimiter = value[3];
                return Fields(value.Split(FieldDelimiter));
            }

            return this;
        }

        public SegmentBuilder Subcomponent(int fieldIndex, int repetition, int componentIndex, int subcomponentIndex,
            string value)
        {
            this[fieldIndex].Subcomponent(repetition, componentIndex, subcomponentIndex, value);
            return this;
        }

        public SegmentBuilder Subcomponents(int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents)
        {
            this[fieldIndex].Subcomponents(repetition, componentIndex, subcomponents);
            return this;
        }

        public SegmentBuilder Subcomponents(int fieldIndex, int repetition, int componentIndex, int startIndex,
            params string[] subcomponents)
        {
            this[fieldIndex].Subcomponents(repetition, componentIndex, startIndex, subcomponents);
            return this;
        }

        public override string ToString()
        {
            var index = 0;
            var result = new StringBuilder();

            if (_fieldBuilders.Count <= 0)
            {
                return string.Empty;
            }

            if (!_fieldBuilders.ContainsKey(0))
            {
                throw new BuilderException(ErrorCode.SegmentBuilderHasInvalidSegmentType);
            }

            var type = _fieldBuilders[0].ToString();

            if (type == null || type.Length != 3)
            {
                throw new BuilderException(ErrorCode.SegmentBuilderHasInvalidSegmentType);
            }

            var typeIsMsh = (type == "MSH");

            foreach (var field in _fieldBuilders.OrderBy(i => i.Key))
            {
                if (field.Key < 0)
                {
                    continue;
                }

                while (index < field.Key)
                {
                    result.Append(FieldDelimiter);
                    index++;
                }

                if (typeIsMsh && index == 1)
                {
                    index++;
                }
                else
                {
                    result.Append(field.Value);
                }
            }

            return result.ToString();
        }
    }
}