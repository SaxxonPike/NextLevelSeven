using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Base class for fields that have no descendants.</summary>
    internal abstract class StaticValueFieldParser : FieldParser
    {
        /// <summary>Create a field delimiter descendant.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index within the ancestor.</param>
        /// <param name="externalIndex">Exposed index.</param>
        protected StaticValueFieldParser(Parser ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        /// <summary>Delimiter is invalid for a field delimiter field.</summary>
        public override sealed char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>Returns an empty collection.</summary>
        public override sealed IEnumerable<IElementParser> Descendants
        {
            get { return Enumerable.Empty<IElementParser>(); }
        }

        /// <summary>Returns 1, since fields with static values cannot be divided further.</summary>
        public override int ValueCount
        {
            get { return 1; }
        }

        /// <summary>Get or set the value of this field.</summary>
        public abstract override string Value { get; set; }

        /// <summary>Get or set the value of this field wrapped in an enumerable.</summary>
        public abstract override IEnumerable<string> Values { get; set; }

        /// <summary>Get the field delimiter value.</summary>
        /// <param name="repetition">Not used.</param>
        /// <param name="component">Not used.</param>
        /// <param name="subcomponent">Not used.</param>
        /// <returns>Field delimiter value.</returns>
        public override sealed string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value;
        }

        /// <summary>Get the field delimiter value.</summary>
        /// <param name="repetition">Not used.</param>
        /// <param name="component">Not used.</param>
        /// <param name="subcomponent">Not used.</param>
        /// <returns>Field delimiter value.</returns>
        public override sealed IEnumerable<string> GetValues(int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return Value.Yield();
        }

        /// <summary>Get a repetition division of this field.</summary>
        /// <returns>Repetition descendant.</returns>
        protected override sealed RepetitionParser CreateRepetition(int index)
        {
            throw new ParserException(ErrorCode.FixedFieldsCannotBeDivided);
        }

        /// <summary>Deep clone this field.</summary>
        /// <returns>Cloned field.</returns>
        protected override sealed FieldParser CloneInternal()
        {
            return new FieldParser(EncodingConfiguration)
            {
                Index = Index,
                Value = Value
            };
        }
    }
}