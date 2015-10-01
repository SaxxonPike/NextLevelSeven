using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;

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
            get
            {
                return HL7.NullValues.Contains(_value)
                    ? null
                    : _value;
            }
            set { SetSubcomponent(value); }
        }

        /// <summary>Returns 0 if null, and 1 otherwise.</summary>
        public override int ValueCount
        {
            get { return 1; }
        }

        /// <summary>Return an enumerable with the content inside.</summary>
        public override IEnumerable<string> Values
        {
            get { yield return _value; }
            set { _value = string.Concat(value); }
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

        /// <summary>Deep clone this subcomponent.</summary>
        /// <returns></returns>
        SubcomponentBuilder CloneSubcomponent()
        {
            return new SubcomponentBuilder(new EncodingConfiguration(Encoding), Index)
            {
                Value = Value
            };            
        }

        /// <summary>Returns zero. Subcomponents cannot be divided any further. Therefore, they have no useful delimiter.</summary>
        public override char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists
        {
            get { return _value != null; }
        }

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IComponent ISubcomponent.Ancestor
        {
            get { return Ancestor as IComponent; }
        }

        /// <summary>Get this element's heirarchy-specific ancestor builder.</summary>
        IComponentBuilder ISubcomponentBuilder.Ancestor
        {
            get { return Ancestor as IComponentBuilder; }
        }

        /// <summary>Delete a descendant at the specified index.</summary>
        /// <param name="index">Index to delete at.</param>
        public override void Delete(int index)
        {
            throw new BuilderException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Insert a descendant element.</summary>
        /// <param name="element">Element to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public override IElement Insert(int index, IElement element)
        {
            throw new BuilderException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Insert a descendant element string.</summary>
        /// <param name="value">Value to insert.</param>
        /// <param name="index">Index to insert at.</param>
        public override IElement Insert(int index, string value)
        {
            throw new BuilderException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Move descendant to another index.</summary>
        /// <param name="sourceIndex">Source index.</param>
        /// <param name="targetIndex">Target index.</param>
        public override void Move(int sourceIndex, int targetIndex)
        {
            throw new BuilderException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Throws. Subcomponents cannot be divided any further.</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override IElement GetGenericElement(int index)
        {
            throw new BuilderException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Returns an empty enumerable. Subcomponents cannot be divided any further.</summary>
        /// <returns></returns>
        protected override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }
    }
}