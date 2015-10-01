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
        private readonly IndexedCache<SubcomponentBuilder> _subcomponents;

        /// <summary>Create a component builder using the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index of the component.</param>
        internal ComponentBuilder(Builder builder, int index)
            : base(builder, index)
        {
            _subcomponents = new IndexedCache<SubcomponentBuilder>(CreateSubcomponentBuilder);
        }

        private ComponentBuilder(IEncoding config, int index)
            : base(config, index)
        {
            _subcomponents = new IndexedCache<SubcomponentBuilder>(CreateSubcomponentBuilder);
        }

        /// <summary>Get a descendant subcomponent builder.</summary>
        /// <param name="index">Index within the component to get the builder from.</param>
        /// <returns>Subcomponent builder for the specified index.</returns>
        public new ISubcomponentBuilder this[int index]
        {
            get { return _subcomponents[index]; }
        }

        /// <summary>Get the number of subcomponents in this component, including subcomponents with no content.</summary>
        public override int ValueCount
        {
            get { return _subcomponents.Max(kv => kv.Key); }
        }

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
            set { SetSubcomponents(value.ToArray()); }
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

                foreach (var subcomponent in _subcomponents.OrderBy(i => i.Key))
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

                return (result.Length == 0)
                    ? null
                    : result.ToString();
            }
            set { SetComponent(value); }
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
            return CloneInternal();
        }

        /// <summary>Deep clone this component.</summary>
        /// <returns>Clone of the component.</returns>
        IComponent IComponent.Clone()
        {
            return CloneInternal();
        }

        /// <summary>Deep clone this component.</summary>
        /// <returns>Clone of the component.</returns>
        ComponentBuilder CloneInternal()
        {
            return new ComponentBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };            
        }

        /// <summary>Get the subcomponent delimiter.</summary>
        public override char Delimiter
        {
            get { return SubcomponentDelimiter; }
        }

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
        public override bool Exists
        {
            get { return _subcomponents.Any(s => s.Value.Exists); }
        }

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IRepetition IComponent.Ancestor
        {
            get { return Ancestor as IRepetition; }
        }

        /// <summary>Delete a descendant at the specified index.</summary>
        /// <param name="index">Index to delete at.</param>
        public override void Delete(int index)
        {
            DeleteDescendant(_subcomponents, index);
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public override IElement Insert(int index, IElement element)
        {
            return InsertDescendant(_subcomponents, index, element);
        }

        /// <summary>Insert a descendant element string.</summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public override IElement Insert(int index, string value)
        {
            return InsertDescendant(_subcomponents, index, value);
        }

        /// <summary>Move descendant to another index.</summary>
        /// <param name="sourceIndex">Source index.</param>
        /// <param name="targetIndex">Target index.</param>
        public override void Move(int sourceIndex, int targetIndex)
        {
            MoveDescendant(_subcomponents, sourceIndex, targetIndex);
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
    }
}