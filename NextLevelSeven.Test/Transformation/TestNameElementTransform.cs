using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Transformation;

namespace NextLevelSeven.Test.Transformation
{
    class TestNameElementTransform : ElementTransform
    {
        public TestNameElementTransform(IElement element, string readValue)
            : base(element)
        {
            _readValue = readValue;
        }

        private readonly string _readValue;

        public override ElementTransform CloneTransform(IElement element)
        {
            return new TestNameElementTransform(element, _readValue);
        }

        public override string Value
        {
            get { return _readValue; }
            set { base.Value = value; }
        }
    }
}
