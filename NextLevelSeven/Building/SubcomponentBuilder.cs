using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
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
        public ISubcomponentBuilder Subcomponent(string value)
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
                    : (_value ?? string.Empty);
            }
            set { Subcomponent(value); }
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
            get { return new WrapperEnumerable<string>(i => _value, (i, v) => { }, () => ValueCount, 1); }
            set { _value = string.Concat(value); }
        }

        public string GetValue()
        {
            return Value;
        }

        public IEnumerable<string> GetValues()
        {
            return Value.Yield();
        }

        public override IElement Clone()
        {
            return new SubcomponentBuilder(Ancestor, Index);
        }

        ISubcomponent ISubcomponent.Clone()
        {
            return new SubcomponentBuilder(Ancestor, Index);
        }

        public override IEncodedTypeConverter As
        {
            get { return new BuilderCodec(this); }
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>
        ///     Copy the contents of this builder to a string.
        /// </summary>
        /// <returns>Converted subcomponent.</returns>
        public override string ToString()
        {
            return Value;
        }

        protected override IElement GetGenericElement(int index)
        {
            return this[index];
        }
    }
}