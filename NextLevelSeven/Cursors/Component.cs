using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    /// Represents a component level element of an HL7 message.
    /// </summary>
    internal class Component : Element
    {
        public Component(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private Component(string value)
            : base(value)
        {
        }

        public override IElement CloneDetached()
        {
            return new Component(Value);
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.SubcomponentDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SubcomponentIndexMustBeGreaterThanZero));
            }
            return new Subcomponent(this, index - 1, index);
        }
    }
}
