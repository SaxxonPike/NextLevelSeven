using System.Collections.Generic;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents the special MSH-1 field, which contains the field delimiter for the rest of the segment.</summary>
    internal sealed class DelimiterFieldParser : StaticValueFieldParser
    {
        /// <summary>Create a field delimiter descendant.</summary>
        /// <param name="ancestor">Ancestor element.</param>
        public DelimiterFieldParser(Parser ancestor)
            : base(ancestor, 0, 1)
        {
        }

        /// <summary>Get or set the value of the field delimiter.</summary>
        public override string Value
        {
            get { return new string(Ancestor.DescendantDivider.Value[3], 1); }
            set { throw new ParserException(ErrorCode.ElementValueCannotBeChanged); }
        }

        /// <summary>
        ///     Get or set the value of the field delimiter. Only the first value is considered.
        /// </summary>
        public override IEnumerable<string> Values
        {
            get { yield return Value; }
            set { throw new ParserException(ErrorCode.ElementValueCannotBeChanged); }
        }
    }
}