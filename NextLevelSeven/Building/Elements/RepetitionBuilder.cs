using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Represents an HL7 field repetition.</summary>
    internal sealed class RepetitionBuilder : DescendantBuilder, IRepetitionBuilder
    {
        /// <summary>Descendant builders.</summary>
        private readonly BuilderElementCache<ComponentBuilder> _components;

        /// <summary>Create a repetition builder using the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal RepetitionBuilder(Builder builder, int index)
            : base(builder, index)
        {
            _components = new BuilderElementCache<ComponentBuilder>(CreateComponentBuilder);
        }

        private RepetitionBuilder(IEncoding config, int index)
            : base(config, index)
        {
            _components = new BuilderElementCache<ComponentBuilder>(CreateComponentBuilder);
        }

        /// <summary>Get a descendant component builder.</summary>
        /// <param name="index">Index within the field repetition to get the builder from.</param>
        /// <returns>Component builder for the specified index.</returns>
        public new IComponentBuilder this[int index] => _components[index];

        /// <summary>Get the number of components in this field repetition, including components with no content.</summary>
        public override int ValueCount => _components.MaxKey;

        /// <summary>Get or set component content within this field repetition.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _components[i].Value;
                }
            }
            set => SetComponents(value.ToArray());
        }

        /// <summary>Get or set the field repetition string.</summary>
        public override string Value
        {
            get
            {
                if (_components.Count == 0)
                {
                    return null;
                }

                var index = 1;
                var result = new StringBuilder();

                foreach (var component in _components.OrderedByKey)
                {
                    while (index < component.Key)
                    {
                        result.Append(Encoding.ComponentDelimiter);
                        index++;
                    }

                    if (component.Key > 0)
                    {
                        result.Append(component.Value);
                    }
                }

                return result.Length == 0
                    ? null
                    : result.ToString();
            }
            set => SetFieldRepetition(value);
        }

        /// <summary>Set a component's content.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetComponent(int componentIndex, string value)
        {
            _components[componentIndex].SetComponent(value);
            return this;
        }

        /// <summary>Replace all component values within this field repetition.</summary>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetComponents(params string[] components)
        {
            _components.Clear();
            if (components == null)
            {
                return this;
            }

            var index = 1;
            foreach (var component in components)
            {
                SetComponent(index++, component);
            }
            return this;
        }

        /// <summary>Set a sequence of components within this field repetition, beginning at the specified start index.</summary>
        /// <param name="startIndex">Component index to begin replacing at.</param>
        /// <param name="components">Values to replace with.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetComponents(int startIndex, params string[] components)
        {
            if (components == null)
            {
                return this;
            }

            var index = startIndex;
            foreach (var component in components)
            {
                SetComponent(index++, component);
            }
            return this;
        }

        /// <summary>Set a field repetition's value.</summary>
        /// <param name="value"></param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetFieldRepetition(string value)
        {
            if (value == null)
            {
                _components.Clear();
            }
            else
            {
                SetComponents(value.Split(ComponentDelimiter));
            }
            return this;
        }

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetSubcomponent(int componentIndex, int subcomponentIndex, string value)
        {
            _components[componentIndex].SetSubcomponent(subcomponentIndex, value);
            return this;
        }

        /// <summary>Replace all subcomponents within a component.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetSubcomponents(int componentIndex, params string[] subcomponents)
        {
            _components[componentIndex].SetSubcomponents(subcomponents);
            return this;
        }

        /// <summary>Set a sequence of subcomponents within a component, beginning at the specified start index.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This RepetitionBuilder, for chaining purposes.</returns>
        public IRepetitionBuilder SetSubcomponents(int componentIndex, int startIndex, params string[] subcomponents)
        {
            _components[componentIndex].SetSubcomponents(startIndex, subcomponents);
            return this;
        }

        /// <summary>Get the value at the specified indices.</summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>Data.</returns>
        public string GetValue(int component = -1, int subcomponent = -1)
        {
            return component < 0 ? Value : _components[component].GetValue(subcomponent);
        }

        /// <summary>Get the values at the specified indices.</summary>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>All occurrences.</returns>
        public IEnumerable<string> GetValues(int component = -1, int subcomponent = -1)
        {
            return component < 0 ? Values : _components[component].GetValues(subcomponent);
        }

        /// <summary>Deep clone this element.</summary>
        /// <returns>Clone of the element.</returns>
        public override IElement Clone()
        {
            return CloneRepetition();
        }

        /// <summary>Deep clone this field repetition.</summary>
        /// <returns>Clone of the repetition.</returns>
        IRepetition IRepetition.Clone()
        {
            return CloneRepetition();
        }

        /// <summary>Get this element's value delimiter.</summary>
        public override char Delimiter => ComponentDelimiter;

        /// <summary>Get this element's components.</summary>
        IEnumerable<IComponent> IRepetition.Components
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _components[i];
                }
            }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists => _components.AnyExists;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IField IRepetition.Ancestor => Ancestor as IField;

        /// <summary>Get this element's heirarchy-specific ancestor builder.</summary>
        IFieldBuilder IRepetitionBuilder.Ancestor => Ancestor as IFieldBuilder;

        private RepetitionBuilder CloneRepetition()
        {
            return new RepetitionBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };
        }

        /// <summary>Create a component builder for the specified index.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>New component builder.</returns>
        private ComponentBuilder CreateComponentBuilder(int index)
        {
            return new ComponentBuilder(this, index);
        }

        /// <summary>Get the descendant element at the specified index.</summary>
        /// <param name="index">Index of the element.</param>
        /// <returns>Element at the index.</returns>
        protected override IElement GetGenericElement(int index)
        {
            return _components[index];
        }

        protected override IIndexedCache<Builder> GetCache()
        {
            return _components;
        }
    }
}