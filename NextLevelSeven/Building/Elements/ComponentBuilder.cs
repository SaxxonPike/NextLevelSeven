using System.Collections.Generic;
using System.Linq;
using System.Text;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Represents an HL7 component.</summary>
    internal sealed class ComponentBuilder : DescendantBuilder, IComponentBuilder
    {
        /// <summary>Descendant builders.</summary>
        private readonly BuilderElementCache<SubcomponentBuilder> _subcomponents;

        /// <summary>Create a component builder using the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index of the component.</param>
        internal ComponentBuilder(Builder builder, int index)
            : base(builder, index)
        {
            _subcomponents = new BuilderElementCache<SubcomponentBuilder>(CreateSubcomponentBuilder);
        }

        private ComponentBuilder(IEncoding config, int index)
            : base(config, index)
        {
            _subcomponents = new BuilderElementCache<SubcomponentBuilder>(CreateSubcomponentBuilder);
        }

        /// <summary>Get a descendant subcomponent builder.</summary>
        /// <param name="index">Index within the component to get the builder from.</param>
        /// <returns>Subcomponent builder for the specified index.</returns>
        public new ISubcomponentBuilder this[int index] => _subcomponents[index];

        /// <summary>Get the number of subcomponents in this component, including subcomponents with no content.</summary>
        public override int ValueCount => _subcomponents.MaxKey;

        /// <summary>Get or set subcomponent content within this component.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _subcomponents[i].Value;
                }
            }
            set => SetSubcomponents(value.ToArray());
        }

        /// <summary>Get or set the component string.</summary>
        public override string Value
        {
            get
            {
                if (_subcomponents.Count == 0)
                {
                    return null;
                }

                var index = 1;
                var result = new StringBuilder();

                foreach (var subcomponent in _subcomponents.OrderedByKey)
                {
                    while (index < subcomponent.Key)
                    {
                        result.Append(Encoding.SubcomponentDelimiter);
                        index++;
                    }

                    if (subcomponent.Key > 0)
                    {
                        result.Append(subcomponent.Value);
                    }
                }

                return result.Length == 0
                    ? null
                    : result.ToString();
            }
            set => SetComponent(value);
        }

        /// <summary>Set this component's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder SetComponent(string value)
        {
            _subcomponents.Clear();
            var index = 1;

            if (value == null)
            {
                return this;
            }

            foreach (var subcomponent in value.Split(Encoding.SubcomponentDelimiter))
            {
                SetSubcomponent(index++, subcomponent);
            }

            return this;
        }

        /// <summary>Set a subcomponent's content.</summary>
        /// <param name="subcomponentIndex">Subcomponent index.</param>
        /// <param name="value">New value.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder SetSubcomponent(int subcomponentIndex, string value)
        {
            _subcomponents[subcomponentIndex].SetSubcomponent(value);
            return this;
        }

        /// <summary>Replace all subcomponents within this component.</summary>
        /// <param name="subcomponents">Subcomponent index.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder SetSubcomponents(params string[] subcomponents)
        {
            _subcomponents.Clear();
            if (subcomponents == null || subcomponents.Length == 0)
            {
                return this;
            }

            var index = 1;
            foreach (var subcomponent in subcomponents)
            {
                SetSubcomponent(index++, subcomponent);
            }
            return this;
        }

        /// <summary>Set a sequence of subcomponents within this component, beginning at the specified start index.</summary>
        /// <param name="startIndex">Subcomponent index to begin replacing at.</param>
        /// <param name="subcomponents">Values to replace with.</param>
        /// <returns>This ComponentBuilder, for chaining purposes.</returns>
        public IComponentBuilder SetSubcomponents(int startIndex, params string[] subcomponents)
        {
            if (subcomponents == null || subcomponents.Length == 0)
            {
                return this;
            }

            var index = startIndex;
            foreach (var subcomponent in subcomponents)
            {
                SetSubcomponent(index++, subcomponent);
            }
            return this;
        }

        /// <summary>Get the value at the specified index.</summary>
        /// <param name="subcomponent">Subcomponent to get value from.</param>
        /// <returns>Value at index. Null if not present.</returns>
        public string GetValue(int subcomponent = -1)
        {
            return subcomponent < 0 ? Value : _subcomponents[subcomponent].Value;
        }

        /// <summary>Get the values at the specified index.</summary>
        /// <param name="subcomponent">Subcomponent to get values from.</param>
        /// <returns>Value at index. Empty if not present.</returns>
        public IEnumerable<string> GetValues(int subcomponent = -1)
        {
            return subcomponent < 0 ? Values : _subcomponents[subcomponent].Value.Yield();
        }

        /// <summary>Deep clone this element.</summary>
        /// <returns>Clone of the element.</returns>
        public override IElement Clone()
        {
            return CloneComponent();
        }

        /// <summary>Deep clone this component.</summary>
        /// <returns>Clone of the component.</returns>
        IComponent IComponent.Clone()
        {
            return CloneComponent();
        }

        /// <summary>Get the subcomponent delimiter.</summary>
        public override char Delimiter => SubcomponentDelimiter;

        /// <summary>Get this element's subcomponents.</summary>
        IEnumerable<ISubcomponent> IComponent.Subcomponents
        {
            get
            {
                var count = ValueCount;
                for (var i = 1; i <= count; i++)
                {
                    yield return _subcomponents[i];
                }
            }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists => _subcomponents.AnyExists;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IRepetition IComponent.Ancestor => Ancestor as IRepetition;

        /// <summary>Get this element's heirarchy-specific ancestor builder.</summary>
        IRepetitionBuilder IComponentBuilder.Ancestor => Ancestor as IRepetitionBuilder;

        /// <summary>Deep clone this component.</summary>
        /// <returns>Clone of the component.</returns>
        private ComponentBuilder CloneComponent()
        {
            return new ComponentBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };
        }

        /// <summary>Create a subcomponent builder object.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Subcomponent builder object.</returns>
        private SubcomponentBuilder CreateSubcomponentBuilder(int index)
        {
            return new SubcomponentBuilder(this, index);
        }

        /// <summary>Get the element at the specified index.</summary>
        /// <param name="index">Index to reference.</param>
        /// <returns>Element at index.</returns>
        protected override IElement GetGenericElement(int index)
        {
            return _subcomponents[index];
        }

        protected override IIndexedCache<Builder> GetCache()
        {
            return _subcomponents;
        }
    }
}