using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

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
            get => Ancestor.DescendantDivider[1];
            set => throw new ElementException(ErrorCode.ElementValueCannotBeChanged);
        }

        /// <summary>Get or set this field's encoding characters.</summary>
        public override IEnumerable<string> Values
        {
            get { return Value.Select(c => new string(c, 1)); }
            set => throw new ElementException(ErrorCode.ElementValueCannotBeChanged);
        }
    }
}