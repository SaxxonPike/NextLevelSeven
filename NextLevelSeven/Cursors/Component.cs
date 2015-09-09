using System;
using System.Collections.Generic;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;
using NextLevelSeven.Native;

namespace NextLevelSeven.Cursors
{
    /// <summary>
    ///     Represents a component level element of an HL7 message.
    /// </summary>
    internal sealed class Component : Element
    {
        private readonly Dictionary<int, INativeElement> _cache = new Dictionary<int, INativeElement>();

        public Component(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private Component(string value)
            : base(value)
        {
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.SubcomponentDelimiter; }
        }

        public override EncodingConfiguration EncodingConfiguration
        {
            get { return Ancestor.EncodingConfiguration; }
        }

        public override INativeElement CloneDetached()
        {
            return new Component(Value);
        }

        public override INativeElement GetDescendant(int index)
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