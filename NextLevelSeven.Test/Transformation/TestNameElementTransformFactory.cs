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
            ReadString = readString;
        }

        private readonly string ReadString;

        public ElementTransform CreateTransform(IElement element)
        {
            return new TestNameElementTransform(element, ReadString);
        }
    }
}
