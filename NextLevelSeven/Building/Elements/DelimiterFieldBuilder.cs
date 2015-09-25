using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Building.Elements
{
    /// <summary>A fixed field builder that notifies a segment builder when its value has changed.</summary>
    internal sealed class DelimiterFieldBuilder : FieldBuilder
    {
        /// <summary>Create a field builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal DelimiterFieldBuilder(Builder builder, int index)
            : base(builder, index)
        {
        }

        /// <summary>Get the number of field repetitions in this field, including field repetitions with no content.</summary>
        public override int ValueCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>Get or set field repetition content within this field.</summary>
        public override IEnumerable<string> Values
        {
            get
            {
                yield return Value;
            }
            set
            {
                SetField(string.Concat(value));
            }
        }

        /// <summary>Get or set the field type value.</summary>
        public override string Value
        {
            get
            {
                return FieldDelimiter == '\0' ? null : new string(FieldDelimiter, 1);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    FieldDelimiter = '\0';
                    return;
                }
                FieldDelimiter = value[0];
            }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        public override bool Exists
        {
            get
            {
                return FieldDelimiter != '\0';
            }
        }

        /// <summary>Delimiter fields cannot have repetitions; this method throws unconditionally.</summary>
        /// <param name="index">Not used.</param>
        /// <returns>Nothing.</returns>
        protected override RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
        }

        /// <summary>Set this field's content.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This FieldBuilder, for chaining purposes.</returns>
        public override IFieldBuilder SetField(string value)
        {
            Value = value;
            return this;
        }

        /// <summary>Set the contents of this field.</summary>
        /// <param name="repetition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override IFieldBuilder SetFieldRepetition(int repetition, string value)
        {
            if (repetition > 1)
            {
                throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }

        /// <summary>Get descendant elements.</summary>
        /// <returns>Descendant elements.</returns>
        protected override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }
    }
}