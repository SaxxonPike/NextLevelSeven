using Microsoft.VisualStudio.TestTools.UnitTesting;
using NextLevelSeven.Core;

namespace NextLevelSeven.Test.Building
{
    [TestClass]
    public sealed class SubcomponentBuilderTests : BuildingTestFixture
    {
        [TestMethod]
        public void SubcomponentBuilder_SetsValuesWithConcatenation()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            builder.Values = new[] {"a", "b", "c", "d"};
            Assert.AreEqual(builder.Value, "abcd");
        }

        [TestMethod]
        public void SubcomponentBuilder_MapsBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SubcomponentBuilder_MapsGenericBuilderAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1] as ISubcomponent;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

        [TestMethod]
        public void SubcomponentBuilder_MapsGenericAncestor()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1] as IElement;
            Assert.AreSame(builder, builder.Ancestor[1]);
        }

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

        [TestMethod]
        public void SubcomponentBuilder_CanBeCloned_Generic()
        {
            IElement builder = Message.Build(string.Format("MSH|^~\\&|{0}~{1}^{2}&{3}",
                Randomized.String(), Randomized.String(), Randomized.String(), Randomized.String()))[1][3][2][2][2];
            var clone = builder.Clone();
            Assert.AreNotSame(builder, clone, "Builder and its clone must not refer to the same object.");
            Assert.AreEqual(builder.ToString(), clone.ToString(), "Clone data doesn't match source data.");
        }

        [TestMethod]
        public void SubcomponentBuilder_HasOneValue()
        {
            var builder = Message.Build(ExampleMessages.Standard)[1][3][1][1][1];
            Assert.AreEqual(1, builder.ValueCount);
        }
    }
}