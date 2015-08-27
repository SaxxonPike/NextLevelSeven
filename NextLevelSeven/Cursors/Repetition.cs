using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Diagnostics;

namespace NextLevelSeven.Cursors
{
    sealed internal class Repetition : Element
    {
        public Repetition(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private Repetition(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override IElement CloneDetached()
        {
            return new Repetition(Value, EncodingConfiguration);
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.ComponentDelimiter; }
        }

        private readonly EncodingConfiguration _encodingConfigurationOverride;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            if (index < 1)
            {
                throw new ArgumentException(ErrorMessages.Get(ErrorCode.ComponentIndexMustBeGreaterThanZero));
            }
            return new Component(this, index - 1, index);
        }
    }
}
