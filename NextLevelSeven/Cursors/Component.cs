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

        private readonly Dictionary<int, IElement> _cache = new Dictionary<int, IElement>();

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
            if (_cache.ContainsKey(index))
            {
                return _cache[index];
            }

            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.SubcomponentIndexMustBeGreaterThanZero));
            }

            var result = new Subcomponent(this, index - 1, index);
            _cache[index] = result;
            return result;
        }
    }
}
