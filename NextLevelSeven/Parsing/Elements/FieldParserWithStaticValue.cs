using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Parsing.Dividers;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    internal abstract class FieldParserWithStaticValue : FieldParser
    {
        /// <summary>
        ///     Create a field delimiter descendant.
        /// </summary>
        /// <param name="ancestor">Ancestor element.</param>
        /// <param name="index">Index within the ancestor.</param>
        /// <param name="externalIndex">Exposed index.</param>
        protected FieldParserWithStaticValue(ParserBase ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        /// <summary>
        ///     Delimiter is invalid for a field delimiter field.
        /// </summary>
        public override sealed char Delimiter
        {
            get { return '\0'; }
        }

        /// <summary>
        ///     Get the field delimiter value.
        /// </summary>
        /// <param name="repetition">Not used.</param>
        /// <param name="component">Not used.</param>
        /// <param name="subcomponent">Not used.</param>
        /// <returns>Field delimiter value.</returns>
        public override sealed string GetValue(int repetition = -1, int component = -1, int subcomponent = -1)
        {
            return Value;
        }

        /// <summary>
        ///     Get the field delimiter value.
        /// </summary>
        /// <param name="repetition">Not used.</param>
        /// <param name="component">Not used.</param>
        /// <param name="subcomponent">Not used.</param>
        /// <returns>Field delimiter value.</returns>
        public override sealed IEnumerable<string> GetValues(int repetition = -1, int component = -1,
            int subcomponent = -1)
        {
            return Value.Yield();
        }

        /// <summary>
        ///     Returns an empty collection.
        /// </summary>
        public override IEnumerable<IElementParser> Descendants
        {
            get { return Enumerable.Empty<IElementParser>(); }
        }

        /// <summary>
        ///     Returns 1, since fields with static values cannot be divided further.
        /// </summary>
        public override int ValueCount
        {
            get { return 1; }
        }

        /// <summary>
        ///     Get a repetition division of this field.
        /// </summary>
        /// <returns>Repetition descendant.</returns>
        protected override sealed RepetitionParser CreateRepetition(int index)
        {
            return new RepetitionParser(this, index - 1, index, Core.Encoding.EncodingConfiguration.Empty);
        }

        /// <summary>
        ///     Deep clone this field.
        /// </summary>
        /// <returns>Cloned field.</returns>
        protected override sealed FieldParser CloneInternal()
        {
            return new FieldParser(EncodingConfiguration) {Index = Index, Value = Value};
        }

        /// <summary>
        ///     Get the value of this field wrapped in an enumerable.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get { yield return Value; }
            set { Value = string.Concat(value); }
        }
    }
}