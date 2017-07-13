using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Represents an HL7 field.</summary>
    internal class FieldBuilder : DescendantBuilder, IFieldBuilder
    {
        /// <summary>Descendant builders.</summary>
        private readonly BuilderElementCache<RepetitionBuilder> _repetitions;

        /// <summary>Create a field builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal FieldBuilder(Builder builder, int index)
            : base(builder, index)
        {
            _repetitions = new BuilderElementCache<RepetitionBuilder>(CreateRepetitionBuilder);
        }

        private FieldBuilder(IEncoding config, int index)
            : base(config, index)
        {
            _repetitions = new BuilderElementCache<RepetitionBuilder>(CreateRepetitionBuilder);
        }

        /// <summary>Get a descendant field repetition builder.</summary>
        /// <param name="index">Index within the field to get the builder from.</param>
        /// <returns>Field repetition builder for the specified index.</returns>
        public new IRepetitionBuilder this[int index] => _repetitions[index];

        /// <summary>Get the number of field repetitions in this field, including field repetitions with no content.</summary>
        public override int ValueCount => _repetitions.Count > 0
            ? _repetitions.MaxKey
            : 0;

        /// <summary>Get or set field repetition content within this field.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _repetitions[i].Value;
                }
            }
            set => SetFieldRepetitions((value ?? Enumerable.Empty<string>()).ToArray());
        }

        /// <summary>Get or set the field string.</summary>
        public override string Value
        {
            get
            {
                if (_repetitions.Count == 0)
                {
                    return null;
                }

                var index = 1;
                var result = new StringBuilder();

                foreach (var repetition in _repetitions.OrderedByKey)
                {
                    while (index < repetition.Key)
                    {
                        result.Append(Encoding.RepetitionDelimiter);
                        index++;
                    }

                    if (repetition.Key > 0)
                    {
                        result.Append(repetition.Value);
                    }
                }

                return result.Length == 0
                    ? null
                    : result.ToString();
            }
            set => SetField(value);
        }

        /// <summary>Set a component's content.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetComponent(int repetition, int componentIndex, string value)
        {
            _repetitions[repetition].SetComponent(componentIndex, value);
            return this;
        }

        /// <summary>Replace all component values within a field repetition.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetComponents(int repetition, params string[] components)
        {
            _repetitions[repetition].SetComponents(components);
            return this;
        }

        /// <summary>Set a sequence of components within a field repetition, beginning at the specified start index.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetComponents(int repetition, int startIndex, params string[] components)
        {
            _repetitions[repetition].SetComponents(startIndex, components);
            return this;
        }

        /// <summary>Set this field's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public virtual IFieldBuilder SetField(string value)
        {
            _repetitions.Clear();
            var index = 1;

            if (value == null)
            {
                return this;
            }

            foreach (var repetition in value.Split(Encoding.RepetitionDelimiter))
            {
                SetFieldRepetition(index++, repetition);
            }

            return this;
        }

        /// <summary>Set a field repetition's content.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public virtual IFieldBuilder SetFieldRepetition(int repetition, string value)
        {
            _repetitions.Remove(repetition);
            _repetitions[repetition].SetFieldRepetition(value);
            return this;
        }

        /// <summary>Replace all field repetitions within this field.</summary>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetFieldRepetitions(params string[] repetitions)
        {
            _repetitions.Clear();
            if (repetitions == null)
            {
                return this;
            }

            var index = 1;
            foreach (var repetition in repetitions)
            {
                SetFieldRepetition(index++, repetition);
            }
            return this;
        }

        /// <summary>Set a sequence of field repetitions within this field, beginning at the specified start index.</summary>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetFieldRepetitions(int startIndex, params string[] repetitions)
        {
            if (repetitions == null)
            {
                return this;
            }

            var index = startIndex;
            foreach (var repetition in repetitions)
            {
                SetFieldRepetition(index++, repetition);
            }
            return this;
        }

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetSubcomponent(int repetition, int componentIndex, int subcomponentIndex, string value)
        {
            _repetitions[repetition].SetSubcomponent(componentIndex, subcomponentIndex, value);
            return this;
        }

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetSubcomponents(int repetition, int componentIndex, params string[] subcomponents)
        {
            _repetitions[repetition].SetSubcomponents(componentIndex, subcomponents);
            return this;
        }

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder SetSubcomponents(int repetition, int componentIndex, int startIndex,
            params string[] subcomponents)
        {
            _repetitions[repetition].SetSubcomponents(componentIndex, startIndex, subcomponents);
            return this;
        }

        /// <summary>Get the value at the specified indices.</summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns></returns>
        public string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0 ? Value : _repetitions[repetition].GetValue(component, subcomponent);
        }

        /// <summary>Get the values at the specified indices.</summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns></returns>
        public IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0 ? Values : _repetitions[repetition].GetValues(component, subcomponent);
        }

        /// <summary>Deep clone this element.</summary>
        /// <returns>Clone of the element.</returns>
        public sealed override IElement Clone()
        {
            return CloneField();
        }

        /// <summary>Deep clone this field.</summary>
        /// <returns>Clone of the field.</returns>
        IField IField.Clone()
        {
            return CloneField();
        }

        /// <summary>Get this element's value delimiter.</summary>
        public override char Delimiter => RepetitionDelimiter;

        /// <summary>Get this element's field repetitions.</summary>
        IEnumerable<IRepetition> IField.Repetitions
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _repetitions[i];
                }
            }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists => _repetitions.AnyExists;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        ISegment IField.Ancestor => Ancestor as ISegment;

        /// <summary>Get this element's heirarchy-specific ancestor builder.</summary>
        ISegmentBuilder IFieldBuilder.Ancestor => Ancestor as ISegmentBuilder;

        /// <summary>Deep clone this field.</summary>
        /// <returns>Clone of the field.</returns>
        private FieldBuilder CloneField()
        {
            return new FieldBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };
        }

        /// <summary>Create a repetition builder object.</summary>
        /// <param name="index">Index for the new object.</param>
        /// <returns>Repetition builder object.</returns>
        protected virtual RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            return new RepetitionBuilder(this, index);
        }

        /// <summary>Get the element at the specified index.</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected sealed override IElement GetGenericElement(int index)
        {
            return _repetitions[index];
        }

        protected override IIndexedCache<Builder> GetCache()
        {
            return _repetitions;
        }
    }
}