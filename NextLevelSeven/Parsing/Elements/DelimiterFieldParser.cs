using System.Linq;
using NextLevelSeven.Core;
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
            get
            {
                var value = Ancestor.DescendantDivider.Value;
                if (value != null && value.Length > 3)
                {
                    return new string(value[3], 1);
                }
                return null;
            }
            set
            {
                var s = Ancestor.DescendantDivider.Value;
                if (s == null || s.Length < 3)
                {
                    return;
                }
                if (HL7.NullValues.Contains(value))
                {
                    throw new ParserException(ErrorCode.FieldCannotBeNull);
                }
                var newValue = string.Concat(s.Substring(0, 3), value, (s.Length > 3 ? s.Substring(4) : string.Empty));
                Ancestor.DescendantDivider.Value = newValue;
            }
        }
    }
}