using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class SubcomponentBuilderTests : BuildingTestFixture
    {
        [TestMethod]
        public void SubcomponentBuilder_CanGetValue()
        {
            var val0 = Randomized.String();
            var val1 = Randomized.String();
            var builder =
                Message.Build(string.Format("MSH|^~\\&|{0}&{1}&{2}", val0, val1, Randomized.String()))[1][3][1][1][2];
            Assert.AreEqual(builder.Value, val1);
        }

        [TestMethod]
        public void SubcomponentBuilder_CanGetValues()
        {
            var val1 = Randomized.String();
            var val2 = Randomized.String();
            var val3 = Randomized.String();
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Randomized.String(), val1, val2, val3))[1][3][2][2][2];
            AssertEnumerable.AreEqual(builder.Values, new[] {val3});
        }

        [TestMethod]
        public void SubcomponentBuilder_CanBeCloned()
        {
            var builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Randomized.String(), Randomized.String(), Randomized.String(), Randomized.String()))[1][3][2][2][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }
    }
}