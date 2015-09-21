using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Utility;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>
    ///     Represents a subcomponent-level element in an HL7 message.
    /// </summary>
    internal sealed class SubcomponentParser : ElementParser, ISubcomponentParser
    {
        public SubcomponentParser(ElementParser ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        private SubcomponentParser(string value, EncodingConfigurationBase config)
            : base(value, config)
        {
        }

        public override char Delimiter
        {
            get { return '\0'; }
        }

        public override int ValueCount
        {
            get { return 0; }
        }

        public override bool HasSignificantDescendants
        {
            get { return false; }
        }

        public string GetValue()
        {
            return Value;
        }

        public IEnumerable<string> GetValues()
        {
            return Value.Yield();
        }

        public override IElement Clone()
        {
            return CloneInternal();
        }

        ISubcomponent ISubcomponent.Clone()
        {
            return CloneInternal();
        }

        public override IElementParser GetDescendant(int index)
        {
            throw new ParserException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        private SubcomponentParser CloneInternal()
        {
            return new SubcomponentParser(Value, EncodingConfiguration) {Index = Index};
        }
    }
}