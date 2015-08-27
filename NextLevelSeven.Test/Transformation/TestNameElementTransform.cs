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
            ReadValue = readValue;
        }

        private readonly string ReadValue;

        public override ElementTransform CloneTransform(IElement element)
        {
            return new TestNameElementTransform(element, ReadValue);
        }

        public override string Value
        {
            get { return ReadValue; }
            set { base.Value = value; }
        }
    }
}
