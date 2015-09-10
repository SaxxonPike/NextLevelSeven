using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Building
{
    /// <summary>
    /// A fixed field builder that notifies a segment builder when its value has changed.
    /// </summary>
    internal sealed class TypeFieldBuilder : FieldBuilder
    {
        /// <summary>
        ///     Create a field builder with the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        internal TypeFieldBuilder(BuilderBase builder, Action<string, string> onTypeFieldChangedHandler)
            : base(builder)
        {
            _onTypeFieldChangedHandler = onTypeFieldChangedHandler;
        }

        /// <summary>
        /// Internal value.
        /// </summary>
        private string _value;

        /// <summary>
        /// Method to call when this field changes.
        /// </summary>
        private readonly Action<string, string> _onTypeFieldChangedHandler;

        /// <summary>
        ///     Get a descendant field repetition builder.
        /// </summary>
        /// <param name="index">Index within the field to get the builder from.</param>
        /// <returns>Field repetition builder for the specified index.</returns>
        override public IRepetitionBuilder this[int index]
        {
            get { throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided); }
        }

        /// <summary>
        /// Set the contents of this field.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This TypeFieldBuilder.</returns>
        public override IFieldBuilder Field(string value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Set the contents of this field.
        /// </summary>
        /// <param name="repetition">Repetition number. All values greater than one are invalid.</param>
        /// <param name="value">New value.</param>
        /// <returns>This TypeFieldBuilder.</returns>
        public override IFieldBuilder FieldRepetition(int repetition, string value)
        {
            if (repetition > 1)
            {
                throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }

        /// <summary>
        /// Get the field's contents as a string.
        /// </summary>
        /// <returns>Type as string.</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Get or set the field type value.
        /// </summary>
        override public string Value
        {
            get { return _value; }
            set
            {
                var oldValue = _value;
                var newValue = value;
                _onTypeFieldChangedHandler(oldValue, newValue);
                _value = newValue;
            }
        }
    }
}
