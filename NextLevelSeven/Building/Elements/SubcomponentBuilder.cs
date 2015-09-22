using System;
using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Codec;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>
    ///     Subcomponent HL7 element builder.
    /// </summary>
    internal sealed class SubcomponentBuilder : BuilderBaseDescendant, ISubcomponentBuilder
    {
        /// <summary>
        ///     Internal subcomponent value.
        /// </summary>
        private string _value;

        /// <summary>
        ///     Create a subcomponent builder.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal SubcomponentBuilder(BuilderBase builder, int index)
            : base(builder, index)
        {
        }

        /// <summary>
        ///     Set this subcomponent's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This SubcomponentBuilder, for chaining purposes.</returns>
        public ISubcomponentBuilder SetSubcomponent(string value)
        {
            _value = value;
            return this;
        }

        /// <summary>
        ///     Get or set the component string.
        /// </summary>
        public override string Value
        {
            get
            {
                return string.Equals("\"\"", _value, StringComparison.Ordinal)
                    ? null
                    : _value;
            }
            set { SetSubcomponent(value); }
        }

        /// <summary>
        ///     Returns 0 if null, and 1 otherwise.
        /// </summary>
        public override int ValueCount
        {
            get { return _value == null ? 0 : 1; }
        }

        /// <summary>
        ///     Return an enumerable with the content inside.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get { return new ProxyEnumerable<string>(i => _value, null, GetValueCount, 1); }
            set { _value = string.Concat(value); }
        }

        /// <summary>
        ///     Get this subcomponent's value.
        /// </summary>
        /// <returns>Subcomponent value.</returns>
        public string GetValue()
        {
            return Value;
        }

        /// <summary>
        ///     Get this subcomponent's value wrapped as an enumerable.
        /// </summary>
        /// <returns>Subcomponent value.</returns>
        public IEnumerable<string> GetValues()
        {
            return Value.Yield();
        }

        /// <summary>
        ///     Deep clone this element.
        /// </summary>
        /// <returns></returns>
        public override IElement Clone()
        {
            return new SubcomponentBuilder(Ancestor, Index) { Value = Value };
        }

        /// <summary>
        ///     Deep clone this subcomponent.
        /// </summary>
        /// <returns></returns>
        ISubcomponent ISubcomponent.Clone()
        {
            return new SubcomponentBuilder(Ancestor, Index) { Value = Value };
        }

        /// <summary>
        ///     Get a codec which allows interpretation of this subcomponent's value as other types.
        /// </summary>
        public override IEncodedTypeConverter As
        {
            get { return new EncodedTypeConverter(this); }
        }

        /// <summary>
        ///     Returns zero. Subcomponents cannot be divided any further. Therefore, they have no useful delimiter.
        /// </summary>
        public override char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>
        ///     Throws. Subcomponents cannot be divided any further.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override IElement GetGenericElement(int index)
        {
            throw new BuilderException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>
        ///     Returns an empty enumerable. Subcomponents cannot be divided any further.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }
    }
}