using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents the special field at MSH-2, which contains encoding characters for a message.</summary>
    internal sealed class EncodingFieldParser : StaticValueFieldParser
    {
        /// <summary>Create an encoding field.</summary>
        /// <param name="ancestor"></param>
        public EncodingFieldParser(Parser ancestor)
            : base(ancestor, 1, 2)
        {
        }

        /// <summary>Get or set encoding characters.</summary>
        public override string Value
        {
            get { return Ancestor.DescendantDivider[1]; }
            set
            {
                // we are assuming MSH + MSH-1 are configured
                var s = Ancestor.DescendantDivider.Value;
                var length = s.IndexOf(EncodingConfiguration.FieldDelimiter, 4) - 4;
                var builder = new StringBuilder();
                builder.Append(s.Substring(0, 4));
                builder.Append(value);
                if (length > 0)
                {
                    builder.Append(s.Substring(4 + length));
                }
                Ancestor.DescendantDivider.Value = builder.ToString();
            }
        }

        /// <summary>Get or set this field's encoding characters.</summary>
        public override IEnumerable<string> Values
        {
            get { return Value.Select(c => new string(c, 1)); }
            set { Value = string.Concat(value); }
        }
    }
}