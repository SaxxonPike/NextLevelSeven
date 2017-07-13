using System.Collections.Generic;
using System.Linq;
using NextLevelSeven.Core;
using NextLevelSeven.Core.Encoding;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Parsing.Elements
{
    /// <summary>Represents a subcomponent-level element in an HL7 message.</summary>
    internal sealed class SubcomponentParser : DescendantParser, ISubcomponentParser
    {
        /// <summary>Create a subcomponent with the specified ancestor and indices.</summary>
        /// <param name="ancestor">Ancestor component.</param>
        /// <param name="index">Index within the ancestor string divider.</param>
        /// <param name="externalIndex">Exposed index.</param>
        public SubcomponentParser(Parser ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        /// <summary>Create a subcomponent root with the specified encoding configuration.</summary>
        /// <param name="config">Encoding configuration to use.</param>
        private SubcomponentParser(ReadOnlyEncodingConfiguration config)
            : base(config)
        {
        }

        /// <summary>Returns an empty list since there are no descendants in a subcomponent.</summary>
        public override IEnumerable<IElementParser> Descendants => Enumerable.Empty<IElementParser>();

        /// <summary>Delimiter is invalid for subcomponents.</summary>
        public override char Delimiter => '\0';

        /// <summary>Returns 1, since subcomponents can't be divided further.</summary>
        public override int ValueCount => 1;

        /// <summary>Deep clone this element.</summary>
        /// <returns>Clone of the element.</returns>
        public override IElement Clone()
        {
            return CloneSubcomponent();
        }

        /// <summary>Deep clone this subcomponent.</summary>
        /// <returns>Clone of the subcomponent.</returns>
        ISubcomponent ISubcomponent.Clone()
        {
            return CloneSubcomponent();
        }

        /// <summary>Get this element's heirarchy-specific ancestor.</summary>
        IComponent ISubcomponent.Ancestor => Ancestor as IComponent;

        /// <summary>Get this element's heirarchy-specific ancestor parser.</summary>
        IComponentParser ISubcomponentParser.Ancestor => Ancestor as IComponentParser;

        /// <summary>Throws. Subcomponents have no descendants.</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override IElementParser GetDescendant(int index)
        {
            throw new ElementException(ErrorCode.SubcomponentCannotHaveDescendants);
        }

        /// <summary>Deep clone this subcomponent.</summary>
        /// <returns>Clone of the subcomponent.</returns>
        private SubcomponentParser CloneSubcomponent()
        {
            return new SubcomponentParser(EncodingConfiguration)
            {
                Index = Index,
                Value = Value
            };
        }
    }
}