using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Represents an HL7 segment.
    /// </summary>
    public sealed class SegmentBuilder : BuilderBase
    {
        /// <summary>
        ///     Descendant builders.
        /// </summary>
        private readonly Dictionary<int, FieldBuilder> _fieldBuilders = new Dictionary<int, FieldBuilder>();

        /// <summary>
        ///     Create a segment builder with the specified encoding configuration.
        /// </summary>
        /// <param name="encodingConfiguration">Message's encoding configuration.</param>
        internal SegmentBuilder(EncodingConfiguration encodingConfiguration)
            : base(encodingConfiguration)
        {
            FieldDelimiter = '|';
        }

        /// <summary>
        ///     Get a descendant field builder.
        /// </summary>
        /// <param name="index">Index within the segment to get the builder from.</param>
        /// <returns>Field builder for the specified index.</returns>
        public FieldBuilder this[int index]
        {
            get
            {
                if (!_fieldBuilders.ContainsKey(index))
                {
                    _fieldBuilders[index] = new FieldBuilder(EncodingConfiguration);
                }
                return _fieldBuilders[index];
            }
        }

        /// <summary>
        ///     Get the number of fields in this segment, including fields with no content.
        /// </summary>
        public int Count
        {
            get { return (_fieldBuilders.Count > 0) ? _fieldBuilders.Max(kv => kv.Key) + 1 : 0; }
        }

        /// <summary>
        ///     Get or set the three-letter type field of this segment.
        /// </summary>
        public string Type
        {
            get { return this[0].ToString(); }
            set { Field(0, value); }
        }

        /// <summary>
        ///     Get or set field content within this segment.
        /// </summary>
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

        /// <summary>
        ///     Set a component's content.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Component(int fieldIndex, int repetition, int componentIndex, string value)
        {
            this[fieldIndex].Component(repetition, componentIndex, value);
            return this;
        }

        /// <summary>
        ///     Replace all component values within a field repetition.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Components(int fieldIndex, int repetition, params string[] components)
        {
            this[fieldIndex].Components(repetition, components);
            return this;
        }

        /// <summary>
        ///     Set a sequence of components within a field repetition, beginning at the specified start index.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Components(int fieldIndex, int repetition, int startIndex, params string[] components)
        {
            this[fieldIndex].Components(repetition, startIndex, components);
            return this;
        }

        /// <summary>
        ///     Set a field's content.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Field(int fieldIndex, string value)
        {
            if (_fieldBuilders.ContainsKey(fieldIndex))
            {
                _fieldBuilders.Remove(fieldIndex);
            }
            this[fieldIndex].FieldRepetition(1, value);
            return this;
        }

        /// <summary>
        ///     Replace all field values within this segment.
        /// </summary>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
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

        /// <summary>
        ///     Set a sequence of fields within this segment, beginning at the specified start index.
        /// </summary>
        /// <param name="startIndex">Field index to begin replacing at.</param>
        /// <param name="fields">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Fields(int startIndex, params string[] fields)
        {
            var index = startIndex;
            foreach (var field in fields)
            {
                Field(index++, field);
            }
            return this;
        }

        /// <summary>
        ///     Set a field repetition's content.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder FieldRepetition(int fieldIndex, int repetition, string value)
        {
            this[fieldIndex].FieldRepetition(repetition, value);
            return this;
        }

        /// <summary>
        ///     Replace all field repetitions within a field.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder FieldRepetitions(int fieldIndex, params string[] repetitions)
        {
            this[fieldIndex].FieldRepetitions(repetitions);
            return this;
        }

        /// <summary>
        ///     Set a sequence of field repetitions within a field, beginning at the specified start index.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder FieldRepetitions(int fieldIndex, int startIndex, params string[] repetitions)
        {
            this[fieldIndex].FieldRepetitions(startIndex, repetitions);
            return this;
        }

        /// <summary>
        ///     Set this segment's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Segment(string value)
        {
            if (value.Length > 3)
            {
                FieldDelimiter = value[3];
                return Fields(value.Split(FieldDelimiter));
            }

            return this;
        }

        /// <summary>
        ///     Set a subcomponent's content.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Subcomponent(int fieldIndex, int repetition, int componentIndex, int subcomponentIndex,
            string value)
        {
            this[fieldIndex].Subcomponent(repetition, componentIndex, subcomponentIndex, value);
            return this;
        }

        /// <summary>
        ///     Replace all subcomponents within a component.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Subcomponents(int fieldIndex, int repetition, int componentIndex,
            params string[] subcomponents)
        {
            this[fieldIndex].Subcomponents(repetition, componentIndex, subcomponents);
            return this;
        }

        /// <summary>
        ///     Set a sequence of subcomponents within a component, beginning at the specified start index.
        /// </summary>
        /// <param name="fieldIndex">Field index.</param>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This SegmentBuilder, for chaining purposes.</returns>
        public SegmentBuilder Subcomponents(int fieldIndex, int repetition, int componentIndex, int startIndex,
            params string[] subcomponents)
        {
            this[fieldIndex].Subcomponents(repetition, componentIndex, startIndex, subcomponents);
            return this;
        }

        /// <summary>
        ///     Copy the contents of this builder to a string.
        /// </summary>
        /// <returns>Converted segment.</returns>
        public override string ToString()
        {
            var index = 0;
            var result = new StringBuilder();

            if (_fieldBuilders.Count <= 0)
            {
                return string.Empty;
            }

            var typeIsMsh = (_fieldBuilders.ContainsKey(0) && _fieldBuilders[0].ToString() == "MSH");

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