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
