using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Building.Elements
{
    internal abstract class StaticValueFieldBuilder : FieldBuilder
    {
        /// <summary>Create a field builder with the specified encoding configuration.</summary>
        /// <param name="builder">Ancestor builder.</param>
        /// <param name="index">Index in the ancestor.</param>
        internal StaticValueFieldBuilder(Builder builder, int index)
            : base(builder, index)
        {
        }

        abstract public override string Value { get; set; }

        /// <summary>Get the number of field repetitions in this field, including field repetitions with no content.</summary>
        public override int ValueCount
        {
            get { return 1; }
        }

        /// <summary>Get or set field repetition content within this field.</summary>
        public override IEnumerable<string> Values
        {
            get { yield return Value; }
            set { Value = string.Concat(value); }
        }

        /// <summary>Static fields cannot have repetitions; this method throws unconditionally.</summary>
        /// <param name="index">Not used.</param>
        /// <returns>Nothing.</returns>
        sealed protected override RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
        }

        /// <summary>Returns zero. Static fields cannot be divided any further. Therefore, they have no useful delimiter.</summary>
        sealed public override char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>If true, the element is considered to exist.</summary>
        sealed public override bool Exists
        {
            get { return Value != null; }
        }

        /// <summary>Set the contents of this field.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This StaticValueFieldBuilder.</returns>
        sealed public override IFieldBuilder SetField(string value)
        {
            Value = value;
            return this;
        }

        /// <summary>Set the contents of this field.</summary>
        /// <param name="repetition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        sealed public override IFieldBuilder SetFieldRepetition(int repetition, string value)
        {
            if (repetition != 1)
            {
                throw new BuilderException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }

        /// <summary>Get descendant elements.</summary>
        /// <returns>Descendant elements.</returns>
        sealed protected override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }
    }
}
