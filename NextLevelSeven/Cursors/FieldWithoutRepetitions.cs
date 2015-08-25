using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class FieldWithoutRepetitions : Element
    {
        public FieldWithoutRepetitions(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        private FieldWithoutRepetitions(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override IElement CloneDetached()
        {
            return new FieldWithoutRepetitions(Value, EncodingConfiguration);
        }

        protected override char Delimiter
        {
            get { return '\0'; }
        }

        public override int DescendantCount
        {
            get
            {
                return 1;
            }
        }

        private readonly EncodingConfiguration _encodingConfigurationOverride;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return new Repetition(this, index - 1, index);
        }
    }
}
