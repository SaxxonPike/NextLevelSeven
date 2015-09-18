using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     Represents an HL7 field repetition.
    /// </summary>
    internal sealed class RepetitionBuilder : BuilderBaseDescendant, IRepetitionBuilder
    {
        /// <summary>
        ///     Descendant builders.
        /// </summary>
        private readonly IndexedCache<int, ComponentBuilder> _cache;

        /// <summary>
        ///     Create a repetition builder using the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        /// <param name="value">Default value.</param>
        internal RepetitionBuilder(BuilderBase builder, int index, string value = null)
            : base(builder, index)
        {
            _cache = new IndexedCache<int, ComponentBuilder>(CreateComponentBuilder);
            if (value != null)
            {
                Value = value;
            }
        }

        /// <summary>
        ///     Get a descendant component builder.
        /// </summary>
        /// <param name="index">Index within the field repetition to get the builder from.</param>
        /// <returns>Component builder for the specified index.</returns>
        public new IComponentBuilder this[int index]
        {
            get { return _cache[index]; }
        }

        private ComponentBuilder CreateComponentBuilder(int index)
        {
            return new ComponentBuilder(this, index);
        }

        /// <summary>
        ///     Get the number of components in this field repetition, including components with no content.
        /// </summary>
        public override int ValueCount
        {
            get { return _cache.Max(kv => kv.Key); }
        }

        /// <summary>
        ///     Get or set component content within this field repetition.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => _cache[index].Value,
                    (index, data) => Component(index, data),
                    () => ValueCount,
                    1);
            }
            set { Components(value.ToArray()); }
        }

        /// <summary>
        ///     Get or set the field repetition string.
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

                foreach (var component in _cache.OrderBy(i => i.Key))
                {
                    while (index < component.Key)
                    {
                        result.Append(EncodingConfiguration.ComponentDelimiter);
                        index++;
                    }

                    if (component.Key > 0)
                    {
                        result.Append(component.Value);
                    }
                }

                return result.ToString();
            }
            set { FieldRepetition(value); }
        }

        /// <summary>
        ///     Set a component's content.
        /// </summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder Component(int componentIndex, string value)
        {
            _cache[componentIndex].Component(value);
            return this;
        }

        /// <summary>
        ///     Replace all component values within this field repetition.
        /// </summary>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder Components(params string[] components)
        {
            _cache.Clear();
            var index = 1;
            foreach (var component in components)
            {
                Component(index++, component);
            }
            return this;
        }

        /// <summary>
        ///     Set a sequence of components within this field repetition, beginning at the specified start index.
        /// </summary>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder Components(int startIndex, params string[] components)
        {
            var index = startIndex;
            foreach (var component in components)
            {
                Component(index++, component);
            }
            return this;
        }

        /// <summary>
        ///     Set a field repetition's value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder FieldRepetition(string value)
        {
            if (value == null)
            {
                _cache.Clear();
            }
            else
            {
                Components(value.Split(ComponentDelimiter));
            }
            return this;
        }

        /// <summary>
        ///     Set a subcomponent's content.
        /// </summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder Subcomponent(int componentIndex, int subcomponentIndex, string value)
        {
            _cache[componentIndex].Subcomponent(subcomponentIndex, value);
            return this;
        }

        /// <summary>
        ///     Replace all subcomponents within a component.
        /// </summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder Subcomponents(int componentIndex, params string[] subcomponents)
        {
            _cache[componentIndex].Subcomponents(subcomponents);
            return this;
        }

        /// <summary>
        ///     Set a sequence of subcomponents within a component, beginning at the specified start index.
        /// </summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder Subcomponents(int componentIndex, int startIndex, params string[] subcomponents)
        {
            _cache[componentIndex].Subcomponents(startIndex, subcomponents);
            return this;
        }

        /// <summary>
        ///     Get the value at the specified indices.
        /// </summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Data.</returns>
        public string GetValue(int component = -1, int subcomponent = -1)
        {
            return component < 0
                ? Value
                : _cache[component].GetValue(subcomponent);
        }

        /// <summary>
        ///     Get the values at the specified indices.
        /// </summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>All occurrences.</returns>
        public IEnumerable<string> GetValues(int component = -1, int subcomponent = -1)
        {
            return component < 0
                ? Values
                : _cache[component].GetValues(subcomponent);
        }

        public override IElement Clone()
        {
            return new RepetitionBuilder(Ancestor, Index, Value);
        }

        IRepetition IRepetition.Clone()
        {
            return new RepetitionBuilder(Ancestor, Index, Value);
        }

        public override IEncodedTypeConverter As
        {
            get { return new BuilderCodec(this); }
        }

        public override char Delimiter
        {
            get { return ComponentDelimiter; }
        }

        protected override IElement GetGenericElement(int index)
        {
            return _cache[index];
        }
    }
}