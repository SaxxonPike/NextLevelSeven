using NextLevelSeven.Core;
using NextLevelSeven.Native;
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

        public ElementTransform CreateTransform(INativeElement element)
        {
            return new TestNameElementTransform(element, _readString);
        }
    }
}
