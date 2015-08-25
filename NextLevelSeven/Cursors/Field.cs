using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;

namespace NextLevelSeven.Cursors
{
    internal class Field : Element
    {
        public Field(Element ancestor, int parentIndex, int externalIndex)
            : base(ancestor, parentIndex, externalIndex)
        {
        }

        public Field(string value, EncodingConfiguration config)
            : base(value)
        {
            _encodingConfigurationOverride = new EncodingConfiguration(config);
        }

        public override IElement CloneDetached()
        {
            return new Field(Value, EncodingConfiguration);
        }

        public override char Delimiter
        {
            get { return EncodingConfiguration.RepetitionDelimiter; }
        }

        private readonly EncodingConfiguration _encodingConfigurationOverride;
        public override EncodingConfiguration EncodingConfiguration
        {
            get { return _encodingConfigurationOverride ?? Ancestor.EncodingConfiguration; }
        }

        public override IElement GetDescendant(int index)
        {
            if (index <= 0)
            {
                return new Repetition(new FieldWithoutRepetitions(Ancestor, ParentIndex, Index), 0, 0);
            }
            return new Repetition(this, index - 1, index);
        }
    }
}
