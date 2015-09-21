using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     Represents an HL7 component.
    /// </summary>
    internal sealed class ComponentBuilder : BuilderBaseDescendant, IComponentBuilder
    {
        /// <summary>
        ///     Descendant builders.
        /// </summary>
        private readonly IndexedCache<int, SubcomponentBuilder> _cache;

        /// <summary>
        ///     Create a component builder using the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index of the component.</param>
        /// <param name="value">Default value of the component.</param>
        internal ComponentBuilder(BuilderBase builder, int index, string value = null)
            : base(builder, index)
        {
            _cache = new IndexedCache<int, SubcomponentBuilder>(CreateSubcomponentBuilder);
            if (value != null)
            {
                Value = value;
            }
        }

        /// <summary>
        ///     Get a descendant subcomponent builder.
        /// </summary>
        /// <param name="index">Index within the component to get the builder from.</param>
        /// <returns>Subcomponent builder for the specified index.</returns>
        public new ISubcomponentBuilder this[int index]
        {
            get
            {
                return _cache[index];
            }
        }

        /// <summary>
        ///     Create a subcomponent builder object.
        /// </summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Subcomponent builder object.</returns>
        private SubcomponentBuilder CreateSubcomponentBuilder(int index)
        {
            return new SubcomponentBuilder(this, index);
        }

        /// <summary>
        ///     Get the number of subcomponents in this component, including subcomponents with no content.
        /// </summary>
        public override int ValueCount
        {
            get { return _cache.Max(kv => kv.Key); }
        }

        /// <summary>
        ///     Get or set subcomponent content within this component.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].Value,
                    (index, data) => Subcomponent(index, data),
                    () => ValueCount,
                    1);
            }
            set { Subcomponents(value.ToArray()); }
        }

        /// <summary>
        ///     Get or set the component string.
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

                foreach (var subcomponent in _cache.OrderBy(i => i.Key))
                {
                    while (index < subcomponent.Key)
                    {
                        result.Append(EncodingConfiguration.SubcomponentDelimiter);
                        index++;
                    }

                    if (subcomponent.Key > 0)
                    {
                        result.Append(subcomponent.Value);
                    }
                }

                return result.ToString();
            }
            set { Component(value); }
        }

        /// <summary>
        ///     Set this component's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder Component(string value)
        {
            _cache.Clear();
            var index = 1;

            foreach (var subcomponent in value.Split(EncodingConfiguration.SubcomponentDelimiter))
            {
                Subcomponent(index++, subcomponent);
            }

            return this;
        }

        /// <summary>
        ///     Set a subcomponent's content.
        /// </summary>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder Subcomponent(int subcomponentIndex, string value)
        {
            this[subcomponentIndex].Subcomponent(value);
            return this;
        }

        /// <summary>
        ///     Replace all subcomponents within this component.
        /// </summary>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder Subcomponents(params string[] subcomponents)
        {
            _cache.Clear();
            var index = 1;
            foreach (var subcomponent in subcomponents)
            {
                Subcomponent(index++, subcomponent);
            }
            return this;
        }

        /// <summary>
        ///     Set a sequence of subcomponents within this component, beginning at the specified start index.
        /// </summary>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder Subcomponents(int startIndex, params string[] subcomponents)
        {
            var index = startIndex;
            foreach (var subcomponent in subcomponents)
            {
                Subcomponent(index++, subcomponent);
            }
            return this;
        }

        /// <summary>
        ///     Get the value at the specified index.
        /// </summary>
        /// <param name="subcomponent">Subcomponent to get value from.</param>
        /// <returns>Value at index. Null if not present.</returns>
        public string GetValue(int subcomponent = -1)
        {
            return subcomponent < 0
                ? Value
                : this[subcomponent].Value;
        }

        /// <summary>
        ///     Get the values at the specified index.
        /// </summary>
        /// <param name="subcomponent">Subcomponent to get values from.</param>
        /// <returns>Value at index. Empty if not present.</returns>
        public IEnumerable<string> GetValues(int subcomponent = -1)
        {
            return subcomponent < 0
                ? Values
                : this[subcomponent].Value.Yield();
        }

        /// <summary>
        ///     Deep clone this element.
        /// </summary>
        /// <returns>Clone of the element.</returns>
        public override IElement Clone()
        {
            return new ComponentBuilder(Ancestor, Index, Value);
        }

        /// <summary>
        ///     Deep clone this component.
        /// </summary>
        /// <returns>Clone of the component.</returns>
        IComponent IComponent.Clone()
        {
            return new ComponentBuilder(Ancestor, Index, Value);
        }

        /// <summary>
        ///     Get a codec which can be used to interpret this element's value as other types.
        /// </summary>
        public override IEncodedTypeConverter As
        {
            get { return new BuilderCodec(this); }
        }

        /// <summary>
        ///     Get the subcomponent delimiter.
        /// </summary>
        public override char Delimiter
        {
            get { return SubcomponentDelimiter; }
        }

        /// <summary>
        ///     Get the element at the specified index.
        /// </summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element at index.</returns>
        protected override IElement GetGenericElement(int index)
        {
            return _cache[index];
        }
    }
}