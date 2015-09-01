using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Core;
using NextLevelSeven.Transformation;

namespace NextLevelSeven.Test.Transformation
{
    class TestNameElementTransformFactory : IElementTransformFactory
    {
        public TestNameElementTransformFactory(string readString)
        {
            _readString = readString;
        }

        private readonly string _readString;

        public ElementTransform CreateTransform(IElement element)
        {
            return new TestNameElementTransform(element, _readString);
        }
    }
}
