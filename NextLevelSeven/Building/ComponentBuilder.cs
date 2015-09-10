using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     Represents an HL7 component.
    /// </summary>
    internal sealed class ComponentBuilder : BuilderBaseDescendant, IComponentBuilder
    {
        /// <summary>
        ///     Descendant builders.
        /// </summary>
        private readonly Dictionary<int, SubcomponentBuilder> _subcomponentBuilders =
            new Dictionary<int, SubcomponentBuilder>();

        /// <summary>
        ///     Create a component builder using the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        internal ComponentBuilder(BuilderBase builder)
            : base(builder)
        {
        }

        /// <summary>
        ///     Get a descendant subcomponent builder.
        /// </summary>
        /// <param name="index">Index within the component to get the builder from.</param>
        /// <returns>Subcomponent builder for the specified index.</returns>
        public ISubcomponentBuilder this[int index]
        {
            get
            {
                if (!_subcomponentBuilders.ContainsKey(index))
                {
                    _subcomponentBuilders[index] = new SubcomponentBuilder(this);
                }
                return _subcomponentBuilders[index];
            }
        }

        /// <summary>
        ///     Get the number of subcomponents in this component, including subcomponents with no content.
        /// </summary>
        public int Count
        {
            get { return _subcomponentBuilders.Max(kv => kv.Key); }
        }

        /// <summary>
        ///     Get or set subcomponent content within this component.
        /// </summary>
        public IEnumerableIndexable<int, string> Values
        {
            get
            {
                return new WrapperEnumerable<string>(index => this[index].Value,
                    (index, data) => Subcomponent(index, data),
                    () => Count,
                    1);
            }
        }

        /// <summary>
        /// Get or set the component string.
        /// </summary>
        public string Value
        {
            get
            {
                var index = 1;
                var result = new StringBuilder();

                foreach (var subcomponent in _subcomponentBuilders.OrderBy(i => i.Key))
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
            _subcomponentBuilders.Clear();
            var index = 1;

            value = value ?? string.Empty;
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
            _subcomponentBuilders.Clear();
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
        ///     Copy the contents of this builder to a string.
        /// </summary>
        /// <returns>Converted component.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}