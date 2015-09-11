using System.Collections.Generic;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Building
{
    /// <summary>
    ///     A fixed field builder that notifies a segment builder when its value has changed.
    /// </summary>
    internal sealed class DelimiterFieldBuilder : FieldBuilder
    {
        /// <summary>
        ///     Create a field builder with the specified encoding configuration.
        /// </summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal DelimiterFieldBuilder(BuilderBase builder, int index)
            : base(builder, index)
        {
        }

        /// <summary>
        ///     Get a descendant field repetition builder.
        /// </summary>
        /// <param name="index">Index within the field to get the builder from.</param>
        /// <returns>Field repetition builder for the specified index.</returns>
        public override IRepetitionBuilder this[int index]
        {
            get { throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided); }
        }

        /// <summary>
        ///     Get the number of field repetitions in this field, including field repetitions with no content.
        /// </summary>
        public override int ValueCount
        {
            get { return 1; }
        }

        /// <summary>
        ///     Get or set field repetition content within this field.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get { return new WrapperEnumerable<string>(index => Value, (index, value) => Value = value, () => 1); }
            set { Field(string.Concat(value)); }
        }

        /// <summary>
        ///     Get or set the field type value.
        /// </summary>
        public override string Value
        {
            get { return new string(FieldDelimiter, 1); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    FieldDelimiter = '|';
                    return;
                }
                FieldDelimiter = value[0];
            }
        }

        /// <summary>
        ///     Set this field's content.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public override IFieldBuilder Field(string value)
        {
            Value = value ?? string.Empty;
            return this;
        }

        /// <summary>
        ///     Set the contents of this field.
        /// </summary>
        /// <param name="repetition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        ///     Copy the contents of this builder to a string.
        /// </summary>
        /// <returns>Converted field.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}