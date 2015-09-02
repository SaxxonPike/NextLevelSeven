using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    public sealed class FieldBuilder
    {
        private readonly EncodingConfiguration _encodingConfiguration;

        private readonly Dictionary<int, RepetitionBuilder> _repetitionBuilders =
            new Dictionary<int, RepetitionBuilder>();

        internal FieldBuilder(EncodingConfiguration encodingConfiguration)
        {
            _encodingConfiguration = encodingConfiguration;
        }

        public RepetitionBuilder this[int index]
        {
            get
            {
                if (!_repetitionBuilders.ContainsKey(index))
                {
                    _repetitionBuilders[index] = new RepetitionBuilder(_encodingConfiguration, index == 0);
                }
                return _repetitionBuilders[index];
            }
        }

        public int Count
        {
            get { return (_repetitionBuilders.Count > 0) ? _repetitionBuilders.Max(kv => kv.Key) : 0; }
        }

        public IEnumerableIndexable<int, string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].ToString(),
                    (index, data) => FieldRepetition(index, data),
                    () => Count,
                    1);
            }
        }

        public FieldBuilder Component(int repetition, int componentIndex, string value)
        {
            this[repetition].Component(componentIndex, value);
            return this;
        }

        public FieldBuilder Components(int repetition, params string[] components)
        {
            this[repetition].Components(components);
            return this;
        }

        public FieldBuilder Components(int repetition, int startIndex, params string[] components)
        {
            this[repetition].Components(startIndex, components);
            return this;
        }

        public FieldBuilder Field(string value)
        {
            _repetitionBuilders.Clear();
            var index = 1;

            value = value ?? string.Empty;
            foreach (var repetition in value.Split(_encodingConfiguration.RepetitionDelimiter))
            {
                FieldRepetition(index++, repetition);
            }

            return this;
        }

        public FieldBuilder FieldRepetition(int repetition, string value)
        {
            if (repetition < 1)
            {
                _repetitionBuilders.Clear();
            }
            else if (_repetitionBuilders.ContainsKey(repetition))
            {
                _repetitionBuilders.Remove(repetition);
            }
            this[repetition].Component(1, value);
            return this;
        }

        public FieldBuilder FieldRepetitions(params string[] repetitions)
        {
            _repetitionBuilders.Clear();
            var index = 1;
            foreach (var repetition in repetitions)
            {
                FieldRepetition(index++, repetition);
            }
            return this;
        }

        public FieldBuilder FieldRepetitions(int startIndex, params string[] repetitions)
        {
            var index = startIndex;
            foreach (var repetition in repetitions)
            {
                FieldRepetition(index++, repetition);
            }
            return this;
        }

        public FieldBuilder Subcomponent(int repetition, int componentIndex, int subcomponentIndex, string value)
        {
            this[repetition].Subcomponent(componentIndex, subcomponentIndex, value);
            return this;
        }

        public FieldBuilder Subcomponents(int repetition, int componentIndex, params string[] subcomponents)
        {
            this[repetition].Subcomponents(componentIndex, subcomponents);
            return this;
        }

        public FieldBuilder Subcomponents(int repetition, int componentIndex, int startIndex,
            params string[] subcomponents)
        {
            this[repetition].Subcomponents(componentIndex, startIndex, subcomponents);
            return this;
        }

        public override string ToString()
        {
            var index = 1;
            var result = new StringBuilder();

            foreach (var repetition in _repetitionBuilders.OrderBy(i => i.Key))
            {
                while (index < repetition.Key)
                {
                    result.Append(_encodingConfiguration.RepetitionDelimiter);
                    index++;
                }

                if (repetition.Key > 0)
                {
                    result.Append(repetition.Value);
                }
            }

            return result.ToString();
        }
    }
}