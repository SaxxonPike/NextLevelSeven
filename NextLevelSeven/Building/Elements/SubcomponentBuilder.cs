using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>Subcomponent HL7 element builder.</summary>
    internal sealed class SubcomponentBuilder : DescendantBuilder, ISubcomponentBuilder
    {
        /// <summary>Internal subcomponent value.</summary>
        private string _value;

        /// <summary>Create a subcomponent builder.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal SubcomponentBuilder(Builder builder, int index)
            : base(builder, index)
        {
        }

        private SubcomponentBuilder(IEncoding config, int index)
            : base(config, index)
        {
        }

        /// <summary>Set this subcomponent's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        public ISubcomponentBuilder SetSubcomponent(string value)
        {
            _value = value;
            return this;
        }

        /// <summary>Get or set the component string.</summary>
        public override string Value
        {
            get => HL7.NullValues.Contains(_value)
                ? null
                : _value;
            set => SetSubcomponent(value);
        }

        /// <summary>Returns 0 if null, and 1 otherwise.</summary>
        public override int ValueCount => 1;

        /// <summary>Return an enumerable with the content inside.</summary>
        public override IEnumerable<string> Values
        {
            get { yield return _value; }
            set => _value = string.Concat(value);
        }

        /// <summary>Deep clone this element.</summary>
        /// <returns></returns>
        public override IElement Clone()
        {
            return CloneSubcomponent();
        }

        /// <summary>Deep clone this subcomponent.</summary>
        /// <returns></returns>
        ISubcomponent ISubcomponent.Clone()
        {
            return CloneSubcomponent();
        }

        /// <summary>Returns zero. Subcomponents cannot be divided any further. Therefore, they have no useful delimiter.</summary>
        public override char Delimiter => '\0';

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists => _value != null;

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IComponent ISubcomponent.Ancestor => Ancestor as IComponent;

        /// <summary>Get this element's heirarchy-specific ancestor builder.</summary>
        IComponentBuilder ISubcomponentBuilder.Ancestor => Ancestor as IComponentBuilder;

        /// <summary>Deep clone this subcomponent.</summary>
        /// <returns></returns>
        private SubcomponentBuilder CloneSubcomponent()
        {
            return new SubcomponentBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };
        }

        protected override bool AssertIndexIsMovable(int index)
        {
            throw new ElementException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Throws. Subcomponents cannot be divided any further.</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override IElement GetGenericElement(int index)
        {
            throw new ElementException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Returns an empty enumerable. Subcomponents cannot be divided any further.</summary>
        /// <returns></returns>
        protected override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }

        /// <summary>
        ///     Shouldn't be reachable, but is required as part of being derived from Builder.
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected override IIndexedCache<Builder> GetCache()
        {
            throw new NotImplementedException();
        }
    }
}