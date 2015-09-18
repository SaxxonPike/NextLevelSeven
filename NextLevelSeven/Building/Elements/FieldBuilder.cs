﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     Represents an HL7 field.
    /// </summary>
    internal class FieldBuilder : BuilderBaseDescendant, IFieldBuilder
    {
        /// <summary>
        ///     Descendant builders.
        /// </summary>
        private readonly IndexedCache<int, RepetitionBuilder> _cache;

        /// <summary>
        ///     Create a field builder with the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        /// <param name="value">Default value.</param>
        internal FieldBuilder(BuilderBase builder, int index, string value = null)
            : base(builder, index)
        {
            _cache = new IndexedCache<int, RepetitionBuilder>(CreateRepetitionBuilder);
            if (value != null)
            {
                InitValue(value);
            }
        }

        /// <summary>
        ///     Get a descendant field repetition builder.
        /// </summary>
        /// <param name="index">Index within the field to get the builder from.</param>
        /// <returns>Field repetition builder for the specified index.</returns>
        public new IRepetitionBuilder this[int index]
        {
            get { return _cache[index]; }
        }

        /// <summary>
        ///     Create a repetition builder object.
        /// </summary>
        /// <param name="index">Index for the new object.</param>
        /// <returns>Repetition builder object.</returns>
        protected virtual RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            return new RepetitionBuilder(this, index);
        }

        /// <summary>
        ///     Get the number of field repetitions in this field, including field repetitions with no content.
        /// </summary>
        public override int ValueCount
        {
            get { return (_cache.Count > 0) ? _cache.Max(kv => kv.Key) : 0; }
        }

        /// <summary>
        ///     Get or set field repetition content within this field.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => _cache[index].Value,
                    (index, data) => FieldRepetition(index, data),
                    () => ValueCount,
                    1);
            }
            set { FieldRepetitions(value.ToArray()); }
        }

        /// <summary>
        ///     Get or set the field string.
        /// </summary>
        public override string Value
        {
            get
            {
                if (_cache.Count == 0)
                {
                    return null;
                }

                var index = 1;
                var result = new StringBuilder();

                foreach (var repetition in _cache.OrderBy(i => i.Key))
                {
                    while (index < repetition.Key)
                    {
                        result.Append(EncodingConfiguration.RepetitionDelimiter);
                        index++;
                    }

                    if (repetition.Key > 0)
                    {
                        result.Append(repetition.Value);
                    }
                }

                return result.ToString();
            }
            set { Field(value); }
        }

        /// <summary>
        ///     Set a component's content.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder Component(int repetition, int componentIndex, string value)
        {
            _cache[repetition].Component(componentIndex, value);
            return this;
        }

        /// <summary>
        ///     Replace all component values within a field repetition.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder Components(int repetition, params string[] components)
        {
            _cache[repetition].Components(components);
            return this;
        }

        /// <summary>
        ///     Set a sequence of components within a field repetition, beginning at the specified start index.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder Components(int repetition, int startIndex, params string[] components)
        {
            _cache[repetition].Components(startIndex, components);
            return this;
        }

        /// <summary>
        ///     Set this field's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public virtual IFieldBuilder Field(string value)
        {
            _cache.Clear();
            var index = 1;

            if (value == null)
            {
                return this;
            }

            foreach (var repetition in value.Split(EncodingConfiguration.RepetitionDelimiter))
            {
                FieldRepetition(index++, repetition);
            }

            return this;
        }

        /// <summary>
        ///     Set a field repetition's content.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public virtual IFieldBuilder FieldRepetition(int repetition, string value)
        {
            if (repetition < 1)
            {
                _cache.Clear();
            }
            else if (_cache.Contains(repetition))
            {
                _cache.Remove(repetition);
            }
            _cache[repetition].FieldRepetition(value);
            return this;
        }

        /// <summary>
        ///     Replace all field repetitions within this field.
        /// </summary>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder FieldRepetitions(params string[] repetitions)
        {
            _cache.Clear();
            var index = 1;
            foreach (var repetition in repetitions)
            {
                FieldRepetition(index++, repetition);
            }
            return this;
        }

        /// <summary>
        ///     Set a sequence of field repetitions within this field, beginning at the specified start index.
        /// </summary>
        /// <param name="startIndex">Field repetition index to begin replacing at.</param>
        /// <param name="repetitions">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder FieldRepetitions(int startIndex, params string[] repetitions)
        {
            var index = startIndex;
            foreach (var repetition in repetitions)
            {
                FieldRepetition(index++, repetition);
            }
            return this;
        }

        /// <summary>
        ///     Set a subcomponent's content.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder Subcomponent(int repetition, int componentIndex, int subcomponentIndex, string value)
        {
            _cache[repetition].Subcomponent(componentIndex, subcomponentIndex, value);
            return this;
        }

        /// <summary>
        ///     Replace all subcomponents within a component.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder Subcomponents(int repetition, int componentIndex, params string[] subcomponents)
        {
            _cache[repetition].Subcomponents(componentIndex, subcomponents);
            return this;
        }

        /// <summary>
        ///     Set a sequence of subcomponents within a component, beginning at the specified start index.
        /// </summary>
        /// <param name="repetition">Field repetition index.</param>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public IFieldBuilder Subcomponents(int repetition, int componentIndex, int startIndex,
            params string[] subcomponents)
        {
            _cache[repetition].Subcomponents(componentIndex, startIndex, subcomponents);
            return this;
        }

        /// <summary>
        ///     Get the value at the specified indices.
        /// </summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns></returns>
        public string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0
                ? Value
                : _cache[repetition].GetValue(component, subcomponent);
        }

        /// <summary>
        ///     Get the values at the specified indices.
        /// </summary>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns></returns>
        public IEnumerable<string> GetValues(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return repetition < 0
                ? Values
                : _cache[repetition].GetValues(component, subcomponent);
        }

        sealed public override IElement Clone()
        {
            return new FieldBuilder(Ancestor, Index, Value);
        }

        IField IField.Clone()
        {
            return new FieldBuilder(Ancestor, Index, Value);
        }

        sealed public override IEncodedTypeConverter As
        {
            get { return new BuilderCodec(this); }
        }

        sealed public override char Delimiter
        {
            get { return RepetitionDelimiter; }
        }

        /// <summary>
        ///     Initialize initial value.
        /// </summary>
        /// <param name="value"></param>
        private void InitValue(string value)
        {
            Value = value;
        }

        sealed protected override IElement GetGenericElement(int index)
        {
            return _cache[index];
        }
    }
}