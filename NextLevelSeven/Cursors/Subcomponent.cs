using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    sealed internal class Subcomponent : Element
    {
        public Subcomponent(Element ancestor, int index, int externalIndex)
            : base(ancestor, index, externalIndex)
        {
        }

        private Subcomponent(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override IElement CloneDetached()
        {
            return new Subcomponent(Value, EncodingConfiguration);
        }

        protected override char Delimiter
        {
            get { return '\0'; }
        }

        public override int DescendantCount
        {
            get { return 0; }
        }

        private readonly EncodingConfiguration _encodingConfigurationOverride;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            return null;
        }
    }
}
