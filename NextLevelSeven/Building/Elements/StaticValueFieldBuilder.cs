using System.Collections.Generic;
using System.Linq;
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

        public abstract override string Value { get; set; }

        /// <summary>Get the number of field repetitions in this field, including field repetitions with no content.</summary>
        public override int ValueCount => 1;

        /// <summary>Get or set field repetition content within this field.</summary>
        public override IEnumerable<string> Values
        {
            get { yield return Value; }
            set => Value = string.Concat(value);
        }

        /// <summary>Returns zero. Static fields cannot be divided any further. Therefore, they have no useful delimiter.</summary>
        public sealed override char Delimiter => '\0';

        /// <summary>If true, the element is considered to exist.</summary>
        public sealed override bool Exists => Value != null;

        protected override bool AssertIndexIsMovable(int index)
        {
            throw new ElementException(ErrorCode.EncodingElementCannotBeMoved);
        }

        /// <summary>Static fields cannot have repetitions; this method throws unconditionally.</summary>
        /// <param name="index">Not used.</param>
        /// <returns>Nothing.</returns>
        protected sealed override RepetitionBuilder CreateRepetitionBuilder(int index)
        {
            throw new ElementException(ErrorCode.FixedFieldsCannotBeDivided);
        }

        /// <summary>Set the contents of this field.</summary>
        /// <param name="value">New value.</param>
        /// <returns>This StaticValueFieldBuilder.</returns>
        public sealed override IFieldBuilder SetField(string value)
        {
            Value = value;
            return this;
        }

        /// <summary>Set the contents of this field.</summary>
        /// <param name="repetition"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public sealed override IFieldBuilder SetFieldRepetition(int repetition, string value)
        {
            if (repetition != 1)
            {
                throw new ElementException(ErrorCode.FixedFieldsCannotBeDivided);
            }
            Value = value;
            return this;
        }

        /// <summary>Get descendant elements.</summary>
        /// <returns>Descendant elements.</returns>
        protected sealed override IEnumerable<IElement> GetDescendants()
        {
            return Enumerable.Empty<IElement>();
        }
    }
}